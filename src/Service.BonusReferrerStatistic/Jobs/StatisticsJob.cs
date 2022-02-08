using System.Threading.Tasks;
using DotNetCoreDecorators;
using Microsoft.EntityFrameworkCore;
using MyNoSqlServer.Abstractions;
using Service.BonusCampaign.Domain.Models.Enums;
using Service.BonusRefererStatistic.Postgres;
using Service.BonusReferrerStatistic.Domain.Models;
using Service.BonusReferrerStatistic.Domain.Models.NoSql;
using Service.BonusReferrerStatistic.Services;
using Service.BonusRewards.Domain.Models;
using Service.ClientProfile.Domain.Models;
using Service.ClientProfile.Grpc;
using Service.ClientProfile.Grpc.Models.Requests;
using Service.FeeShareEngine.Domain.Models.Models;
using Service.IndexPrices.Client;

namespace Service.BonusReferrerStatistic.Jobs
{
    public class StatisticsJob
    {
        private readonly IConvertIndexPricesClient _convertIndexPrices;
        private readonly IClientProfileService _clientProfileClient;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
        private readonly IMyNoSqlServerDataWriter<ReferrerProfileNoSqlEntity> _dataWriter;
        public StatisticsJob(ISubscriber<ClientProfileUpdateMessage> clientProfileSubscriber, ISubscriber<FeePaymentEntity> feePaymentSubscriber, ISubscriber<ExecuteRewardMessage> rewardSubscriber, IConvertIndexPricesClient convertIndexPrices, IClientProfileService clientProfileClient, DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder, IMyNoSqlServerDataWriter<ReferrerProfileNoSqlEntity> dataWriter)
        {
            _convertIndexPrices = convertIndexPrices;
            _clientProfileClient = clientProfileClient;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            _dataWriter = dataWriter;
            clientProfileSubscriber.Subscribe(HandleProfileUpdates);
            feePaymentSubscriber.Subscribe(HandleFeePayments);
            rewardSubscriber.Subscribe(HandleRewards);
        }

        private async ValueTask HandleProfileUpdates(ClientProfileUpdateMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.OldProfile.ReferrerClientId) &&
                !string.IsNullOrWhiteSpace(message.NewProfile.ReferrerClientId))
            {
                var profile = await GetOrClientProfile(message.NewProfile.ReferrerClientId);
                profile.ReferralInvited += 1;
                await SaveProfile(profile);
            }
        }

        private async ValueTask HandleFeePayments(FeePaymentEntity message)
        {
            var profile = await GetOrClientProfile(message.ReferrerClientId);
            var price = _convertIndexPrices.GetConvertIndexPriceByPairAsync(message.AssetId, "USD"); //TODO: select asset?
            profile.CommissionEarned += (message.Amount * price.Price);
            await SaveProfile(profile);
        }
        
        private async ValueTask HandleRewards(ExecuteRewardMessage message)
        {
            if(string.IsNullOrEmpty(message.ClientId))
                return;

            if (message.RewardType == RewardType.ReferrerPaymentAbsolute.ToString())
            {
                var profile = await GetOrClientProfile(message.ClientId);
                var price = _convertIndexPrices.GetConvertIndexPriceByPairAsync(message.Asset, "USD"); //TODO: select asset?
                profile.BonusEarned += (message.AmountAbs * price.Price);
                await SaveProfile(profile);
            }

            if (message.RewardType == RewardType.FeeShareAssignment.ToString())
            {
                var clientProfile = await _clientProfileClient.GetOrCreateProfile(new GetClientProfileRequest()
                    { ClientId = message.ClientId });
                var profile = await (GetOrClientProfile(clientProfile.ReferrerClientId));
                profile.ReferralActivated += 1;
                await SaveProfile(profile);
            }
        }

        private async Task<ReferrerProfile> GetOrClientProfile(string clientId)
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            var profile = await context.ReferrerProfiles.FirstOrDefaultAsync(t => t.ClientId == clientId);
            return profile ?? new ReferrerProfile
            {
                ClientId = clientId,
                ReferralInvited = 0,
                ReferralActivated = 0,
                BonusEarned = 0,
                CommissionEarned = 0
            };
        }

        private async Task SaveProfile(ReferrerProfile profile)
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

            await _dataWriter.InsertOrReplaceAsync(ReferrerProfileNoSqlEntity.Create(profile));
            await _dataWriter.CleanAndKeepLastRecordsAsync(ReferrerProfileNoSqlEntity.GeneratePartitionKey(),
                Program.Settings.MaxCachedEntities);
            await context.UpsertAsync(new[] { profile });
        }
    }
}
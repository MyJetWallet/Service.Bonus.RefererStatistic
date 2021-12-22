using System.Threading.Tasks;
using MyNoSqlServer.DataReader;
using Service.BonusReferrerStatistic.Domain.Models.NoSql;
using Service.BonusReferrerStatistic.Grpc;
using Service.BonusReferrerStatistic.Grpc.Models;

namespace Service.BonusReferrerStatistic.Client
{
    public class NoSqlReferrerClient : IReferrerStatService
    {
        private readonly IReferrerStatService _grpcService;
        private readonly MyNoSqlReadRepository<ReferrerProfileNoSqlEntity> _reader;
        private readonly MyNoSqlReadRepository<ReferrerStatSettingsNoSqlEntity> _settingsReader;

        public NoSqlReferrerClient(IReferrerStatService grpcService, MyNoSqlReadRepository<ReferrerProfileNoSqlEntity> reader, MyNoSqlReadRepository<ReferrerStatSettingsNoSqlEntity> settingsReader)
        {
            _grpcService = grpcService;
            _reader = reader;
            _settingsReader = settingsReader;
        }

        public async Task<ReferrerStatResponse> GetReferrerStats(GetStatRequest request)
        {
           var entity = _reader.Get(ReferrerProfileNoSqlEntity.GeneratePartitionKey(),
                ReferrerProfileNoSqlEntity.GenerateRowKey(request.ClientId));

           var settings = _settingsReader.Get(ReferrerStatSettingsNoSqlEntity.GeneratePartitionKey(),ReferrerStatSettingsNoSqlEntity.GenerateRowKey());
           if (entity != null)
               return new ReferrerStatResponse
               {
                   ReferralInvited = entity.ReferrerProfile.ReferralInvited,
                   ReferralActivated = entity.ReferrerProfile.ReferralActivated,
                   BonusEarned = entity.ReferrerProfile.BonusEarned,
                   CommissionEarned = entity.ReferrerProfile.CommissionEarned,
                   Total = entity.ReferrerProfile.BonusEarned + entity.ReferrerProfile.CommissionEarned,
                   Weight = settings?.BannerWeight ?? 0
               };
           
           return await _grpcService.GetReferrerStats(request);
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Service.BonusRefererStatistic.Postgres;
using Service.BonusReferrerStatistic.Domain.Models.NoSql;
using Service.BonusReferrerStatistic.Grpc;
using Service.BonusReferrerStatistic.Grpc.Models;

namespace Service.BonusReferrerStatistic.Services
{
    public class ReferrerStatService: IReferrerStatService
    {
        private readonly ILogger<ReferrerStatService> _logger;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
        private readonly IMyNoSqlServerDataReader<ReferrerStatSettingsNoSqlEntity> _settingsReader;
        public ReferrerStatService(ILogger<ReferrerStatService> logger, DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder, IMyNoSqlServerDataReader<ReferrerStatSettingsNoSqlEntity> settingsReader)
        {
            _logger = logger;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            _settingsReader = settingsReader;
        }

        public async Task<ReferrerStatResponse> GetReferrerStats(GetStatRequest request)
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            var profile = await context.ReferrerProfiles.FirstOrDefaultAsync(t => t.ClientId == request.ClientId);

            var settings = _settingsReader.Get(ReferrerStatSettingsNoSqlEntity.GeneratePartitionKey(),ReferrerStatSettingsNoSqlEntity.GenerateRowKey());
            if (profile == null)
                return new ReferrerStatResponse
                {
                    ReferralInvited = 0,
                    ReferralActivated = 0,
                    BonusEarned = 0,
                    CommissionEarned = 0,
                    Total = 0,
                    Weight = settings?.BannerWeight ?? 0
                };
            
            return new ReferrerStatResponse
            {
                ReferralInvited = profile.ReferralInvited,
                ReferralActivated = profile.ReferralActivated,
                BonusEarned = Math.Round(profile.BonusEarned, 2),
                CommissionEarned = Math.Round(profile.CommissionEarned, 2),
                Total = Math.Round(profile.BonusEarned + profile.CommissionEarned, 2),
                Weight = settings?.BannerWeight ?? 0
            };
        }
    }
}

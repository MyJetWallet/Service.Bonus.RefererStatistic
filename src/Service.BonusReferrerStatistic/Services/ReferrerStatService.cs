using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.BonusRefererStatistic.Postgres;
using Service.BonusReferrerStatistic.Grpc;
using Service.BonusReferrerStatistic.Grpc.Models;

namespace Service.BonusReferrerStatistic.Services
{
    public class ReferrerStatService: IReferrerStatService
    {
        private readonly ILogger<ReferrerStatService> _logger;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;

        public ReferrerStatService(ILogger<ReferrerStatService> logger, DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder)
        {
            _logger = logger;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        public async Task<ReferrerStatResponse> GetReferrerStats(GetStatRequest request)
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            var profile = await context.ReferrerProfiles.FirstOrDefaultAsync(t => t.ClientId == request.ClientId);

            if (profile == null)
                return new ReferrerStatResponse
                {
                    ReferralInvited = 0,
                    ReferralActivated = 0,
                    BonusEarned = 0,
                    CommissionEarned = 0,
                    Total = 0
                };
            
            return new ReferrerStatResponse
            {
                ReferralInvited = profile.ReferralInvited,
                ReferralActivated = profile.ReferralActivated,
                BonusEarned = profile.BonusEarned,
                CommissionEarned = profile.CommissionEarned,
                Total = profile.BonusEarned + profile.CommissionEarned
            };
        }
    }
}

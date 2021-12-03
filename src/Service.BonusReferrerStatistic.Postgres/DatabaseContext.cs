using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyJetWallet.Sdk.Postgres;
using Service.BonusReferrerStatistic.Domain.Models;

namespace Service.BonusRefererStatistic.Postgres
{
    public class DatabaseContext : MyDbContext
    {
        public const string Schema = "referrerstats";

        public const string ProfilesTableName = "profiles";

        public DbSet<ReferrerProfile> ReferrerProfiles { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.Entity<ReferrerProfile>().ToTable(ProfilesTableName);
            modelBuilder.Entity<ReferrerProfile>().HasKey(e => e.ClientId);
            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> UpsertAsync(IEnumerable<ReferrerProfile> entities)
        {
            var result = await ReferrerProfiles.UpsertRange(entities).AllowIdentityMatch().RunAsync();
            return result;
        }
    }
}

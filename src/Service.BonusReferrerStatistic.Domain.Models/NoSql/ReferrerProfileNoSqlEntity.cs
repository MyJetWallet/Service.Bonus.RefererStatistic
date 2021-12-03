using MyNoSqlServer.Abstractions;

namespace Service.BonusReferrerStatistic.Domain.Models.NoSql
{
    public class ReferrerProfileNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-referrerstats-profile";

        public static string GeneratePartitionKey() => "ReferrerProfile";

        public static string GenerateRowKey(string clientId) => clientId;
        
        public ReferrerProfile ReferrerProfile { get; set; }

        public static ReferrerProfileNoSqlEntity Create(ReferrerProfile profile) =>
            new()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(profile.ClientId),
                ReferrerProfile = profile
            };
    }
}
using MyNoSqlServer.Abstractions;

namespace Service.BonusReferrerStatistic.Domain.Models.NoSql
{
    public class ReferrerStatSettingsNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-referrerstats-settings";

        public static string GeneratePartitionKey() => "ReferrerStats";

        public static string GenerateRowKey() => "Settings";
        
        public int BannerWeight { get; set; }

        public static ReferrerStatSettingsNoSqlEntity Create(int bannerWeight) =>
            new()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(),
                BannerWeight = bannerWeight
            };
    }
}
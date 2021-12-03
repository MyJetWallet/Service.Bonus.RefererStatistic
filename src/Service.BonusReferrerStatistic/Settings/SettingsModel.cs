using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.BonusReferrerStatistic.Settings
{
    public class SettingsModel
    {
        [YamlProperty("BonusReferrerStatistic.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("BonusReferrerStatistic.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("BonusReferrerStatistic.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }
        
        [YamlProperty("BonusReferrerStatistic.MyNoSqlWriterUrl")]
        public string MyNoSqlWriterUrl { get; set; }
        
        [YamlProperty("BonusReferrerStatistic.MyNoSqlReaderHostPort")]
        public string MyNoSqlReaderHostPort { get; set; }

        [YamlProperty("BonusReferrerStatistic.SpotServiceBusHostPort")]
        public string SpotServiceBusHostPort { get; set; }
        
        [YamlProperty("BonusReferrerStatistic.PostgresConnectionString")]
        public string PostgresConnectionString { get; set; }
        
        [YamlProperty("BonusReferrerStatistic.MaxCachedEntities")]
        public int MaxCachedEntities { get; set; }
        
        [YamlProperty("BonusReferrerStatistic.ClientProfileGrpcServiceUrl")]
        public string ClientProfileGrpcServiceUrl { get; set; }
        
    }
}

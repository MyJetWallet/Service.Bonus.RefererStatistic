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
    }
}

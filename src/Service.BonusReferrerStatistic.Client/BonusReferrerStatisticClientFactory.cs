using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using MyNoSqlServer.DataReader;
using Service.BonusReferrerStatistic.Domain.Models.NoSql;
using Service.BonusReferrerStatistic.Grpc;

namespace Service.BonusReferrerStatistic.Client
{
    [UsedImplicitly]
    public class BonusReferrerStatisticClientFactory: MyGrpcClientFactory
    {
        private readonly MyNoSqlReadRepository<ReferrerProfileNoSqlEntity> _reader;
        private readonly MyNoSqlReadRepository<ReferrerStatSettingsNoSqlEntity> _settingsReader;

        public BonusReferrerStatisticClientFactory(string grpcServiceUrl, MyNoSqlReadRepository<ReferrerProfileNoSqlEntity> reader, MyNoSqlReadRepository<ReferrerStatSettingsNoSqlEntity> settingsReader) : base(grpcServiceUrl)
        {
            _reader = reader;
            _settingsReader = settingsReader;
        }

        public IReferrerStatService GetReferralService()  => 
            _reader != null && _settingsReader != null
                ? new NoSqlReferrerClient(CreateGrpcService<IReferrerStatService>(), _reader, _settingsReader) 
                : CreateGrpcService<IReferrerStatService>();
    }
}

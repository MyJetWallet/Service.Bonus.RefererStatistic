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

        public BonusReferrerStatisticClientFactory(string grpcServiceUrl, MyNoSqlReadRepository<ReferrerProfileNoSqlEntity> reader) : base(grpcServiceUrl)
        {
            _reader = reader;
        }

        public IReferrerStatService GetReferralService()  => 
            _reader != null  
                ? new NoSqlReferrerClient(CreateGrpcService<IReferrerStatService>(), _reader) 
                : CreateGrpcService<IReferrerStatService>();
    }
}

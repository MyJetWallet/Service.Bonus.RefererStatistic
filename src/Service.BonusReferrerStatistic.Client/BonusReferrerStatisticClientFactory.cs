using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.BonusReferrerStatistic.Grpc;

namespace Service.BonusReferrerStatistic.Client
{
    [UsedImplicitly]
    public class BonusReferrerStatisticClientFactory: MyGrpcClientFactory
    {
        public BonusReferrerStatisticClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public IHelloService GetHelloService() => CreateGrpcService<IHelloService>();
    }
}

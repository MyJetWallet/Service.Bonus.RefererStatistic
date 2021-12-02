using Autofac;
using Service.BonusReferrerStatistic.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.BonusReferrerStatistic.Client
{
    public static class AutofacHelper
    {
        public static void RegisterBonusReferrerStatisticClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new BonusReferrerStatisticClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetHelloService()).As<IHelloService>().SingleInstance();
        }
    }
}

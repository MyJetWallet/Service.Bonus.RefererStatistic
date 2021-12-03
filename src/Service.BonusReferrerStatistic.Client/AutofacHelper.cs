using Autofac;
using MyNoSqlServer.Abstractions;
using MyNoSqlServer.DataReader;
using Service.BonusReferrerStatistic.Domain.Models.NoSql;
using Service.BonusReferrerStatistic.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.BonusReferrerStatistic.Client
{
    public static class AutofacHelper
    {
        public static void RegisterBonusReferrerStatisticClientWithoutCache(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new BonusReferrerStatisticClientFactory(grpcServiceUrl, null);

            builder.RegisterInstance(factory.GetReferralService()).As<IReferrerStatService>().SingleInstance();
        }
        
        public static void RegisterBonusReferrerStatisticClient(this ContainerBuilder builder, string grpcServiceUrl, IMyNoSqlSubscriber myNoSqlSubscriber)
        {
            var subs = new MyNoSqlReadRepository<ReferrerProfileNoSqlEntity>(myNoSqlSubscriber, ReferrerProfileNoSqlEntity.TableName);

            var factory = new BonusReferrerStatisticClientFactory(grpcServiceUrl, subs);

            builder
                .RegisterInstance(subs)
                .As<IMyNoSqlServerDataReader<ReferrerProfileNoSqlEntity>>()
                .SingleInstance();
            
            builder
                .RegisterInstance(factory.GetReferralService())
                .As<IReferrerStatService>()
                .SingleInstance();
        }
    }
}

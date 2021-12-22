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
            var factory = new BonusReferrerStatisticClientFactory(grpcServiceUrl, null, null);

            builder.RegisterInstance(factory.GetReferralService()).As<IReferrerStatService>().SingleInstance();
        }
        
        public static void RegisterBonusReferrerStatisticClient(this ContainerBuilder builder, string grpcServiceUrl, IMyNoSqlSubscriber myNoSqlSubscriber)
        {
            var subs = new MyNoSqlReadRepository<ReferrerProfileNoSqlEntity>(myNoSqlSubscriber, ReferrerProfileNoSqlEntity.TableName);
            var settingsSubs = new MyNoSqlReadRepository<ReferrerStatSettingsNoSqlEntity>(myNoSqlSubscriber, ReferrerStatSettingsNoSqlEntity.TableName);

            var factory = new BonusReferrerStatisticClientFactory(grpcServiceUrl, subs, settingsSubs);

            builder
                .RegisterInstance(subs)
                .As<IMyNoSqlServerDataReader<ReferrerProfileNoSqlEntity>>()
                .SingleInstance();
            
            builder
                .RegisterInstance(settingsSubs)
                .As<IMyNoSqlServerDataReader<ReferrerStatSettingsNoSqlEntity>>()
                .SingleInstance();
            
            builder
                .RegisterInstance(factory.GetReferralService())
                .As<IReferrerStatService>()
                .SingleInstance();
        }
    }
}

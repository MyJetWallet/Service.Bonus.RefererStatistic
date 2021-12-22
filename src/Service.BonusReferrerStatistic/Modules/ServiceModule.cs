using Autofac;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.ServiceBus;
using MyServiceBus.Abstractions;
using Service.BonusReferrerStatistic.Domain.Models.NoSql;
using Service.BonusReferrerStatistic.Jobs;
using Service.BonusRewards.Domain.Models;
using Service.ClientProfile.Client;
using Service.ClientProfile.Domain.Models;
using Service.FeeShareEngine.Domain.Models.Models;
using Service.IndexPrices.Client;

namespace Service.BonusReferrerStatistic.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var serviceBusClient =
                builder.RegisterMyServiceBusTcpClient(Program.ReloadedSettings(t => t.SpotServiceBusHostPort),
                    Program.LogFactory);

            var queueName = "BonusReferrerStatistics";
            builder.RegisterMyServiceBusSubscriberSingle<ClientProfileUpdateMessage>(serviceBusClient,
                ClientProfileUpdateMessage.TopicName, queueName, TopicQueueType.PermanentWithSingleConnection);
            builder.RegisterMyServiceBusSubscriberSingle<FeePaymentEntity>(serviceBusClient, FeePaymentEntity.TopicName,
                queueName, TopicQueueType.PermanentWithSingleConnection);
            builder.RegisterMyServiceBusSubscriberSingle<ExecuteRewardMessage>(serviceBusClient,
                ExecuteRewardMessage.TopicName, queueName, TopicQueueType.PermanentWithSingleConnection);

            var myNoSqlClient = builder.CreateNoSqlClient(Program.ReloadedSettings(t => t.MyNoSqlReaderHostPort));
            builder.RegisterClientProfileClients(myNoSqlClient, Program.Settings.ClientProfileGrpcServiceUrl);
            builder.RegisterConvertIndexPricesClient(myNoSqlClient);
            builder.RegisterMyNoSqlWriter<ReferrerProfileNoSqlEntity>(Program.ReloadedSettings(t => t.MyNoSqlWriterUrl),
                ReferrerProfileNoSqlEntity.TableName);

            builder.RegisterMyNoSqlReader<ReferrerStatSettingsNoSqlEntity>(myNoSqlClient,
                ReferrerStatSettingsNoSqlEntity.TableName);

            builder.RegisterType<StatisticsJob>().AsSelf().SingleInstance().AutoActivate();
        }
    }
}
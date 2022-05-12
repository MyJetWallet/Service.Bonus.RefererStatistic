using System;
using Autofac;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.ServiceBus;
using MyServiceBus.Abstractions;
using Service.BonusReferrerStatistic.Domain.Models.NoSql;
using Service.BonusReferrerStatistic.Jobs;
using Service.BonusReferrerStatistic.Services;
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

            var profileDeduplicator =
                builder.RegisterMyServiceBusDeduplicator<ClientProfileUpdateMessage>(
                    t => t.Timestamp.ToString(),
                    Program.ReloadedSettings(t=>t.MyNoSqlWriterUrl),
                    queueName,
                    ClientProfileUpdateMessage.TopicName,
                    TimeSpan.FromHours(4),
                    Program.LogFactory);
            
            var feePaymentDeduplicator =
                builder.RegisterMyServiceBusDeduplicator<FeePaymentEntity>(
                    t => t.PaymentOperationId,
                    Program.ReloadedSettings(t=>t.MyNoSqlWriterUrl),
                    queueName,
                    FeePaymentEntity.TopicName,
                    TimeSpan.FromHours(4),
                    Program.LogFactory);
            
            var rewardDeduplicator =
                builder.RegisterMyServiceBusDeduplicator<ExecuteRewardMessage>(
                    t => t.RewardId,
                    Program.ReloadedSettings(t=>t.MyNoSqlWriterUrl),
                    queueName,
                    ExecuteRewardMessage.TopicName,
                    TimeSpan.FromHours(4),
                    Program.LogFactory);
            
            builder.RegisterMyServiceBusSubscriberSingle<ClientProfileUpdateMessage>(serviceBusClient,
                ClientProfileUpdateMessage.TopicName, queueName, TopicQueueType.PermanentWithSingleConnection, profileDeduplicator);
            builder.RegisterMyServiceBusSubscriberSingle<FeePaymentEntity>(serviceBusClient, FeePaymentEntity.TopicName,
                queueName, TopicQueueType.PermanentWithSingleConnection,feePaymentDeduplicator);
            builder.RegisterMyServiceBusSubscriberSingle<ExecuteRewardMessage>(serviceBusClient,
                ExecuteRewardMessage.TopicName, queueName, TopicQueueType.PermanentWithSingleConnection, rewardDeduplicator);

            var myNoSqlClient = builder.CreateNoSqlClient(Program.Settings.MyNoSqlReaderHostPort, Program.LogFactory);
            builder.RegisterClientProfileClients(myNoSqlClient, Program.Settings.ClientProfileGrpcServiceUrl);
            builder.RegisterConvertIndexPricesClient(myNoSqlClient);
            builder.RegisterMyNoSqlWriter<ReferrerProfileNoSqlEntity>(Program.ReloadedSettings(t => t.MyNoSqlWriterUrl),
                ReferrerProfileNoSqlEntity.TableName);
            builder.RegisterMyNoSqlWriter<MessageRecordsNoSqlEntity>(Program.ReloadedSettings(t => t.MyNoSqlWriterUrl),
                MessageRecordsNoSqlEntity.TableName);
            
            builder.RegisterMyNoSqlReader<ReferrerStatSettingsNoSqlEntity>(myNoSqlClient,
                ReferrerStatSettingsNoSqlEntity.TableName);

            builder.RegisterType<StatisticsJob>().AsSelf().SingleInstance().AutoActivate();
        }
    }
}
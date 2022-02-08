using System;
using System.Collections.Generic;
using MyNoSqlServer.Abstractions;

namespace Service.BonusReferrerStatistic.Domain.Models.NoSql
{
    public class MessageRecordsNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-message-records";

        public static string GeneratePartitionKey() => "Records";

        public static string GenerateRowKey(string messageId) => messageId;
        
        public string HandledMessageId { get; set; }

        public static MessageRecordsNoSqlEntity Create(string handledMessageId) =>
            new()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(handledMessageId),
                HandledMessageId = handledMessageId,
                Expires = DateTime.UtcNow.AddHours(6)
            };
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using MyNoSqlServer.Abstractions;
using Service.BonusReferrerStatistic.Domain.Models.NoSql;

namespace Service.BonusReferrerStatistic.Services
{
    public class MessageRecordsRegistry
    {
        private readonly List<string> _messages = new ();
        private readonly IMyNoSqlServerDataWriter<MessageRecordsNoSqlEntity> _writer;

        public MessageRecordsRegistry(IMyNoSqlServerDataWriter<MessageRecordsNoSqlEntity> writer)
        {
            _writer = writer;
        }

        public async Task<bool> IsHandled(string messageId)
        {
            if (!_messages.Any())
            {
                var messages = (await _writer.GetAsync()).Select(t=>t.HandledMessageId).ToList();
                _messages.AddRange(messages);
            }
            return _messages.Contains(messageId);
        }

        public async Task AddMessage(string messageId)
        {
            if (!_messages.Any())
            {
                var messages = (await _writer.GetAsync()).Select(t=>t.HandledMessageId).ToList();
                _messages.AddRange(messages);
            }
            _messages.Add(messageId);
            await _writer.InsertAsync(MessageRecordsNoSqlEntity.Create(messageId));
        }
    }
}
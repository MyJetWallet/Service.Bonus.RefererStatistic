using System.Runtime.Serialization;
using Service.BonusReferrerStatistic.Domain.Models;

namespace Service.BonusReferrerStatistic.Grpc.Models
{
    [DataContract]
    public class HelloMessage : IHelloMessage
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }
    }
}
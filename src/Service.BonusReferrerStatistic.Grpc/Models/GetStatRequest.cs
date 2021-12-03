using System.Runtime.Serialization;

namespace Service.BonusReferrerStatistic.Grpc.Models
{
    [DataContract]
    public class GetStatRequest
    {
        [DataMember(Order = 1)]
        public string ClientId { get; set; }
    }
}
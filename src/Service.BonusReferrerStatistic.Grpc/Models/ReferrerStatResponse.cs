using System.Runtime.Serialization;

namespace Service.BonusReferrerStatistic.Grpc.Models
{
    [DataContract]
    public class ReferrerStatResponse 
    {
        [DataMember(Order = 1)]public int ReferralInvited { get; set; }
        [DataMember(Order = 2)]public int ReferralActivated { get; set; }
        [DataMember(Order = 3)]public decimal BonusEarned { get; set; }
        [DataMember(Order = 4)]public decimal CommissionEarned { get; set; }
        [DataMember(Order = 5)]public decimal Total { get; set; }
    }
}
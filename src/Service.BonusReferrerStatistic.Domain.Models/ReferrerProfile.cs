namespace Service.BonusReferrerStatistic.Domain.Models
{
    public class ReferrerProfile
    {
        public string ClientId { get; set; }
        public int ReferralInvited { get; set; }
        public int ReferralActivated { get; set; }
        public decimal BonusEarned { get; set; }
        public decimal CommissionEarned { get; set; }
    }
}
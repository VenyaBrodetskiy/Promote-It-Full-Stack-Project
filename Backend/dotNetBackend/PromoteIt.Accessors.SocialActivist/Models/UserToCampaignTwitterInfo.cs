namespace PromoteIt.Accessors.SocialActivist.Models
{
    public record UserToCampaignTwitterInfo
    {
        public int UserId { get; set; }
        public int CampaignId { get; set; }
        public int CurrentTweetCount { get; set; }
    }
}

namespace PromoteIt.Engines.Twitter.Models
{
    public record CampaignInfo
    {
        public int Id { get; set; }
        public string Hashtag { get; set; }
        public string LandingPage { get; set; }
        public string NonProfitOrganizationName { get; set; }
    }
}

namespace PromoteIt.Accessors.Campaign.Models
{
    public record CampaignDTO
    {
        public int Id { get; set; }
        public string Hashtag { get; set; }
        public string LandingPage { get; set; }
        public string NonProfitOrganizationName { get; set; }
    }
}
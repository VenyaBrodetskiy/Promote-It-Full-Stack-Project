namespace TwitterAccessor
{
    public record CampaignInfo
    {
        public int CampaignId { get; set; }
        public string? Hashtag { get; set; }
        public string? LandingPage { get; set; }

    }
}

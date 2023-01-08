namespace dotNetBackend.DTO
{
    public record UserToCampaignBalanceDTO
    {
        public int Id { get; set; }

        public string? TwitterHandle { get; set; }

        public string? CampaignHashtag { get; set; }

        public int Balance { get; set; }

        public int PreviousTweetCount { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
namespace dotNetBackend.DTO
{
    public record TransactionDTO
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int CampaignId { get; set; }

        public int StateId { get; set; }
    }
}

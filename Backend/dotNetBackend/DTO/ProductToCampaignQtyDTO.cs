namespace dotNetBackend.DTO
{
    public record ProductToCampaignQtyDTO
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int CampaignId { get; set; }

        public int ProductQty { get; set; }
    }
}

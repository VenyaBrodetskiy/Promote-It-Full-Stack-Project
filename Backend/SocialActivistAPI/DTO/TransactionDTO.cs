namespace SocialActivistAPI.DTO
{
    public record TransactionDTO
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int StateId { get; set; }


    }
}

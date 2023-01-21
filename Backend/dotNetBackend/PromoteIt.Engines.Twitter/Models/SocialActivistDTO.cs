namespace PromoteIt.Engines.Twitter.Models
{
    public record SocialActivistDTO
    {
        public int UserId { get; set; }

        public string TwitterHandle { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
    }
}

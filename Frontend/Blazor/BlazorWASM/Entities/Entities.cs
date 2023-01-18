namespace BlazorWASM.Entities
{
    public class UserCredentials
    {
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class AuthToken
    {
        public string? Token { get; set; }
    }

    public class Campaign
    {
        public int Id { get; set; }
        public string Hashtag { get; set; }
        public string LandingPage { get; set; }
        public string NonProfitOrganizationName { get; set; }
    }

    public class NonProfitUser
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
    }

    public class BusinessOwner
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? TwitterHandle { get; set; }
    }

    public class SocialActivist
    {
        public int Id { get; set; }
        public string? TwitterHandle { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class Tweet
    {
        public string? User { get; set; }
        public DateTime PublishedOn { get; set; }
        public string? Link { get; set; }
        public int Retweets { get; set; }
        public string? Body { get; set; }
    }
}

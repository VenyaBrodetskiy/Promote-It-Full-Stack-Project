namespace BlazorWASM.Constants
{
    public static class Endpoints
    {
        // C# server
        public static string AllTweets { get; } = "https://localhost:7133/getAllTweets";

        // NodeJs server
        public static string Login { get; } = "http://localhost:6060/api/auth/login";
        public static string Campaigns { get; } = "http://localhost:6060/api/campaign/";
        public static string BusinessOwners { get; } = "http://localhost:6060/api/user/business-owner";
        public static string NonProfit { get; } = "http://localhost:6060/api/user/nonprofit-organization";
        public static string SocialActivists { get; } = "http://localhost:6060/api/user/social-activist";
    }

    public static class Page
    {
        public const string Home = "/home";
        public const string Campaigns = "/campaigns";
        public const string Login = "/";
        public const string Tweets = "/tweets";
        public const string Users = "/users";
    }

    public enum UserTypes
    {
        BusinessOwner = 1,
        SocialActivist = 2,
        NonprofitOrganization = 3,
        ProlobbyOwner = 4,
        System = 5
    }

    public static class TokenClaims
    {
        public static string UserId { get; } = "userId";
        public static string UserTypeId { get; } = "userTypeId";
    }

    public class Roles
    {
        public const string BusinessOwner = "BusinessOwner";
        public const string SocialActivist = "SocialActivist";
        public const string NonprofitOrganization = "NonprofitOrganization";
        public const string ProlobbyOwner = "ProlobbyOwner";
        public const string SystemBackendOnly = "SystemBackendOnly";
        public const string ProlobbyAndSystem = "ProlobbyAndSystem";
    }
}

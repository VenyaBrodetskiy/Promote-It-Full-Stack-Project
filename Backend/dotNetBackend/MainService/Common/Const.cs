namespace dotNetBackend.Common
{
    public class Const
    {
        public const string BaseUrl = "api";
        public const int NonExistId = 0;
    }

    public static class Endpoints
    {
        public static string TwitterCreateNewPost { get; } = "https://localhost:7128/create-tweet/";
        public static string TwitterEngineGetAllTweets { get; } = "https://localhost:7109/get-all-tweets";
        public static string TwitterEngineUpdateBalances { get; } = "https://localhost:7109/update-all-balances";
        public static string AccessorGetSocialActivists { get; } = "https://localhost:7170/get-all-social-activists";
        public static string AccessorChangeBalance { get; } = "https://localhost:7170/change-balance-user-to-campaign";

    }

    public static class TokenClaims
    {
        public static string UserId { get; } = "userId";
        public static string UserTypeId { get; } = "userTypeId";
    }

    public class Policies
    {
        public const string BusinessOwner = "BusinessOwner";
        public const string SocialActivist = "SocialActivist";
        public const string NonprofitOrganization = "NonprofitOrganization";
        public const string ProlobbyOwner = "ProlobbyOwner";
        public const string SystemBackendOnly = "SystemBackendOnly";
        public const string ProlobbyAndSystem = "ProlobbyAndSystem";
    }
}

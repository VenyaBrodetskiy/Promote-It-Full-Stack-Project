namespace dotNetBackend.Common
{
    public class Const
    {
        public const string BaseUrl = "api";
        public const int NonExistId = 0;
    }

    public static class EndpointsTwitter
    {
        public static string TwitterCreateNewPost { get; } = "https://localhost:7128/create-tweet/";
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

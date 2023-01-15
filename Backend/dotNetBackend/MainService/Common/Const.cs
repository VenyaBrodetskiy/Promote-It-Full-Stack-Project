namespace dotNetBackend.Common
{
    public class Const
    {
        public const string BaseUrl = "api";
        public const string TwitterCreateNewPost = "https://localhost:7133/create-tweet/";
        public const int NonExistId = 0;
    }

    public class TokenClaims
    {
        public const string UserId = "userId";
        public const string UserTypeId = "userTypeId";
    }

    public class Policies
    {
        public const string BusinessOwner = "BusinessOwner";
        public const string SocialActivist = "SocialActivist";
        public const string NonprofitOrganization = "NonprofitOrganization";
        public const string ProlobbyOwner = "ProlobbyOwner";
        public const string SystemBackendOnly = "SystemBackendOnly";
    }
}

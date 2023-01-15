namespace TwitterAccessor.Common
{
    public static class EndpointsMain
    {
        public static string ApiGetSocialActivists { get; } = "https://localhost:7121/api/SocialActivist/GetAll";
        public static string ApiPostUpdateUserBalance { get; } = "https://localhost:7121/api/SocialActivist/UpdateUserBalance";
    }

    public static class EndpointsNodeJs
    {
        public static string ApiLogin { get; } = "http://localhost:6060/api/auth/login";
        public static string ApiGetCampaigns { get; } = "http://localhost:6060/api/campaign";
    }
}

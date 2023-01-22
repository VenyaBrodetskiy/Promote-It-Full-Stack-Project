namespace PromoteIt.Engines.Twitter.Common
{
    public static class Endpoints
    {
        public static string AccessorGetCampaigns { get; } = "https://localhost:7009/get-all-campaigns";
        public static string AccessorGetSocialActivists { get; } = "https://localhost:7170/get-all-social-activists";
        public static string AccessorChangeBalance { get; } = "https://localhost:7170/change-balance-user-to-campaign";
        public static string AccessorGetTweetsByHashtag { get; } = "https://localhost:7128/getTweetsByHashtag";
    }
}

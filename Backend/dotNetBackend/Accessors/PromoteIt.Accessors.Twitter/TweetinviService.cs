using Tweetinvi;

namespace PromoteIt.Accessors.Twitter
{
    public class TweetinviService
    {
        private const string APIConsumerKey = "YgA9HWd0ZIzxYYFds5jSZUh1l";
        private const string APIConsumerSecret = "Ff3AeXnb75cFVFz1ogJRyM1f2HW3jgWQhUKjj82PhCR36Dzj2K";
        private const string APIAccessToken = "1606956857276473345-OOMGlxS04sibEchdsu7Igw1cIK8eXt";
        private const string APIAccessTokenSecret = "jEk11YfxbeJ2iB97eUUmhynlG16yi4PntdDmjy8siTy0K";
        public readonly TwitterClient userClient;

        public TweetinviService()
        {
            userClient = new TwitterClient(APIConsumerKey, APIConsumerSecret, APIAccessToken, APIAccessTokenSecret);
        }
    }
}

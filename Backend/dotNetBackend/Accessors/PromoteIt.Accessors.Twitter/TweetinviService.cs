using Tweetinvi;

namespace PromoteIt.Accessors.Twitter
{
    public class TweetinviService
    {
        //private const string APIConsumerKey = "3WiD7jcKBCKRhL1zWj2ycK4Jr";
        //private const string APIConsumerSecret = "Cipg1u63cKEWhLdlwIx4AeZdJDV1Jh7YGwg380qpz3Uzm4I3JT";
        //private const string APIAccessToken = "1606956857276473345-NIeOu9DIktWrqsMefFaz2pHXfeqIHq";
        //private const string APIAccessTokenSecret = "7J8u1j7oZj2667i2YcMOGrUNtPN6fK0csgAo7u3uNAYXX";
        public readonly TwitterClient userClient;

        public TweetinviService()
        {
            var APIConsumerKey = Environment.GetEnvironmentVariable("APIConsumerKey");
            var APIConsumerSecret = Environment.GetEnvironmentVariable("APIConsumerSecret");
            var APIAccessToken = Environment.GetEnvironmentVariable("APIAccessToken");
            var APIAccessTokenSecret = Environment.GetEnvironmentVariable("APIAccessTokenSecret");
            userClient = new TwitterClient(APIConsumerKey, APIConsumerSecret, APIAccessToken, APIAccessTokenSecret);
        }
    }
}

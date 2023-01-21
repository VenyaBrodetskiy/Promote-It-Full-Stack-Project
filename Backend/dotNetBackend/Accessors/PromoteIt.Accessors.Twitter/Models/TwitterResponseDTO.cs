using Tweetinvi.Client;
using Tweetinvi.Models.V2;

namespace PromoteIt.Accessors.Twitter.Models
{
    public record TwitterResponseDTO
    {
        public List<TweetDTO> Tweets { get; set; }
        public List<AuthorDTO> Authors { get; set; }
    }

    public record TweetDTO
    {
        public string AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }
        public PublicMetricsDTO PublicMetrics { get; set; }
    }

    public record PublicMetricsDTO
    {
        public int LikeCount { get; set; }
        public int QuoteCount { get; set; }
        public int ReplyCount { get; set; }
        public int RetweetCount { get; set; }
    }

    public record AuthorDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
    }
}

namespace PromoteIt.Engines.Twitter.Models
{
    public record FilteredTweetDTO
    {
        public string? User { get; set; }
        public DateTime PublishedOn { get; set; }
        public string? Link { get; set; }
        public string? Hashtag { get; set; }
        public int Retweets { get; set; }
        public string? Body { get; set; }
    }
}

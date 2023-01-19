namespace TwitterAccessor.Models
{
    public class TweetDTO
    {
        public string? User { get; set; }
        public DateTime PublishedOn { get; set; }
        public string? Link { get; set; }
        public int Retweets { get; set; }
        public string? Body { get; set; }
    }
}

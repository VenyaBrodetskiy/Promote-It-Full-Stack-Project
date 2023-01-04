namespace dotNetBackend.DTO
{
    public record ProductDTO
    {

        public string Title { get; set; } = null!;

        public int Price { get; set; }

        public int UserId { get; set; }
    }
}

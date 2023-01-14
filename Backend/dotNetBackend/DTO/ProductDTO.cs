namespace dotNetBackend.DTO
{
    public record NewProductDTO
    {
        public string Title { get; set; } = null!;

        public int Price { get; set; }
    }

    public record ProductDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public int Price { get; set; }
    }
}

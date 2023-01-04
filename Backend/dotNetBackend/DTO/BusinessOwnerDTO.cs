namespace dotNetBackend.DTO
{

public record BusinessOwnerDTO
    {
    public int UserId { get; set; }

    public string TwitterHandle { get; set; } = null!;

    public string Name { get; set; } = null!;

    }

}

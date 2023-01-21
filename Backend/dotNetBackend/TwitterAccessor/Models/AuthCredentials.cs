using TwitterAccessor.Common;

namespace TwitterAccessor.Models
{
    public record AuthCredentials
    {
        public string? Login { get; init; }
        public string? Password { get; init; } 
    }
}

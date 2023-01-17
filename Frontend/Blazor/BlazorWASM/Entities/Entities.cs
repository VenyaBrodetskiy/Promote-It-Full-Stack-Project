namespace BlazorWASM.Entities
{
    public class UserCredentials
    {
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class AuthToken
    {
        public string? Token { get; set; }
    }
}

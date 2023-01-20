using System.ComponentModel.DataAnnotations;

namespace BlazorWASM.Entities
{
    public class UserCredentials
    {
        [Required(ErrorMessage = "Login is required field")]
        public string Login { get; set; } 
        [Required(ErrorMessage = "Password is required field")]
        public string Password { get; set; } 
    }
}

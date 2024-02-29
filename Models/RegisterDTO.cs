using System.ComponentModel.DataAnnotations;

namespace JokesApi.Models
{
    public class RegisterDTO
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        [DataType("Password")]
        public string Password { get; set; } = null!;
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;

    }
}
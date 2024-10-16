using System.ComponentModel.DataAnnotations;

namespace Hexachess.Models
{
    public class RegisterViewModel : ILoginRegister
    {
        [Required] [StringLength(24, MinimumLength = 3, ErrorMessage = "Must be between 3 and 24 characters")]
        public string Username { get; set; }

        [Required] [StringLength(255, MinimumLength = 8, ErrorMessage = "Must be between 8 and 255 characters")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string PasswordRepeated { get; set; }
    }
}
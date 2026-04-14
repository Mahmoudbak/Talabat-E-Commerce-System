using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entity;

namespace Talabat.APIs.Dtos
{
    public class RegisterDto 
    {
        [Required]
        public string DisplayName { get; set; } = null!;



        [Required]
        [EmailAddress]
        public string Email { get; set; }=null!;


        [Required]
        public string Phone { get; set; } = null!;


        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9])\S{8,64}$",
        ErrorMessage = "Password must be 8–64 chars, with upper, lower, digit, and special character.")]
        public string Password { get; set; }
    }
}

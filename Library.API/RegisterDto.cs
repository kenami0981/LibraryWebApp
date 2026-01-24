using System.ComponentModel.DataAnnotations;

namespace Library.API
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one number, and one special character."
        )]
        public string Password { get; set; }

        [Required(ErrorMessage = "Display name is required")]
        [MinLength(2, ErrorMessage = "Display name must be at least 2 characters long")]
        public string DisplayName { get; set; }
    }
}


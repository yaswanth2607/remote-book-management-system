using System.ComponentModel.DataAnnotations;

namespace Privacy_Folder.Models
{
    public class LoginDetails
    {
        [Required]
        [RegularExpression(@"^[A-Za-z\s]{8,}$", ErrorMessage = "Name must be at least 8 characters and contain only letters and spaces.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email!")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com|in|org|net)$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and include at least 1 uppercase letter, 1 number, and 1 special character.")]
        public string Pass { get; set; }
    }
}

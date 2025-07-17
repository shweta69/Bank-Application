using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App_DTOs
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Username must contain only English letters")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be a 10-digit number.")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Enter a valid email address")]
        public string EmailAddress { get; set; }
        public string Address { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
        ErrorMessage = "Password must contain upper, lower, number, special char and be 6+ characters long")]
        public string Password { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
    }
}

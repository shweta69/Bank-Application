using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App_DTOs
{
    public class EditDetailsDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be a 10-digit number.")]
        public string PhoneNumber { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        public string Address { get; set; }
    }
}

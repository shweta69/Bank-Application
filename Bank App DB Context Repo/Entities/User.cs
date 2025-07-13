using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_App_DB_Context_Repo.Entities
{
    public class User
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] //removes identity
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Customer ID must be a 6-digit number.")]
        public int CustomerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be a 10-digit number.")]
        public string PhoneNumber { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}

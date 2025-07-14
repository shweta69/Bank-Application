using System.ComponentModel.DataAnnotations;

namespace Bank_App_DTOs
{
    public class CreateAccountDto
    {
        [Required]
        public AccountTypes AccountType { get; set; }

        [Required]
        [MaxLength(100)]
        public decimal InitialBalance { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bank_App_DTOs;

namespace Bank_App_DB_Context_Repo.Entities
{
    public class Account
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] //removes identity
        public int AccountId { get; set; }

        [Required]
        public int CustomerId { get; set; } // Foreign key to User
        [Required]
        public AccountTypes AccountType { get; set; } // e.g., Savings, Current

        [Required]
        [MaxLength(100)]
        public decimal Balance { get; set; }

        // Navigation property (optional but helpful)
        [ForeignKey("CustomerId")]
        public User User { get; set; }
    }
}

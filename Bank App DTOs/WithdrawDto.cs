using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App_DTOs
{
    public class WithdrawDto
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}

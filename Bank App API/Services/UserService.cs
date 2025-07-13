using Bank_App_DB_Context_Repo;
using Bank_App_DTOs;
namespace Bank_App_API.Services
{
    public class UserService: IUserService
    {
        private readonly UserDbContext _context;
        public UserService(UserDbContext context) {
            _context = context;
        }

        public Task<string> CreateAccountAsync(CreateAccountDto dto)
        {
            // Implementation for creating an account
            throw new NotImplementedException();
        }

        public Task<string> DepositAsync(DepositDto dto)
        {
            // Implementation for depositing money
            throw new NotImplementedException();
        }

        public Task<string> WithdrawAsync(WithdrawDto dto)
        {
            // Implementation for withdrawing money
            throw new NotImplementedException();
        }

        public Task<decimal> CheckBalanceAsync(int accountId)
        {
            // Implementation for checking balance
            throw new NotImplementedException();
        }

        public Task<string> EditDetailsAsync(EditDetailsDto dto)
        {
            // Implementation for editing account details
            throw new NotImplementedException();
        }

        public Task<string> ChangePasswordAsync(ChangePasswordDto dto)
        {
            // Implementation for changing password
            throw new NotImplementedException();
        }

        public Task<string> DeleteAccountAsync(int accountId)
        {
            // Implementation for deleting an account
            throw new NotImplementedException();
        }

        public Task<string> LogOutAsync(LogoutDto dto)
        {
            // Implementation for logging out
            throw new NotImplementedException();
        }
    }
}

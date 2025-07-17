using System.Security.Principal;
using Bank_App_DB_Context_Repo;
using Bank_App_DB_Context_Repo.Entities;
using Bank_App_DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
namespace Bank_App_API.Services
{
    public class UserService: IUserService
    {
        private readonly UserDbContext _context;
        public UserService(UserDbContext context) {
            _context = context;
        }
        public async Task<string> CreateAccountAsync(CreateAccountDto createAccount, int customerId)
        {
            try {
                if (!Enum.IsDefined(typeof(AccountTypes), createAccount.AccountType))
                    return "Invalid account type selected.";

                if (createAccount.InitialBalance < 5000)
                    return "Initial balance must be at least Rs.5000.";

                var userExists = await _context.Users.AnyAsync(u => u.CustomerId == customerId);
                if (!userExists)
                    return "Customer does not exist.";

                int accountId;
                do
                {
                    accountId = new Random().Next(100000, 999999);
                } while (await _context.Accounts.AnyAsync(a => a.AccountId == accountId));

                var newAccount = new Account
                {
                    AccountId = accountId,
                    CustomerId = customerId,
                    AccountType = createAccount.AccountType,
                    Balance = createAccount.InitialBalance
                };

                _context.Accounts.Add(newAccount);
                await _context.SaveChangesAsync();

                return $"Account created successfully. Your Account ID is: {accountId}";

            }
            catch (Exception ex)
            {
                return $"An error occurred while creating the account: {ex.Message}";
            }
        }


        public Task<string> DepositAsync(DepositDto depositDto)
        {
            if (depositDto.AccountId != 0 && depositDto.AccountId > 0)
            {
                // Check if the account exists
                var accountExist = _context.Accounts.AnyAsync(a => a.AccountId == depositDto.AccountId);
                if (!accountExist.Result)
                {
                    return Task.FromResult("Account does not exist.");
                }
                // Check if the amount is valid
                if (depositDto.Amount <= 0)
                {
                    return Task.FromResult("Deposit amount must be greater than zero.");
                }
            }
            else {
                return Task.FromResult("Please enter the valid six digit account Id");
            }

            // Find the account and update the balance
            var account = _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == depositDto.AccountId);
            account.Result.Balance += depositDto.Amount;
            _context.SaveChangesAsync();
            return Task.FromResult($"Successfully deposited {depositDto.Amount} to account {depositDto.AccountId}. New balance: {account.Result.Balance}");
            
        }

        public Task<string> WithdrawAsync(WithdrawDto withdrawDto)
        {
            if (withdrawDto.AccountId != 0 && withdrawDto.AccountId > 0)
            {
                // Check if the account exists
                var accountExist = _context.Accounts.AnyAsync(a => a.AccountId == withdrawDto.AccountId);
                if (!accountExist.Result)
                {
                    return Task.FromResult("Account does not exist.");
                }
                // Check if the amount is valid
                if (withdrawDto.Amount <= 0)
                {
                    return Task.FromResult("Withdrawal amount must be greater than zero.");
                }
            }
            else
            {
                return Task.FromResult("Please enter the valid six digit account Id");  
            }

            // Find the account and check balance
            var account = _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == withdrawDto.AccountId);
            if (account.Result.Balance < withdrawDto.Amount)
            {
                return Task.FromResult("Insufficient balance for withdrawal.");
            }
            // Update the balance
            account.Result.Balance -= withdrawDto.Amount;
            _context.SaveChangesAsync();
            return Task.FromResult($"Successfully withdrew {withdrawDto.Amount} from account {withdrawDto.AccountId}. New balance: {account.Result.Balance}");
        }

        public Task<decimal> CheckBalanceAsync(int accountId)
        {
            if (accountId != 0 && accountId > 0)
            {
                // Check if the account exists
                var accountExist = _context.Accounts.AnyAsync(a => a.AccountId == accountId);
                if (!accountExist.Result)
                {
                    return Task.FromResult(0m); // Account does not exist
                }
            }
            else
            {
                return Task.FromResult(0m); // Invalid account ID
            }

            // Find the account and return the balance
            var account = _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
            return Task.FromResult(account.Result.Balance);
        }

        public Task<string> EditDetailsAsync(EditDetailsDto editUserdto, int customerId)
        {
            if (customerId != 0 && customerId > 0)
            {
                // Check if the user exists
                var userExist = _context.Users.AnyAsync(u => u.CustomerId == customerId);
                if (!userExist.Result)
                {
                    return Task.FromResult("User does not exist.");
                }
            }
            else
            {
                return Task.FromResult("Please enter a valid six-digit customer ID.");
            }
            // Find the user and update details
            var user = _context.Users.FirstOrDefaultAsync(u => u.CustomerId == customerId);
            if (user.Result == null)
            {
                return Task.FromResult("User not found.");
            }
            user.Result.Name = editUserdto.Name ?? user.Result.Name;
            user.Result.PhoneNumber = editUserdto.PhoneNumber ?? user.Result.PhoneNumber;
            user.Result.EmailAddress = editUserdto.EmailAddress ?? user.Result.EmailAddress;
            user.Result.Address = editUserdto.Address ?? user.Result.Address;
            _context.SaveChangesAsync();
            return Task.FromResult("User details updated successfully.");
        }

        public async Task<string> ChangePasswordAsync(ChangePasswordDto dto, int customerId)
        {
            if (customerId <= 0)
                return "Please enter a valid six-digit customer ID.";

            var user = await _context.Users.FirstOrDefaultAsync(u => u.CustomerId == customerId);
            if (user == null)
                return "User not found.";

            // Verify current password
            var currentPasswordHash = HashPassword(dto.CurrentPassword);
            if (user.Password != currentPasswordHash)
                return "Current password is incorrect.";

            if (dto.Password != dto.ReEnterPassword)
                return "New password and confirm password do not match.";

            // Hash new password and save
            user.Password = HashPassword(dto.Password);
            await _context.SaveChangesAsync();
            return "Password changed successfully.";
        }

        // Simple hash function (use a stronger hash in production, e.g., BCrypt)
        private static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        

        public Task<string> DeleteAccountAsync(int accountId)
        {
            if (accountId != 0 && accountId > 0)
            {
                // Check if the account exists
                var accountExist = _context.Accounts.AnyAsync(a => a.AccountId == accountId);
                if (!accountExist.Result)
                {
                    return Task.FromResult("Account does not exist.");
                }
                // Find the account and delete it
                var account = _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
                _context.Accounts.Remove(account.Result);
                _context.SaveChangesAsync();
                return Task.FromResult($"Account {accountId} deleted successfully.");
            }
            else
            {
                return Task.FromResult("Please enter a valid six-digit account ID.");
            }
        }
    }
}

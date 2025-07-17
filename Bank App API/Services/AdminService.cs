using Bank_App_DB_Context_Repo;
using Bank_App_DB_Context_Repo.Entities;
using Bank_App_DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class AdminService : IAdminService
{
    private readonly UserDbContext _context;

    public AdminService(UserDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateAccountForUserAsync(CreateAccountDto dto, int customerId)
    {
        if (!Enum.IsDefined(typeof(AccountTypes), dto.AccountType))
            return "Invalid account type selected.";

        if (dto.InitialBalance < 5000)
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
            AccountType = dto.AccountType,
            Balance = dto.InitialBalance
        };

        _context.Accounts.Add(newAccount);
        await _context.SaveChangesAsync();

        return $"Account created successfully. Account ID: {accountId}";
    }

    public async Task<string> CheckBalanceAsync(int customerId, int accountId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.CustomerId == customerId);
        if (!userExists)
            return "Customer does not exist.";

        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId && a.CustomerId == customerId);
        if (account == null)
            return "Account does not exist for this user.";

        return $"Current balance for Account ID {accountId} is: ₹{account.Balance}";
    }

    public async Task<string> DeleteUserAsync(int customerId, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.CustomerId == customerId);
        if (user == null)
            return "User does not exist.";

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.Password, password);

        if (result != PasswordVerificationResult.Success)
            return "Incorrect password.";

        _context.Users.Remove(user); // Cascade delete will remove accounts
        await _context.SaveChangesAsync();
        return $"User {user.Name} and all associated accounts deleted successfully.";
    }

    public async Task<string> DeleteAccountAsync(int customerId, int accountId, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.CustomerId == customerId);
        if (user == null)
            return "User does not exist.";

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.Password, password);

        if (result != PasswordVerificationResult.Success)
            return "Incorrect password.";

        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId && a.CustomerId == customerId);
        if (account == null)
            return "Account does not exist for this user.";

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();
        return $"Account {accountId} deleted successfully.";
    }

    public async Task<List<UserSummaryDto>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .Where(u => !u.IsAdmin)
            .Select(u => new UserSummaryDto
            {
                Name = u.Name,
                NumberOfAccounts = _context.Accounts.Count(a => a.CustomerId == u.CustomerId)
            }).ToListAsync();

        return users;
    }

    public async Task<string> EditOwnDetailsAsync(EditDetailsDto dto, int customerId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.CustomerId == customerId);
        if (user == null)
            return "User not found.";

        user.Name = dto.Name ?? user.Name;
        user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;
        user.EmailAddress = dto.EmailAddress ?? user.EmailAddress;
        user.Address = dto.Address ?? user.Address;

        await _context.SaveChangesAsync();
        return "Details updated successfully.";
    }

    public async Task<string> ChangePasswordAsync(ChangePasswordDto dto, int customerId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.CustomerId == customerId);
        if (user == null)
            return "User not found.";

        var hasher = new PasswordHasher<User>();
        var verificationResult = hasher.VerifyHashedPassword(user, user.Password, dto.CurrentPassword);

        if (verificationResult != PasswordVerificationResult.Success)
            return "Current password is incorrect.";

        if (dto.Password != dto.ReEnterPassword)
            return "Passwords do not match.";

        user.Password = hasher.HashPassword(user, dto.Password);
        await _context.SaveChangesAsync();
        return "Password changed successfully.";
    }
}

using Bank_App_DTOs;

public interface IAdminService
{
    Task<string> CreateAccountForUserAsync(CreateAccountDto dto, int customerId);
    Task<string> CheckBalanceAsync(int customerId, int accountId);
    Task<string> DeleteUserAsync(int customerId, string password);
    Task<string> DeleteAccountAsync(int customerId, int accountId, string password);
    Task<List<UserSummaryDto>> GetAllUsersAsync(); 
    Task<string> EditOwnDetailsAsync(EditDetailsDto dto, int customerId);
    Task<string> ChangePasswordAsync(ChangePasswordDto dto, int customerId);
}

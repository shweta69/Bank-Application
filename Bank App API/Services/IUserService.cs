using Bank_App_DTOs;

namespace Bank_App_API.Services
{
    public interface IUserService
    {
        Task<string> CreateAccountAsync(CreateAccountDto dto, int CustomerId);
        Task<string> DepositAsync(DepositDto dto);
        Task<string> WithdrawAsync(WithdrawDto dto);
        Task<decimal> CheckBalanceAsync(int accountId);
        Task<string> EditDetailsAsync(EditDetailsDto dto, int customerId);
        Task<string> ChangePasswordAsync(ChangePasswordDto dto, int customerId);
        Task<string> DeleteAccountAsync(int accountId);
    }
}

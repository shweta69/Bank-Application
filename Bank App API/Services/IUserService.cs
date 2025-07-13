using Bank_App_DTOs;

namespace Bank_App_API.Services
{
    public interface IUserService
    {
        Task<string> CreateAccountAsync(CreateAccountDto dto);
        Task<string> DepositAsync(DepositDto dto);
        Task<string> WithdrawAsync(WithdrawDto dto);
        Task<decimal> CheckBalanceAsync(int accountId);
        Task<string> EditDetailsAsync(EditDetailsDto dto);
        Task<string> ChangePasswordAsync(ChangePasswordDto dto);
        Task<string> DeleteAccountAsync(int accountId);
        Task<string> LogOutAsync(LogoutDto dto);
    }
}

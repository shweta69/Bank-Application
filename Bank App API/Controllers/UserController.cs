using Bank_App_DTOs;
using Bank_App_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Bank_App_API.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create-account/{customerId}")]
        public async Task<IActionResult> CreateAccount(CreateAccountDto dto, int customerId)
        {
            var result = await _userService.CreateAccountAsync(dto, customerId);
            return Ok(result);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(DepositDto dto)
        {
            var result = await _userService.DepositAsync(dto);
            return Ok(result);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawDto dto)
        {
            var result = await _userService.WithdrawAsync(dto);
            return Ok(result);
        }

        [HttpGet("check-balance/{accountId}")]
        public async Task<IActionResult> CheckBalance(int accountId)
        {
            var result = await _userService.CheckBalanceAsync(accountId);
            return Ok(result);
        }

        [HttpPut("edit-details/{customerId}")]
        public async Task<IActionResult> EditDetails(EditDetailsDto dto, int customerId)
        {
            var result = await _userService.EditDetailsAsync(dto, customerId);
            return Ok(result);
        }

        [HttpPut("change-password/{customerId}")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto, int customerId)
        {
            var result = await _userService.ChangePasswordAsync(dto, customerId);
            return Ok(result);
        }

        [HttpDelete("delete-account/{accountId}")]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            var result = await _userService.DeleteAccountAsync(accountId);
            return Ok(result);
        }
    }
}

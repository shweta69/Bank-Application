using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bank_App_DTOs;

namespace Bank_App_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("create-account/{customerId}")]
        public async Task<IActionResult> CreateAccount(CreateAccountDto dto, int customerId)
        {
            var result = await _adminService.CreateAccountForUserAsync(dto, customerId);
            return Ok(result);
        }

        [HttpGet("check-balance/{customerId}/{accountId}")]
        public async Task<IActionResult> CheckBalance(int customerId, int accountId)
        {
            var result = await _adminService.CheckBalanceAsync(customerId, accountId);
            return Ok(result);
        }

        [HttpDelete("delete-user/{customerId}")]
        public async Task<IActionResult> DeleteUser(int customerId, [FromBody] string password)
        {
            var result = await _adminService.DeleteUserAsync(customerId, password);
            return Ok(result);
        }

        [HttpDelete("delete-account/{customerId}/{accountId}")]
        public async Task<IActionResult> DeleteAccount(int customerId, int accountId, [FromBody] string password)
        {
            var result = await _adminService.DeleteAccountAsync(customerId, accountId, password);
            return Ok(result);
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _adminService.GetAllUsersAsync();
            return Ok(result);
        }

        [HttpPut("edit-details/{customerId}")]
        public async Task<IActionResult> EditOwnDetails(EditDetailsDto dto, int customerId)
        {
            var result = await _adminService.EditOwnDetailsAsync(dto, customerId);
            return Ok(result);
        }

        [HttpPut("change-password/{customerId}")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto, int customerId)
        {
            var result = await _adminService.ChangePasswordAsync(dto, customerId);
            return Ok(result);
        }
    }
}

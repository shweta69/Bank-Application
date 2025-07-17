using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bank_App_DB_Context_Repo;
using Bank_App_DB_Context_Repo.Entities;
using Bank_App_DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Bank_App_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRegistration: ControllerBase
    {
        private readonly UserDbContext _userDb; //for calling db via ef
        private readonly IConfiguration _config; //for accessing the jwt tokken
        public UserRegistration(UserDbContext _userDb, IConfiguration _config)
        {
            this._userDb = _userDb;
            this._config = _config; 
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto user)
        {
            try
            {
                if (user == null)
                    return BadRequest("User data is null");

                // Generate unique CustomerId
                int customerId;
                do
                {
                    customerId = new Random().Next(100000, 999999);
                } while (await _userDb.Users.AnyAsync(u => u.CustomerId == customerId));

                var hasher = new PasswordHasher<User>();

                // With this alternative approach:
                var tempUser = new User
                {
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    EmailAddress = user.EmailAddress,
                    Address = user.Address,
                    IsAdmin = user.IsAdmin
                };
                var hashedPassword = hasher.HashPassword(tempUser, user.Password);

                var newUser = new User
                {
                    CustomerId = customerId,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    EmailAddress = user.EmailAddress,
                    Address = user.Address,
                    Password = hashedPassword,
                    IsAdmin = user.IsAdmin
                };

                await _userDb.Users.AddAsync(newUser);
                await _userDb.SaveChangesAsync();

                return Ok($"User registered successfully. Your Customer ID is: {customerId}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(int customerId, string password)
        {
            try
            {
                var result = await _userDb.Users.FirstOrDefaultAsync(u=> u.CustomerId == customerId);
                if (result == null)
                    return Unauthorized("Invalid credentials");

                var hasher = new PasswordHasher<User>();
                var verificationResult = hasher.VerifyHashedPassword(result, result.Password, password);

                if (verificationResult == PasswordVerificationResult.Success)
                {
                    Claim[] claims = new Claim[]
                    {
                new Claim(ClaimTypes.Name, result.Name),
                new Claim(ClaimTypes.Role, result.IsAdmin ? "Admin" : "User"),
                new Claim("CustomerId", result.CustomerId.ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _config["JwtSettings:Issuer"],
                        audience: _config["JwtSettings:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds);

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
                else
                {
                    return Unauthorized("Invalid credentials");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}

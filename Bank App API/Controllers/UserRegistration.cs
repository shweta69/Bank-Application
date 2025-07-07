using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bank_App_DB_Context_Repo;
using Bank_App_DB_Context_Repo.Entities;
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
        public async Task<IActionResult> RegisterUser(User user)
        {
            if (user == null)
                return BadRequest("User data is null");
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);
            var existingUser = await _userDb.Users.FirstOrDefaultAsync(u => u.CustomerId == user.CustomerId);
            if (existingUser != null)
                return Conflict("User with this Customer ID already exists");
            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, user.Password);
            await _userDb.Users.AddAsync(user);
            await _userDb.SaveChangesAsync();
            return Ok("user registered sucessfully");
            //return CreatedAtAction(nameof(RegisterUser), new { id = user.CustomerId }, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(string username, int customerId, string password)
        {
            var result = await _userDb.Users.FirstOrDefaultAsync(u => u.Name.ToLower().Trim() == username.ToLower().Trim() && u.CustomerId == customerId);
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
    }
}

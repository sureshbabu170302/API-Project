using APIprojectDoctorPatient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIprojectDoctorPatient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DPContext _context;

        private const string CustomerRole = "Customer";
        private const string AdminRole = "Admin";

        public TokensController(IConfiguration configuration, DPContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("Customer")]
        public async Task<IActionResult> Post(Customer _userData)
        {
            if (_userData != null && !string.IsNullOrEmpty(_userData.Customer_Email) && !string.IsNullOrEmpty(_userData.Customer_Password))
            {
                var user = await GetUser(_userData.Customer_Email, _userData.Customer_Password);

                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Customer_Id", user.Customer_Id.ToString()),
                        new Claim("Customer_Email", user.Customer_Email),
                        new Claim(ClaimTypes.Role, CustomerRole)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(2),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("Admin")]
        public async Task<IActionResult> PostAdmin(Admin adminData)
        {
            if (adminData != null && !string.IsNullOrEmpty(adminData.Admin_Email) && !string.IsNullOrEmpty(adminData.Admin_Password))
            {
                var admin = await GetAdmin(adminData.Admin_Email, adminData.Admin_Password);

                if (admin != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Admin_Id", admin.Admin_Id.ToString()),
                        new Claim("Admin_Email", admin.Admin_Email),
                        new Claim(ClaimTypes.Role, AdminRole)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(5),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<Customer> GetUser(string email, string password)
        {
            return await _context.Customers.FirstOrDefaultAsync(u => u.Customer_Email == email && u.Customer_Password == password);
        }

        private async Task<Admin> GetAdmin(string email, string password)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Admin_Email == email && a.Admin_Password == password);
        }
    }
}


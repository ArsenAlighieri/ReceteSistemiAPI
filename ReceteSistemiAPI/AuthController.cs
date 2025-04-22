using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReceteSistemiAPI;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReceteSistemiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MySqlDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(MySqlDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Register Endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (_context.Users.Any(u => u.Username == userDto.Username))
            {
                return BadRequest("Kullanıcı adı zaten mevcut.");
            }

            var user = new User
            {
                Username = userDto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password), // Şifreyi hash'le
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("Kullanıcı kaydedildi.");
        }

        // Login Endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == userDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password)) // Şifreyi doğrula
            {
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");
            }

            // JWT token oluşturuluyor
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("Role", "User")  // Eğer roller kullanıyorsanız, buraya rol ekleyebilirsiniz
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}

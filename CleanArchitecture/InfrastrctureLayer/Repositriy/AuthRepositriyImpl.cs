
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApplicationLayer.Dtos;
using ApplicationLayer.Repositriy;
using DomainLayer.Models;
using InfrastrctureLayer.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using static ApplicationLayer.Generic.mGeneric;

namespace InfrastrctureLayer.Repositriy
{
    public class AuthRepositriyImpl : IAuthRepositriy
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthRepositriyImpl> _logger;
        public AuthRepositriyImpl(ApplicationDbContext context, ILogger<AuthRepositriyImpl> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }
        public AuthRepositriyImpl(ApplicationDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<mApiResponse<string>> AuthenticateUser(string emailOruserName, string password)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == emailOruserName.ToLower() || u.UserName.ToLower() == emailOruserName.ToLower());
                if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                    return new mApiResponse<string>(401, "Invalid email or password");

                string token = GenerateJwtToken(user);
                return new mApiResponse<string>(200, "Authentication successful", token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during authentication.");
                return new mApiResponse<string>(500, "An unexpected error occurred.");
            }
        }

        public async Task<mApiResponse<string>> RegisterUser(RegisterRequest usrBody)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == usrBody.Email || u.UserName == usrBody.UserName))
                    return new mApiResponse<string>(400, "User already exists");

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(usrBody.PasswordHash);
                var user = new User {UserName = usrBody.UserName, PasswordHash = hashedPassword, Role = "Admin", RefreshTokenExpiryTime= DateTime.Now, Email = usrBody.Email, };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new mApiResponse<string>(200, "User registered successfully");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while updating the database.");
                return new mApiResponse<string>(500, "An error occurred while registering the user.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return new mApiResponse<string>(500, "An unexpected error occurred.");
            }
        }
        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, user.Email) }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:AccessTokenExpireMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

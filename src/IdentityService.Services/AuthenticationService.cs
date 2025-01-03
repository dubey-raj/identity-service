using IdentityService.DataStorage.DAL;
using IdentityService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using User = IdentityService.Model.User;

namespace IdentityService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly UsersContext _dbContext;

        public AuthenticationService(IConfiguration configuration, UsersContext usersContext)
        {
            _config = configuration;
            _dbContext = usersContext;
        }
        public (bool IsValid, User UserInfo) ValidateUser(string username, string password)
        {
            User? userDetail = null;
            var user = _dbContext.Users.Include(u=>u.Roles).FirstOrDefault(u => u.Email == username && u.PasswordHash == password);
            
            if (user != null)
            {
                userDetail = new User
                {
                    Id = user.Id,
                    UserName = user.Email,
                    Roles = user.Roles.Select(r => r.Name).ToArray()
                };
            }
            return (user != null, userDetail!);
        }

        public string GenerateToken(User user)
        {
            var secretKey = _config["Jwt:SecretKey"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            var refreshToken = GenerateRefreshToken();
            var currentTimeStamp = DateTime.Now;
            var expiryTime = currentTimeStamp.AddHours(1);
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, currentTimeStamp.ToString(), ClaimValueTypes.DateTime),
            new Claim(CustomClaimTypes.Role, string.Join(",", user.Roles)),
            new Claim(CustomClaimTypes.RefreshToken, refreshToken)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiryTime, // Token expires in 1 hour
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            SaveRefreshToken(user.Id, refreshToken, expiryTime);
            return accessToken;
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
        
        // Store refresh token in the database
        public void SaveRefreshToken(int userId, string refreshToken, DateTime expiry)
        {
            var token = new RefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                IssuedAt = DateTime.Now,
                ExpiresAt = expiry
            };
            _dbContext.RefreshTokens.Add(token);
            _dbContext.SaveChanges();
        }

        // Validate refresh token
        public async Task<bool> ValidateRefreshToken(int userId, string refreshToken)
        {
            var token = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == refreshToken && rt.IsRevoked == false);

            if (token == null || token.ExpiresAt < DateTime.Now)
            {
                return false;
            }

            return true;
        }
    }
}

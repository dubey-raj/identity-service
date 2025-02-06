using IdentityService.DataStorage.DAL;
using IdentityService.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Client = IdentityService.Model.Client;
using User = IdentityService.Model.User;

namespace IdentityService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UsersContext _dbContext;

        ///<inheritdoc/>
        public AuthenticationService(IConfiguration configuration, IPasswordHasher<User> passwordHasher, UsersContext usersContext)
        {
            _config = configuration;
            _passwordHasher = passwordHasher;
            _dbContext = usersContext;
        }

        ///<inheritdoc/>
        public (bool IsValid, User UserInfo) ValidateUser(string username, string password)
        {
            User? userDetail = null;
            var user = _dbContext.Users
                .Include(ur=>ur.UserRoles)
                .ThenInclude(ur=> ur.Role)
                .FirstOrDefault(u => u.Email == username);
            if (user == null)
            {
                return (false, userDetail!);
            }

            var result = _passwordHasher.VerifyHashedPassword(userDetail, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return (false, userDetail!);
            }

            userDetail = new User
            {
                Id = user.Id,
                UserName = user.Email,
                Roles = user.UserRoles.Select(r => r.Role.Name).ToArray()
            };

            return (true, userDetail!);
        }

        ///<inheritdoc/>
        public string CreateToken(User user)
        {
            DateTime expiryTime = DateTime.Now.AddHours(1);
            var refreshToken = GenerateRefreshToken();
            var claims = CreateClaims(user, refreshToken, expiryTime);
            var token = GenerateToken(claims);
            SaveToken(user.Id, refreshToken, expiryTime);
            return token;
        }

        ///<inheritdoc/>
        public (bool IsValid, Client ClientInfo) ValidateClient(string clientId, string clientSecret)
        {
            Client? client = null;
            var clientDetail = _dbContext.Clients
                .Include(cs => cs.ClientScopes).
                ThenInclude(cs => cs.Scope)
                .FirstOrDefault(c => c.ClientId == clientId && c.ClientSecret == clientSecret);

            if (clientDetail != null)
            {
                client = new Client
                {
                    Id = clientDetail.Id,
                    Name = clientDetail.ClientName,
                    Scopes = clientDetail.ClientScopes.Select(sc => sc.Scope.Name).ToArray()
                };
            }
            return (client != null, client!);
        }

        ///<inheritdoc/>
        public string CreateToken(Client client)
        {
            DateTime expiryTime = DateTime.Now.AddHours(1);
            var refreshToken = GenerateRefreshToken();
            var claims = CreateClaims(client, refreshToken, expiryTime);
            var token = GenerateToken(claims);
            //SaveToken(client.Id, refreshToken, expiryTime);
            return token;
        }

        ///<inheritdoc/>
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

        private Claim[] CreateClaims(User user, string refreshToken, DateTime expiryTime)
        {
            var currentTimeStamp = DateTime.Now;
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, currentTimeStamp.ToString(), ClaimValueTypes.DateTime),
            new Claim(CustomClaimTypes.Role, string.Join(",", user.Roles)),
            new Claim(CustomClaimTypes.RefreshToken, refreshToken)
            };

            return claims;
        }

        private Claim[] CreateClaims(Client client, string refreshToken, DateTime expiryTime)
        {
            var currentTimeStamp = DateTime.Now;
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, client.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, currentTimeStamp.ToString(), ClaimValueTypes.DateTime),
            new Claim(CustomClaimTypes.Scope, string.Join(",", client.Scopes))
            };

            return claims;
        }

        private string GenerateToken(Claim[] claims)
        {
            var secretKey = _config["Jwt:SecretKey"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            DateTime expiryTime = DateTime.Now.AddHours(1);
            var refreshToken = GenerateRefreshToken();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiryTime,
                signingCredentials: creds
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken;
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
        
        private void SaveToken(int userId, string refreshToken, DateTime expiry)
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
    }
}

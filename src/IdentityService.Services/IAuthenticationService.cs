using IdentityService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Validate User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>User details, null if passed credentials are invalid</returns>
        (bool IsValid, User UserInfo) ValidateUser(string username, string password);

        /// <summary>
        /// Validate Client
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns>User details, null if passed credentials are invalid</returns>
        (bool IsValid, Client ClientInfo) ValidateClient(string clientId, string clientSecret);

        /// <summary>
        /// Validate refresh token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<bool> ValidateRefreshToken(int userId, string refreshToken);

        /// <summary>
        /// Generate new JWT token
        /// </summary>
        /// <param name="user"></param>
        /// <returns>return JWT token</returns>
        string CreateToken(User user);

        /// <summary>
        /// Generate new JWT token
        /// </summary>
        /// <param name="client"></param>
        /// <returns>return JWT token</returns>
        string CreateToken(Client client);
    }
}

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
        /// Generate new JWT token
        /// </summary>
        /// <param name="user"></param>
        /// <returns>return JWT token</returns>
        string GenerateToken(User user);
    }
}

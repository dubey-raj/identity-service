using IdentityService.Model;

namespace IdentityService.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>User details, null if passed credentials are invalid</returns>
        UserResponse GetUserByIdAsync(int id);
    }
}

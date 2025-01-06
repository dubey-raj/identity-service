using IdentityService.DataStorage.DAL;
using IdentityService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Services
{
    public class UserService : IUserService
    {
        private readonly UsersContext _dbContext;

        ///<inheritdoc/>
        public UserService(IConfiguration configuration, UsersContext usersContext)
        {
            _dbContext = usersContext;
        }

        public UserResponse GetUserByIdAsync(int id)
        {
            UserResponse userResponse = null;
            var user = _dbContext.Users
                .Include(u => u.UserAddresses)
                .FirstOrDefault(u => u.Id == id);

            if (user != null) {
                userResponse = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    UserAddresses = user.UserAddresses.Select(ud => new Address
                    {
                        Id = ud.Id,
                        AddressLine1 = ud.AddressLine1,
                        AddressLine2 = ud.AddressLine2,
                        City = ud.City,
                        PostalCode = ud.PostalCode,
                        State = ud.State,
                        Country = ud.Country,
                        IsDefault = ud.IsDefault
                    }).ToList()
                };
            }
            return userResponse;
        }
    }
}

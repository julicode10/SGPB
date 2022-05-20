using Microsoft.AspNetCore.Identity;
using SGPB.Web.Data.Entities;
using SGPB.Web.Models;
using System.Threading.Tasks;

namespace SGPB.Web.Helpers
{
        public interface IUserHelper
        {
                Task<User> GetUserAsync(string email);

                Task<IdentityResult> AddUserAsync(User user, string password);

                Task CheckRoleAsync(string roleName);

                Task AddUserToRoleAsync(User user, string roleName);

                Task<bool> IsUserInRoleAsync(User user, string roleName);

                Task<SignInResult> LoginAsync(LoginViewModel model);

                Task LogoutAsync();

                Task<User> AddUserAsync(AddUserViewModel model);

        }

}

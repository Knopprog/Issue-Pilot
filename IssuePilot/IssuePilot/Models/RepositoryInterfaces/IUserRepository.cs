using IssuePilot.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using IssuePilot.Models.DBModels;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string Id);
        Task<(User, IdentityResult)> AddUserAsync(User newUser, string password);
        Task<List<User>> GetAllUsersAsync();
        Task<IdentityResult> UpdateUserAsync(AdministrationEditViewModel editUserModel);
        Task DeleteUserAsync(string userId);
        Task<IdentityResult> UpdatePasswordAsync(AdministrationChangePasswordViewModel changePasswordViewModel);
    }
}

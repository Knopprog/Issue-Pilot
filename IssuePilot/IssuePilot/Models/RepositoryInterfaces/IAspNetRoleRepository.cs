using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using IssuePilot.Models.DBModels;
using System.Collections.Generic;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface IAspNetRoleRepository
    {
        Task<IdentityResult> AddUserToIdentityRoleAsync(User user, string role);
        Task<IdentityResult> RemoveUserFromIdentityRoleAsync(User user, string role);
        Task<string> GetRoleOfUserByIdAsync(User user);
    }
}

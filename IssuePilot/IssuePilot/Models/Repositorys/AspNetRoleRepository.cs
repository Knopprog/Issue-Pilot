using IssuePilot.Data;
using IssuePilot.Models.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using IssuePilot.Models.DBModels;

namespace IssuePilot.Models.Repositorys
{
    public class AspNetRoleRepository : IAspNetRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AspNetRoleRepository(ApplicationDbContext context, UserManager<User> userManager, IUserRepository userRepository)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IdentityResult> AddUserToIdentityRoleAsync(User user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveUserFromIdentityRoleAsync(User user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<string> GetRoleOfUserByIdAsync(User user)
        {
            System.Collections.Generic.IList<string> roles = await _userManager.GetRolesAsync(user);
            return roles.Count == 0 ? "" : roles.First();
        }
    }
}

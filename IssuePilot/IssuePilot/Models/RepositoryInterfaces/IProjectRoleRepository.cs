using System.Collections.Generic;
using System.Threading.Tasks;
using IssuePilot.Models.DBModels;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface IProjectRoleRepository
    {
        Task<List<ProjectRole>> GetAllProjectRolesAsync();
        Task AddProjectMemberEntryAsync(User user, ProjectRoleId roleId, Project project);
        Task<List<Project>> GetProjectsOfUserByIdAsync(string userId);
        Task<List<User>> GetMembersOfProjectByIdAsync(int projectId);
        Task<bool> IsUserInProjectAsync(string userId, int projectId);
        Task<ProjectRole> GetProjectRoleOfUserAsync(string userId, int projectId);
        Task<List<User>> GetAllNonMembersOfProjectAsync(int projectId);
        Task UpdateProjectRoleAsync(int projectId, string userId, ProjectRoleId newRoleId);
        Task RemoveMemberFromProjectAsync(int projectId, string userId);
    }
}

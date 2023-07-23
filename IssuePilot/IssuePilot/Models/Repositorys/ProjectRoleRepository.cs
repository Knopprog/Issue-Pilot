using IssuePilot.Data;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssuePilot.Models.Repositorys
{
    public class ProjectRoleRepository : IProjectRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public ProjectRoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProjectMemberEntryAsync(User user, ProjectRoleId roleId, Project project)
        {
            ProjectRole dbRole = _context.ProjectRoles.Find(roleId);

            ProjectMemberEntry entry = new ProjectMemberEntry
            {
                Project = project,
                ProjectRole = dbRole,
                User = user
            };
            _context.ProjectMemberEntries.Add(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllNonMembersOfProjectAsync(int projectId)
        {
            var alreadyMember = await GetMembersOfProjectByIdAsync(projectId);
            return _context.Users.ToList().Except(alreadyMember).Where(c => c.IsDeleted == false).ToList();
        }

        public async Task<List<ProjectRole>> GetAllProjectRolesAsync()
        {
            return await _context.ProjectRoles.ToListAsync();
        }
        public async Task<List<Project>> GetProjectsOfUserByIdAsync(string userId)
        {
            var memberEntry = await _context.ProjectMemberEntries.Where(r => r.FK_UserId == userId).ToListAsync();
            List<Project> listOfProjects = new List<Project>();
            foreach (var member in memberEntry)
            {
                listOfProjects.Add(_context.Projects.Find(member.Project.Id));
            }
            return listOfProjects;
        }

        public async Task<List<User>> GetMembersOfProjectByIdAsync(int projectId)
        {
            var memberEntry = await _context.ProjectMemberEntries.Where(r => r.FK_ProjectId == projectId).ToListAsync();
            var listOfUsers = new List<User>();
            foreach (var entry in memberEntry)
            {
                listOfUsers.Add(await _context.Users.FindAsync(entry.FK_UserId));
            }
            return listOfUsers;
        }

        public async Task<ProjectRole> GetProjectRoleOfUserAsync(string userId, int projectId)
        {
            var entry = await _context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.Project.Id == projectId && r.User.Id == userId);
            return await _context.ProjectRoles.FindAsync(entry.ProjectRoleId);
        }

        public async Task<bool> IsUserInProjectAsync(string userId, int projectId)
        {
            var entry = await _context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.FK_ProjectId == projectId && r.FK_UserId == userId);
            return entry != null;
        }

        public async Task RemoveMemberFromProjectAsync(int projectId, string userId)
        {
            var entry = await _context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.FK_ProjectId == projectId && r.FK_UserId == userId);
            _context.ProjectMemberEntries.Remove(entry);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjectRoleAsync(int projectId, string userId, ProjectRoleId newRoleId)
        {
            var dbEntry = await _context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.FK_ProjectId == projectId && r.FK_UserId == userId);
            _context.ProjectMemberEntries.Remove(dbEntry);

            await AddProjectMemberEntryAsync(dbEntry.User, newRoleId, dbEntry.Project);
        }
    }
}

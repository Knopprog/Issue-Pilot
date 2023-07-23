using IssuePilot.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using IssuePilot.Models.DBModels;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface IProjectRepository
    {
        // Project
        Task<(ProjectUpdateViewModel, Project)> CreateProjectAsync(ProjectUpdateViewModel model, User currentUser);
        Task<Project> GetProjectByIdAsync(int id);
        Task<ProjectUpdateViewModel> UpdateProjectAsync(ProjectUpdateViewModel model);
        Task<List<Project>> GetAllProjectsAsync();
        Task DeleteProjectAsync(int id);

        // TicketCategories
        Task<List<TicketCategory>> GetAllTicketCategoriesOfProjectAsync(int projectId);
        Task CreateTicketCategoryAsync(string categorieTitle, Project project);
        Task UpdateTicketCategoryAsync(int categoryId, string name);
        Task<TicketCategory> GetTicketCategoryByIdAsync(int id);
        Task DeleteTicketCategoryAsync(int id);
    }
}

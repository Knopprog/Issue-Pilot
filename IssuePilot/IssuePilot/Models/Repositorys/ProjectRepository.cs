using IssuePilot.Data;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels;
using IssuePilot.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssuePilot.Models.Repositorys
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ITicketRepository _ticketRepository;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public ProjectRepository(ApplicationDbContext context, ITicketRepository ticketRepository)
        {
            _context = context;
            _ticketRepository = ticketRepository;
        }

        public async Task<(ProjectUpdateViewModel, Project)> CreateProjectAsync(ProjectUpdateViewModel model, User currentUser)
        {
            Project project = new Project();
            // Does title already exist?
            if (!await _context.Projects.AnyAsync(p => p.Title == model.Title))
            {
                project.Title = model.Title;
            }
            else
            {
                model.TitleExists = true;
                return (model, project);
            }
            project.Description = model.Description;
            project.CreateDate = DateTime.Now;
            project.DeletedTicketsCount = 0;
            project.Creator = currentUser;
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            var createdProject = await _context.Projects.FirstOrDefaultAsync(r => r.Title == project.Title);

            // Create default categories
            await CreateTicketCategoryAsync("Bug", createdProject);
            await CreateTicketCategoryAsync("Frage", createdProject);
            await CreateTicketCategoryAsync("Dokumentation", createdProject);
            await CreateTicketCategoryAsync("Diskussion", createdProject);
            await CreateTicketCategoryAsync("Feature", createdProject);

            return (model, createdProject);
        }

        public async Task CreateTicketCategoryAsync(string categoryTitle, Project project)
        {
            TicketCategory ticketCategory = new TicketCategory
            {
                Name = categoryTitle,
                Project = project
            };
            await _context.TicketCategories.AddAsync(ticketCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await GetProjectByIdAsync(id);
            var tickets = await _ticketRepository.GetTicketsOfProjectAsync(id);
            
            var entryList = await _context.NewsEntries.Where(n => n.Project.Id == id).ToListAsync();
            if (entryList != null)
            {
                _context.NewsEntries.RemoveRange(entryList);
            }

            foreach (var ticket in tickets.ToList())
            {
                await _ticketRepository.DeleteTicketAsync(ticket.Id);
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketCategoryAsync(int id)
        {
            _context.TicketCategories.Remove(await _context.TicketCategories.FindAsync(id));
            await _context.SaveChangesAsync();
        }
        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<List<TicketCategory>> GetAllTicketCategoriesOfProjectAsync(int projectId)
        {
            return await _context.TicketCategories.Where(r => r.Project.Id == projectId).ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task<TicketCategory> GetTicketCategoryByIdAsync(int id)
        {
            return await _context.TicketCategories.FindAsync(id);
        }

        public async Task<ProjectUpdateViewModel> UpdateProjectAsync(ProjectUpdateViewModel model)
        {
            var dBProject = await _context.Projects.FindAsync(model.Id);

            if (dBProject.Title != model.Title)
            {
                if (!await _context.Projects.AnyAsync(p => p.Title == model.Title))
                {
                    dBProject.Title = model.Title;
                }
                else
                {
                    model.TitleExists = true;
                    return model;
                }
            }
            dBProject.Description = model.Description;
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task UpdateTicketCategoryAsync(int categoryId, string name)
        {
            var dbCategory = await _context.TicketCategories.FindAsync(categoryId);
            dbCategory.Name = name;
            await _context.SaveChangesAsync();
        }
    }
}

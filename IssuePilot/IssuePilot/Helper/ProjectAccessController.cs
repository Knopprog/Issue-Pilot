using IssuePilot.Models.DBModels;
using IssuePilot.Models.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IssuePilot.Helper
{
    public abstract class ProjectAccessController : Controller
    {
        private readonly IProjectRoleRepository _projectRoleRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectAccessController(IProjectRoleRepository projectRoleRepository, IProjectRepository projectRepository)
        {
            _projectRoleRepository = projectRoleRepository;
            _projectRepository = projectRepository;
        }
        public IActionResult NoProjectAccess()
        {
            return View();
        }
        public async Task<bool> ProjectFunctionAccessAsync(int projectId)
        {
            if (User.IsInRole("Admin"))
            {
                return true;
            }
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var activeProjectRole = await _projectRoleRepository.GetProjectRoleOfUserAsync(currentUserId, projectId);
            // role is owner or user is not in project
            if (activeProjectRole == null || activeProjectRole.Id != ProjectRoleId.Owner)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> ProjectAccessAsync(int projectId)
        {
            if (User.IsInRole("Admin") || await _projectRoleRepository.IsUserInProjectAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, projectId))
            {
                return true;
            }
            return false;
        }

        public async Task<PaginatedList<Project>> GetProjectsAsync(string sortOrder, int? pageNumber, string currentFilter, string searchString)
        {
            ViewData["TitleSortParm"] = sortOrder == "Title" ? "title_desc" : "Title";
            ViewData["DescriptionSortParm"] = sortOrder == "Description" ? "description_desc" : "Description";
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Date" : "";
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Project> projects = new List<Project>();
            if (User.IsInRole("Admin"))
            {
                projects = await _projectRepository.GetAllProjectsAsync();
            }
            else
            {
                projects = await _projectRoleRepository.GetProjectsOfUserByIdAsync(currentUserId);
            }
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                List<Project> copy =  new List<Project>();
                copy.AddRange(projects);
                foreach (var project in copy)
                {
                    if (project.Description == null)
                    {
                        if (!project.Title.ToUpper().Contains(searchString))
                        {
                            projects.Remove(project);
                        }
                    }
                    else
                    {
                        if (!project.Title.ToUpper().Contains(searchString) && !project.Description.ToUpper().Contains(searchString))
                        {
                            projects.Remove(project);
                        }
                    }
                }
            }
            ViewData["CurrentSort"] = sortOrder;
            switch (sortOrder)
            {
                case "Date":
                    projects = projects.OrderBy(s => s.CreateDate).ToList();
                    break;
                case "Title":
                    projects = projects.OrderBy(s => s.Title).ToList();
                    break;
                case "Description":
                    projects = projects.OrderBy(s => s.Description).ToList();
                    break;
                case "title_desc":
                    projects = projects.OrderByDescending(s => s.Title).ToList();
                    break;
                case "description_desc":
                    projects = projects.OrderByDescending(s => s.Description).ToList();
                    break;
                default:
                    projects = projects.OrderByDescending(s => s.CreateDate).ToList();
                    break;
            }

            int pageSize = 20;
            return await PaginatedList<Project>.CreateFromListAsync(projects, pageNumber ?? 1, pageSize);
        }
    }
}

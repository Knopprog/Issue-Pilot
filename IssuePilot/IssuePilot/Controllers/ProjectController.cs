using IssuePilot.Helper;
using IssuePilot.Models.DBModels;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IssuePilot.Controllers
{
    [Authorize(Roles = "Admin,Projektmanager,Benutzer")]
    public class ProjectController : ProjectAccessController
    {
        private readonly IProjectRoleRepository _projectRoleRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IAspNetRoleRepository _aspNetRoleRepository;

        public ProjectController(IProjectRoleRepository projectRoleRepository, IProjectRepository projectRepository,
            IUserRepository userRepository, ITicketRepository ticketRepository, IDashboardRepository dashboardRepository, IAspNetRoleRepository aspNetRoleRepository) : base(projectRoleRepository, projectRepository)
        {
            _projectRoleRepository = projectRoleRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _dashboardRepository = dashboardRepository;
            _aspNetRoleRepository = aspNetRoleRepository;
        }

        // GET: Projects
        public async Task<IActionResult> Index(string sortOrder, int? pageNumber, string currentFilter, string searchString)
        {
            ProjectIndexViewModel model = new ProjectIndexViewModel()
            {
                Projects = await GetProjectsAsync(sortOrder, pageNumber, currentFilter, searchString),
            };
            return View(model);
        }
        // POST: Projects/Index
        [Authorize(Roles = "Admin,Projektmanager")]
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexCreate([FromForm] ProjectIndexViewModel projectModel)
        {
            if (ModelState.IsValid)
            {
                Project createdProject = null;
                ProjectUpdateViewModel createdModel = new ProjectIndexViewModel();
                User currentUser = await _userRepository.GetUserByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                (createdModel, createdProject) = await _projectRepository.CreateProjectAsync(projectModel, currentUser);
                var indexModel = (ProjectIndexViewModel)createdModel;
                if (indexModel.TitleExists == true)
                {
                    indexModel.Projects = await GetProjectsAsync(null, null, null, null);
                    return View(indexModel);
                }
                await _projectRoleRepository.AddProjectMemberEntryAsync(currentUser, ProjectRoleId.Owner, createdProject);
                await _dashboardRepository.CreateNewsEntryAsync(currentUser, NewsEntryCaseId.NewProject, createdProject, currentUser);
                return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = createdProject.Id }));
            }
            projectModel.Projects = await GetProjectsAsync(null, null, null, null);
            return View(projectModel);
        }


        // GET: Projects/Create
        [Authorize(Roles = "Admin,Projektmanager")]
        public IActionResult Create()
        {
            return View(new ProjectUpdateViewModel());
        }

        // POST: Projects/Create
        [Authorize(Roles = "Admin,Projektmanager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProjectUpdateViewModel projectModel)
        {
            if (ModelState.IsValid)
            {
                Project createdProject = null;
                ProjectUpdateViewModel createdModel = null;
                User currentUser = await _userRepository.GetUserByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                (createdModel, createdProject) = await _projectRepository.CreateProjectAsync(projectModel, currentUser);
                if (createdModel.TitleExists == true)
                {
                    return View(createdModel);
                }
                await _projectRoleRepository.AddProjectMemberEntryAsync(currentUser, ProjectRoleId.Owner, createdProject);
                await _dashboardRepository.CreateNewsEntryAsync(currentUser, NewsEntryCaseId.NewProject, createdProject, currentUser);
                return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = createdProject.Id }));
            }
            return View(projectModel);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            if (await ProjectFunctionAccessAsync(id))
            {
                ProjectUpdateViewModel model = new ProjectUpdateViewModel() { Id = project.Id, Title = project.Title, Description = project.Description };
                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(NoProjectAccess));
            }
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProjectUpdateViewModel projectModel)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectModel.Id);
            if (project == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (await ProjectFunctionAccessAsync(projectModel.Id))
                {
                    var updatedModel = await _projectRepository.UpdateProjectAsync(projectModel);
                    if (updatedModel.TitleExists == true)
                    {
                        return View(updatedModel);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = projectModel.Id }));
                    }
                }
                else
                {
                    return RedirectToAction(nameof(NoProjectAccess));
                }
            }
            return View(projectModel);
        }

        // POST: Projects/Other/5
        [HttpPost, ActionName("Other")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OtherEdit([FromForm] ProjectOtherViewModel projectOtherModel)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectOtherModel.Id);
            if (project == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (await ProjectFunctionAccessAsync(projectOtherModel.Id))
                {
                    var updatedModel = await _projectRepository.UpdateProjectAsync(projectOtherModel);
                    
                    if (updatedModel.TitleExists == true)
                    {
                        var newOtherModel = (ProjectOtherViewModel)updatedModel;
                        var categories = await _projectRepository.GetAllTicketCategoriesOfProjectAsync(project.Id);
                        newOtherModel.CreateDate = project.CreateDate;
                        newOtherModel.TicketCategories = categories;
                        newOtherModel.CreatorName = project.Creator.UserName;
                        newOtherModel.IsOwner = true;
                        newOtherModel.EditCategoryName = null;
                        newOtherModel.CreateCategoryName = null;
                        newOtherModel.ActualTitle = project.Title;
                        return View(newOtherModel);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = projectOtherModel.Id }));
                    }
                }
                else
                {
                    return RedirectToAction(nameof(NoProjectAccess));
                }
            }
            return View(projectOtherModel);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin,Projektmanager")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            if (await ProjectFunctionAccessAsync(id))
            {
                return View(project);
            }
            else
            {
                return RedirectToAction(nameof(NoProjectAccess));
            }
        }

        // POST: Projects/Delete/5
        [Authorize(Roles = "Admin,Projektmanager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (await ProjectFunctionAccessAsync(id))
            {
                await _projectRepository.DeleteProjectAsync(id);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(NoProjectAccess));
            }
        }

        // GET: Projects/Other/5
        public async Task<IActionResult> Other(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(id))
            {
                var categories = await _projectRepository.GetAllTicketCategoriesOfProjectAsync(id);
                ProjectOtherViewModel model = new ProjectOtherViewModel() { Id = project.Id, CreateDate = project.CreateDate, Description = project.Description, Title = project.Title, TicketCategories = categories, CreatorName = project.Creator.UserName, ActualTitle = project.Title };
                if (await ProjectFunctionAccessAsync(project.Id))
                {
                    model.IsOwner = true;
                }
                return View(model);
            }
            return RedirectToAction(nameof(NoProjectAccess));
        }

        // POST: Projects/DeleteCategory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = await _projectRepository.GetTicketCategoryByIdAsync(categoryId);
            if (category == null)
            {
                return NotFound();
            }
            if (await ProjectFunctionAccessAsync(category.Project.Id))
            {
                await _projectRepository.DeleteTicketCategoryAsync(categoryId);
            }
            else
            {
                return RedirectToAction(nameof(NoProjectAccess));
            }
            return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = category.Project.Id }));
        }

        // GET: Projects/CreateCategory
        public async Task<IActionResult> CreateCategory(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            if (await ProjectFunctionAccessAsync(id))
            {
                return View(new ProjectCategoryViewModel() { ProjectId = id });
            }
            else
            {
                return RedirectToAction(nameof(NoProjectAccess));
            }
        }

        // POST: Projects/CreateCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory([FromForm] ProjectCategoryViewModel model)
        {
            var project = await _projectRepository.GetProjectByIdAsync(model.ProjectId);
            if (project == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (project.TicketCategories.Count < 101)
                {
                    if (await ProjectFunctionAccessAsync(project.Id))
                    {
                        await _projectRepository.CreateTicketCategoryAsync(model.Name, project);
                    }
                    else
                    {
                        return RedirectToAction(nameof(NoProjectAccess));
                    }
                }
                return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = model.ProjectId }));
            }
            return View(model);
        }

        // POST: Projects/OtherCreateCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OtherCreateCategory([FromForm] ProjectOtherViewModel model)
        {
            var project = await _projectRepository.GetProjectByIdAsync(model.Id);
            if (project == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (project.TicketCategories.Count < 101)
                {
                    if (await ProjectFunctionAccessAsync(project.Id))
                    {
                        await _projectRepository.CreateTicketCategoryAsync(model.CreateCategoryName, project);
                    }
                    else
                    {
                        return RedirectToAction(nameof(NoProjectAccess));
                    }
                }
                return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = model.Id }));
            }
            return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = model.Id }));
        }

        // GET: Projects/EditCategory/5
        public async Task<IActionResult> EditCategory(int categoryId)
        {
            TicketCategory category = await _projectRepository.GetTicketCategoryByIdAsync(categoryId);
            if (category == null)
            {
                return NotFound();
            }
            if (await ProjectFunctionAccessAsync(category.Project.Id))
            {
                ProjectCategoryViewModel model = new ProjectCategoryViewModel() { CategoryId = category.Id, Name = category.Name, ProjectId = category.Project.Id };
                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(NoProjectAccess));
            }
        }
        // POST: Projects/EditCategory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory([FromForm] ProjectCategoryViewModel model)
        {
            TicketCategory category = await _projectRepository.GetTicketCategoryByIdAsync(model.CategoryId);
            if (category == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (await ProjectFunctionAccessAsync(category.Project.Id))
                {
                    await _projectRepository.UpdateTicketCategoryAsync(model.CategoryId, model.Name);
                    return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = category.Project.Id }));
                }
                else
                {
                    return RedirectToAction(nameof(NoProjectAccess));
                }
            }
            else
            {
                return View(model);
            }
        }
        // POST: Projects/Other/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OtherEditCategory([FromForm] ProjectOtherViewModel model)
        {
            TicketCategory category = await _projectRepository.GetTicketCategoryByIdAsync(model.EditCategoryId);
            if (category == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (await ProjectFunctionAccessAsync(category.Project.Id))
                {
                    await _projectRepository.UpdateTicketCategoryAsync(model.EditCategoryId, model.EditCategoryName);
                    return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = category.Project.Id }));
                }
                else
                {
                    return RedirectToAction(nameof(NoProjectAccess));
                }
            }
            else
            {
                return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = category.Project.Id }));
            }
        }

        // GET: Projects/Members/5
        public async Task<IActionResult> Members(int id, string sortOrder, int? pageNumber, string currentFilter, string searchString)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(id))
            {
                var members = await _projectRoleRepository.GetMembersOfProjectByIdAsync(id);

                List<ProjectMemberDataModel> projectMemberDataList = new List<ProjectMemberDataModel>();
                foreach (var member in members)
                {
                    ProjectMemberDataModel projectMemberData = new ProjectMemberDataModel
                    {
                        User = member,
                        ProjectRole = await _projectRoleRepository.GetProjectRoleOfUserAsync(member.Id, project.Id),
                        UserRole = await _aspNetRoleRepository.GetRoleOfUserByIdAsync(member)
                    };
                    projectMemberDataList.Add(projectMemberData);
                }
                ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
                ViewData["RoleSortParm"] = sortOrder == "Role" ? "role_desc" : "Role";
                ViewData["UserNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

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
                    projectMemberDataList = projectMemberDataList.Where(s => s.User.UserName.ToUpper().Contains(searchString)).ToList();
                }
                ViewData["CurrentSort"] = sortOrder;
                switch (sortOrder)
                {
                    case "Role":
                        projectMemberDataList = projectMemberDataList.OrderBy(s => s.ProjectRole.Id).ToList();
                        break;
                    case "role_desc":
                        projectMemberDataList = projectMemberDataList.OrderByDescending(s => s.ProjectRole.Id).ToList();
                        break;
                    case "Email":
                        projectMemberDataList = projectMemberDataList.OrderBy(s => s.User.Email).ToList();
                        break;
                    case "email_desc":
                        projectMemberDataList = projectMemberDataList.OrderByDescending(s => s.User.Email).ToList();
                        break;
                    case "name_desc":
                        projectMemberDataList = projectMemberDataList.OrderByDescending(s => s.User.UserName).ToList();
                        break;
                    default:
                        projectMemberDataList = projectMemberDataList.OrderBy(s => s.User.UserName).ToList();
                        break;
                }
                int pageSize = 20;

                ProjectMembersViewModel model = new ProjectMembersViewModel() { Project = project, Members = await PaginatedList<ProjectMemberDataModel>.CreateFromListAsync(projectMemberDataList, pageNumber ?? 1, pageSize), ProjectId = project.Id };
                if (await ProjectFunctionAccessAsync(project.Id))
                {
                    model.IsOwner = true;
                }
                // Is user(Admin) a member?
                string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                model.UserId = currentUserId;
                if (await _projectRoleRepository.IsUserInProjectAsync(currentUserId, id))
                {
                    model.IsMember = true;
                }
                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(NoProjectAccess));
            }

        }

        // POST: Projects/RemoveMembers/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(int projectId, string userId)
        {
            if (await _projectRoleRepository.IsUserInProjectAsync(userId, projectId) == false)
            {
                return NotFound();
            }
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            var userToRemove = await _userRepository.GetUserByIdAsync(userId);
            if (await ProjectFunctionAccessAsync(projectId))
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                User currentUser = await _userRepository.GetUserByIdAsync(currentUserId);
                var memberProjectRole = await _projectRoleRepository.GetProjectRoleOfUserAsync(userToRemove.Id, projectId);
                if (memberProjectRole.Id == ProjectRoleId.Member || User.IsInRole("Admin") || (User.IsInRole("Projektmanager") && await _aspNetRoleRepository.GetRoleOfUserByIdAsync(userToRemove) == "Benutzer") || userToRemove == currentUser)
                {
                    await _projectRoleRepository.RemoveMemberFromProjectAsync(projectId, userId);
                    await _dashboardRepository.CreateNewsEntryAsync(userToRemove, NewsEntryCaseId.RemovedFromProject, project, currentUser);
                    return RedirectToAction(nameof(Members), new RouteValueDictionary(new { controller = "Project", action = "Members", Id = projectId }));
                }
            }
            return RedirectToAction(nameof(NoProjectAccess));
        }

        // POST: Projects/LeaveProject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveProject(int projectId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!await _projectRoleRepository.IsUserInProjectAsync(currentUserId, projectId))
            {
                return NotFound();
            }
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            User user = await _userRepository.GetUserByIdAsync(currentUserId);
            await _projectRoleRepository.RemoveMemberFromProjectAsync(projectId, currentUserId);
            await _dashboardRepository.CreateNewsEntryAsync(user, NewsEntryCaseId.RemovedFromProject, project, user);

            return RedirectToAction(nameof(Index));
        }

        // GET: Projects/AddMember
        public async Task<IActionResult> AddMember(int id, string sortOrder, int? pageNumber, string currentFilter, string searchString)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            if (await ProjectFunctionAccessAsync(id))
            {
                var users = await _projectRoleRepository.GetAllNonMembersOfProjectAsync(id);
                var roleList = await _projectRoleRepository.GetAllProjectRolesAsync();
                List<SelectListItem> roleSelectList = new List<SelectListItem>();
                foreach (var role in roleList)
                {
                    roleSelectList.Add(new SelectListItem() { Text = role.Title, Value = role.Id.ToString() });
                }

                ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
                ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
                ViewData["UserNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

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
                    users = users.Where(s => s.UserName.ToUpper().Contains(searchString)).ToList();
                }
                ViewData["CurrentSort"] = sortOrder;
                switch (sortOrder)
                {
                    case "Date":
                        users = users.OrderBy(s => s.CreateDate).ToList();
                        break;
                    case "date_desc":
                        users = users.OrderByDescending(s => s.CreateDate).ToList();
                        break;
                    case "Email":
                        users = users.OrderBy(s => s.Email).ToList();
                        break;
                    case "email_desc":
                        users = users.OrderByDescending(s => s.Email).ToList();
                        break;
                    case "name_desc":
                        users = users.OrderByDescending(s => s.UserName).ToList();
                        break;
                    default:
                        users = users.OrderBy(s => s.UserName).ToList();
                        break;
                }
                int pageSize = 20;

                return View(new ProjectAddMemberViewModel() { ProjectId = id, NonMemberList = await PaginatedList<User>.CreateFromListAsync(users, pageNumber ?? 1, pageSize), ProjectRoleList = roleSelectList });
            }
            return RedirectToAction(nameof(NoProjectAccess));
        }

        // POST: Projects/AddMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember([FromForm] ProjectAddMemberViewModel model)
        {
            var roles = await _projectRoleRepository.GetAllProjectRolesAsync();
            var role = roles.FirstOrDefault(r => r.Id ==(ProjectRoleId) model.ProjectRoleId);
            var user = await _userRepository.GetUserByIdAsync(model.UserId);

            if (role == null || await _projectRoleRepository.IsUserInProjectAsync(model.UserId, model.ProjectId))
            {
                return NotFound();
            }
            if (user.IsDeleted)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var project = await _projectRepository.GetProjectByIdAsync(model.ProjectId);
                if (await ProjectFunctionAccessAsync(project.Id))
                {
                    await _projectRoleRepository.AddProjectMemberEntryAsync(user, (ProjectRoleId)model.ProjectRoleId, project);

                    var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    await _dashboardRepository.CreateNewsEntryAsync(user, NewsEntryCaseId.AddedToProject, project, await _userRepository.GetUserByIdAsync(currentUserId));
                }
                else
                {
                    return RedirectToAction(nameof(NoProjectAccess));
                }
            }
            return RedirectToAction(nameof(Members), new RouteValueDictionary(new { controller = "Project", action = "Members", Id = model.ProjectId }));

        }

        // GET: Projects/EditMember/5
        public async Task<IActionResult> EditMember(int projectId, string userId)
        {
            if (await _projectRoleRepository.IsUserInProjectAsync(userId, projectId) == false)
            {
                return NotFound();
            }
            if (await ProjectFunctionAccessAsync(projectId))
            {
                var userToEdit = await _userRepository.GetUserByIdAsync(userId);
                var memberProjectRole = await _projectRoleRepository.GetProjectRoleOfUserAsync(userToEdit.Id, projectId);
                if (memberProjectRole.Id == ProjectRoleId.Member || User.IsInRole("Admin") || (User.IsInRole("Projektmanager") && await _aspNetRoleRepository.GetRoleOfUserByIdAsync(userToEdit) == "Benutzer") || userToEdit.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    var roleList = await _projectRoleRepository.GetAllProjectRolesAsync();
                    List<SelectListItem> roleSelectList = new List<SelectListItem>();
                    foreach (var role in roleList)
                    {
                        roleSelectList.Add(new SelectListItem() { Text = role.Title, Value = role.Id.ToString() });
                    }
                    ProjectEditMemberViewModel model = new ProjectEditMemberViewModel() { ProjectId = projectId, UserName = userToEdit.UserName, UserId = userId, ProjectRole = memberProjectRole, ProjectRoleId = (int) memberProjectRole.Id, ProjectRoleList = roleSelectList };
                    return View(model);
                }
            }
            return RedirectToAction(nameof(NoProjectAccess));
        }

        // POST: Projects/EditMember/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMember([FromForm] ProjectMembersViewModel model)
        {
            var roles = await _projectRoleRepository.GetAllProjectRolesAsync();
            var role = roles.FirstOrDefault(r => r.Id == (ProjectRoleId) model.ProjectRoleId);
            if (role == null || await _projectRoleRepository.IsUserInProjectAsync(model.UserId, model.ProjectId) == false)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (await ProjectFunctionAccessAsync(model.ProjectId))
                {
                    User userToEdit = await _userRepository.GetUserByIdAsync(model.UserId);
                    var MemberProjectRole = await _projectRoleRepository.GetProjectRoleOfUserAsync(model.UserId, model.ProjectId);
                    if (MemberProjectRole.Id == ProjectRoleId.Member || User.IsInRole("Admin") || (User.IsInRole("Projektmanager") && await _aspNetRoleRepository.GetRoleOfUserByIdAsync(userToEdit) == "Benutzer") || userToEdit.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                    {
                        await _projectRoleRepository.UpdateProjectRoleAsync(model.ProjectId, model.UserId,(ProjectRoleId) model.ProjectRoleId);
                        return RedirectToAction(nameof(Members), new RouteValueDictionary(new { controller = "Project", action = "Members", Id = model.ProjectId }));
                    }
                }
                return RedirectToAction(nameof(NoProjectAccess));
            }
            return RedirectToAction(nameof(EditMember), new RouteValueDictionary(new { controller = "Project", action = "EditMember", projectId = model.ProjectId, userId = model.UserId }));
        }
    }
}

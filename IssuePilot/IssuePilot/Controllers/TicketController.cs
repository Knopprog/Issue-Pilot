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
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IssuePilot.Controllers
{
    [Authorize(Roles = "Admin,Projektmanager,Benutzer")]
    public class TicketController : ProjectAccessController
    {
        private readonly IProjectRoleRepository _projectRoleRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IUserRepository _userRepository;

        public TicketController(IProjectRoleRepository projectRoleRepository, IProjectRepository projectRepository, ITicketRepository ticketRepository, IDashboardRepository dashboardRepository, IUserRepository userRepository) : base(projectRoleRepository, projectRepository)
        {
            _projectRoleRepository = projectRoleRepository;
            _projectRepository = projectRepository;
            _ticketRepository = ticketRepository;
            _dashboardRepository = dashboardRepository;
            _userRepository = userRepository;
        }
        // GET: Ticket/Tickets/5
        public async Task<IActionResult> Tickets(int id, string sortOrder, int? pageNumber, string searchString, string currentFilter, bool open = false, bool inProgress = false, bool canceled = false, bool paused = false, bool closed = false)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(id))
            {
                List<Ticket> tickets = await _ticketRepository.GetTicketsOfProjectAsync(id);
                var categoriesOfProject = await _projectRepository.GetAllTicketCategoriesOfProjectAsync(id);
                var ticketProjectCategoriesOfProject = await _ticketRepository.GetTicketProjectCategoriesOfProjectAsync(categoriesOfProject);

                ViewData["DateAscSortParm"] = "date_asc";
                ViewData["DateDescSortParm"] = "";
                ViewData["TitleAscSortParm"] = "title";
                ViewData["TitleDescSortParm"] = "title_desc";
                ViewData["WeightAscSortParm"] = "weight";
                ViewData["WeightDescSortParm"] = "weight_desc";
                ViewData["DeadlineAscSortParm"] = "deadline";
                ViewData["DeadlineDescSortParm"] = "deadline_desc";

                ViewData["Open"] = open == false;
                ViewData["InProgress"] = inProgress == false;
                ViewData["Canceled"] = canceled == false;
                ViewData["Paused"] = paused == false;
                ViewData["Closed"] = closed == false;
                ViewData["OpenIsChecked"] = open;
                ViewData["InProgressIsChecked"] = inProgress;
                ViewData["CanceledIsChecked"] = canceled;
                ViewData["PausedIsChecked"] = paused;
                ViewData["ClosedIsChecked"] = closed;

                if (open || inProgress || canceled || paused || closed)
                {
                    if (!open)
                    {
                        tickets = tickets.Except(tickets.Where(r => r.Status.Id == TicketStatusId.Open)).ToList();
                    }
                    if (!inProgress)
                    {
                        tickets = tickets.Except(tickets.Where(r => r.Status.Id == TicketStatusId.InProgress)).ToList();
                    }
                    if (!canceled)
                    {
                        tickets = tickets.Except(tickets.Where(r => r.Status.Id == TicketStatusId.Canceled)).ToList();
                    }
                    if (!paused)
                    {
                        tickets = tickets.Except(tickets.Where(r => r.Status.Id == TicketStatusId.Paused)).ToList();
                    }
                    if (!closed)
                    {
                        tickets = tickets.Except(tickets.Where(r => r.Status.Id == TicketStatusId.Closed)).ToList();
                    }
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
                    List<Ticket> copy = new List<Ticket>();
                    copy.AddRange(tickets);
                    foreach (var ticket in copy)
                    {
                        string categories = "";
                        foreach (var category in ticket.TicketProjectCategories)
                        {
                            categories += category.TicketCategory.Name;
                        }

                        if (ticket.Description == null)
                        {
                            if (!ticket.Title.ToUpper().Contains(searchString) && !ticket.TicketCreator.UserName.ToUpper().Contains(searchString) && !ticket.Id.ToString().ToUpper().Contains(searchString) && !categories.ToUpper().Contains(searchString))
                            {
                                tickets.Remove(ticket);
                            }
                        }
                        else
                        {
                            if (!ticket.Title.ToUpper().Contains(searchString) && !ticket.TicketCreator.UserName.ToUpper().Contains(searchString) && !ticket.Description.ToUpper().Contains(searchString) && !ticket.Id.ToString().ToUpper().Contains(searchString) && !categories.ToUpper().Contains(searchString))
                            {
                                tickets.Remove(ticket);
                            }
                        }
                    }
                }

                ViewData["CurrentSort"] = sortOrder;

                switch (sortOrder)
                {
                    case "deadline":
                        tickets = tickets.OrderBy(s => s.Deadline).ToList();
                        break;
                    case "deadline_desc":
                        tickets = tickets.OrderByDescending(s => s.Deadline).ToList();
                        break;
                    case "weight":
                        tickets = tickets.OrderBy(s => s.Weight).ToList();
                        break;
                    case "weight_desc":
                        tickets = tickets.OrderByDescending(s => s.Weight).ToList();
                        break;
                    case "title_desc":
                        tickets = tickets.OrderByDescending(s => s.Title).ToList();
                        break;
                    case "title":
                        tickets = tickets.OrderBy(s => s.Title).ToList();
                        break;
                    case "date_asc":
                        tickets = tickets.OrderBy(s => s.CreateDate).ToList();
                        break;
                    default:
                        tickets = tickets.OrderByDescending(s => s.CreateDate).ToList();
                        sortOrder = "";
                        break;
                }


                int pageSize = 20;
                TicketTicketsViewModel model = new TicketTicketsViewModel()
                {
                    Tickets = await PaginatedList<Ticket>.CreateFromListAsync(tickets, pageNumber ?? 1, pageSize),
                    Project = project,
                    TicketCategoriesOfProject = categoriesOfProject,
                    TicketProjectCategoriesOfProject = ticketProjectCategoriesOfProject,
                    CurrentSortOrder = sortOrder
                };
                return View(model);
            }
            return RedirectToAction(nameof(NoProjectAccess));
        }

        // GET: Ticket/Details/5
        public async Task<IActionResult> Details(int id, int? pageNumber)
        {
            Ticket ticket = await _ticketRepository.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(ticket.Project.Id))
            {
                var model = await GetTicketDetailsById(ticket, pageNumber);
                return View(model);
            }
            return RedirectToAction(nameof(NoProjectAccess));
        }

        // POST: Ticket/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([FromForm] TicketDetailsViewModel model)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(model.Id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                switch (model.PostCommand)
                {
                    case 1: // ticketStatus
                        if (await ProjectAccessAsync(ticket.ProjectId))
                        {
                            if ((int) model.SelectedTicketStatusId >= 0 &&
                               (int) model.SelectedTicketStatusId <= 4)
                            {
                                await _ticketRepository.UpdateTicketStatusAsync( model.SelectedTicketStatusId, currentUserId, ticket.Id);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                        else
                        {
                            return RedirectToAction(nameof(NoProjectAccess));
                        }
                        break;
                    case 2: // become ticketAssignee
                        if (await _projectRoleRepository.IsUserInProjectAsync(currentUserId, ticket.ProjectId))
                        {
                            var currentUser = await _userRepository.GetUserByIdAsync(currentUserId);
                            await _ticketRepository.AddSelfAsAssigneeAsync(model.Id, currentUser);
                        }
                        else
                        {
                            if (User.IsInRole("Admin"))
                            {
                                var currentUser = await _userRepository.GetUserByIdAsync(currentUserId);
                                var project = await _projectRepository.GetProjectByIdAsync(ticket.ProjectId);
                                await _projectRoleRepository.AddProjectMemberEntryAsync(currentUser, ProjectRoleId.Owner, project);
                                await _dashboardRepository.CreateNewsEntryAsync(currentUser, NewsEntryCaseId.AddedToProject, project, currentUser);
                                await _ticketRepository.AddSelfAsAssigneeAsync(model.Id, currentUser);
                            }
                            else
                            {
                                return RedirectToAction(nameof(NoProjectAccess));
                            }
                        }
                        break;
                    case 3: // set ticketAssignees
                        if(model.SelectedAssigneesIds == null)
                        {
                            model.SelectedAssigneesIds = new List<string>();
                        }
                        if (((await ProjectAccessAsync(ticket.ProjectId) && ticket.TicketCreator.Id == currentUserId) || await ProjectFunctionAccessAsync(ticket.ProjectId)) && model.SelectedAssigneesIds.Count <= ticket.Project.ProjectMemberEntries.Count)
                        {
                            await _ticketRepository.UpdateAssigneesAsync(model.SelectedAssigneesIds, ticket.Id, currentUserId);
                        }
                        else
                        {
                            return RedirectToAction(nameof(NoProjectAccess));
                        }
                        break;
                    case 4: // add comment
                        if (model.CommentInputText != null)
                        {
                            if (await ProjectAccessAsync(ticket.ProjectId))
                            {
                                await _ticketRepository.AddCommentAsync(model.CommentInputText, currentUserId, model.Id);
                            }
                            else
                            {
                                return RedirectToAction(nameof(NoProjectAccess));
                            }
                        }
                        break;
                    case 5: // delete comment
                        var comment = ticket.Comments.FirstOrDefault(c => c.Id == model.SelectedCommentId);
                        if (comment != null)
                        {
                            if (comment.Creator.Id == currentUserId || await ProjectFunctionAccessAsync(ticket.ProjectId) || ticket.TicketCreator.Id == comment.Creator.Id)
                            {
                                var currentUser = await _userRepository.GetUserByIdAsync(currentUserId);
                                if (!await _ticketRepository.DeleteCommentAsync(model.SelectedCommentId, currentUser))
                                {
                                    return NotFound();
                                }
                            }
                            else
                            {
                                return RedirectToAction(nameof(NoProjectAccess));
                            }
                        }
                        else
                        {
                            return NotFound();
                        }
                        break;
                    case 6: //stop being TicketAssignee
                        if (await ProjectAccessAsync(ticket.ProjectId))
                        {
                            await _ticketRepository.RemoveSelfFromAssigneesAsync(ticket.Id, currentUserId);
                        }
                        else
                        {
                            return RedirectToAction(nameof(NoProjectAccess));
                        }
                        break;
                }
                return RedirectToAction(nameof(Details), new RouteValueDictionary(new { controller = "Ticket", action = "Details", ticket.Id }));
            }
            return RedirectToAction(nameof(Details), new RouteValueDictionary(new { controller = "Ticket", action = "Details", model.Id }));
        }

        private async Task<TicketDetailsViewModel> GetTicketDetailsById(Ticket ticket, int? pageNumber)
        {
            var categoriesOfProject = await _projectRepository.GetAllTicketCategoriesOfProjectAsync(ticket.Project.Id);
            List<string> ticketCategories;
            (ticketCategories, _) = await _ticketRepository.GetCategoriesOfTicketAsync(ticket.Id, categoriesOfProject);

            List<Comment> comments = await _ticketRepository.GetCommentsOfTicketAsync(ticket.Id);
            comments.Reverse();
            int pageSize = 10;

            var members = await _projectRoleRepository.GetMembersOfProjectByIdAsync(ticket.Project.Id);
            List<SelectListItem> memberList = new List<SelectListItem>();
            foreach (var member in members)
            {
                memberList.Add(new SelectListItem() { Text = member.UserName, Value = member.Id });
            }

            List<string> assignees;
            List<string> assigneesIds;
            (assignees, assigneesIds) = await _ticketRepository.GetAssigneesAsync(ticket.Id, members);

            List<TicketStatus> statuses = await _ticketRepository.GetTicketStatusesAsync();
            statuses[0].Name = "Offen";
            statuses[1].Name = "In Bearbeitung";
            statuses[2].Name = "Pausiert";
            statuses[3].Name = "Abgebrochen";
            statuses[4].Name = "Abgeschlossen";
            SelectList statusList = new SelectList(statuses, "Id", "Name",ticket.Status);
            string closedByUserName = await _ticketRepository.GetClosedByUserOfTicketAsync(ticket.Id);
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            TicketDetailsViewModel model = new TicketDetailsViewModel()
            {
                CurrentTicketStatus = ticket.Status.Id,
                Description = ticket.Description,
                Title = ticket.Title,
                Weight = ticket.Weight,
                Id = ticket.Id,
                ProjectId = ticket.Project.Id,
                ProjectTitle = ticket.Project.Title,
                CreateDate = ticket.CreateDate,
                CloseDate = ticket.CloseDate,
                Deadline = ticket.Deadline,
                CategoriesOfTicket = ticketCategories,
                CommentsOfTicket = await PaginatedList<Comment>.CreateFromListAsync(comments, pageNumber ?? 1, pageSize),
                SelectedAssigneesIds = assigneesIds,
                SelectedAssignees = assignees,
                StatusList = statusList,
                MemberList = memberList,
                ClosedByUser = closedByUserName,
                CreatedByUser = ticket.TicketCreator.UserName,
                CurrentUserId = currentUserId,
                SelectedTicketStatusId = ticket.Status.Id
            };
            if (await ProjectFunctionAccessAsync(ticket.Project.Id))
            {
                model.IsProjectOwner = true;
            }
            if (currentUserId == ticket.TicketCreator.Id)
            {
                model.IsCreator = true;
            }
            if (assigneesIds.FirstOrDefault(a => a == currentUserId) != null)
            {
                model.IsUserAssigned = true;
            }
            var images = ticket.Images;
            if (images != null)
            {
                List<string> imageDataURLList = new List<string>();
                foreach (var image in images)
                {
                    string imageBase64Data = Convert.ToBase64String(image.ImageData);
                    string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                    imageDataURLList.Add(imageDataURL);
                }
                model.ImageDataURLList = imageDataURLList;
            }
            return model;
        }

        // GET: Ticket/History/5
        public async Task<IActionResult> History(int id, int? pageNumber)
        {
            Ticket ticket = await _ticketRepository.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(ticket.Project.Id))
            {
                List<TicketHistoryEntry> ticketHistoryEntriesOfTicket = ticket.TicketHistoryEntries.ToList();
                ticketHistoryEntriesOfTicket.Reverse();
                int pageSize = 15;
                string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                TicketHistoryViewModel model = new TicketHistoryViewModel()
                {
                    TicketHistoryEntriesOfTicket = await PaginatedList<TicketHistoryEntry>.CreateFromListAsync(ticketHistoryEntriesOfTicket, pageNumber ?? 1, pageSize),
                    Id = id,
                    Title = ticket.Title,
                    CreateDate = ticket.CreateDate,
                    CreatorName = ticket.TicketCreator.UserName,
                    ProjectId = ticket.ProjectId,
                    ProjectTitle = ticket.Project.Title,
                    Weight = ticket.Weight,
                };
                if(await ProjectFunctionAccessAsync(ticket.ProjectId)){
                    model.IsProjectOwner = true;
                }
                else
                {
                    model.IsProjectOwner = false;
                }
                if(ticket.TicketCreator.Id == currentUserId)
                {
                    model.IsCreator = true;
                }
                else
                {
                    model.IsCreator = false;
                }
                return View(model);
            }
            return RedirectToAction(nameof(NoProjectAccess));
        }

        // GET: Ticket/SelectProject
        public async Task<IActionResult> SelectProject(string sortOrder, int? pageNumber, string currentFilter, string searchString)
        {
            return View(await GetProjectsAsync(sortOrder, pageNumber, currentFilter, searchString));
        }

        // GET: Ticket/Create/5
        public async Task<IActionResult> Create(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(id))
            {
                TicketCreateViewModel model = new TicketCreateViewModel() { ProjectId = id, ProjectTitle = project.Title };
                model = await GetCategoriesAndMembersOfProject(model);

                return View(model);

            }
            return RedirectToAction(nameof(NoProjectAccess));
        }

        // POST: Ticket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] TicketCreateViewModel model)
        {
            var project = await _projectRepository.GetProjectByIdAsync(model.ProjectId);
            if (project == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(project.Id))
            {
                if (ModelState.IsValid)
                {
                    var user = await _userRepository.GetUserByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    List<Image> imgList = CreateImageList();
                    var ticket = await _ticketRepository.CreateTicketAsync(model, project, user, imgList);
                    await _dashboardRepository.CreateNewsEntryForAllMembersAsync(project, ticket);
                    return RedirectToAction(nameof(Tickets), new RouteValueDictionary(new { controller = "Ticket", action = "Tickets", Id = model.ProjectId }));

                }
                model = await GetCategoriesAndMembersOfProject(model);
                return View(model);
            }
            return RedirectToAction(nameof(NoProjectAccess));

        }
        private async Task<TicketCreateViewModel> GetCategoriesAndMembersOfProject(TicketCreateViewModel model)
        {
            var members = await _projectRoleRepository.GetMembersOfProjectByIdAsync(model.ProjectId);
            List<SelectListItem> memberList = new List<SelectListItem>();
            foreach (var member in members)
            {
                memberList.Add(new SelectListItem() { Text = member.UserName, Value = member.Id });
            }
            List<TicketCategory> categoriesOfProject = await _projectRepository.GetAllTicketCategoriesOfProjectAsync(model.ProjectId);
            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (var categoryOfProject in categoriesOfProject)
            {
                categoryList.Add(new SelectListItem() { Text = categoryOfProject.Name, Value = categoryOfProject.Id.ToString() });
            }
            model.Members = memberList;
            model.CategoriesOfProject = categoryList;

            return model;
        }

        // GET: Ticket/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Ticket ticket = await _ticketRepository.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            if ((ticket.TicketCreator.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value && await ProjectAccessAsync(ticket.ProjectId)) || await ProjectFunctionAccessAsync(ticket.ProjectId))
            {
                TicketEditViewModel model = new TicketEditViewModel()
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    Weight = ticket.Weight,
                    Deadline = ticket.Deadline,
                };
                if (ticket.Deadline != null)
                {
                    model.NonNullableDeadline = (DateTime)ticket.Deadline;
                }
                
                model = await GetTicketEditViewModelData(ticket, model);
                model.SelectedTicketCategories = model.OldTicketCategories;
                return View(model);
            }
            return RedirectToAction(nameof(NoProjectAccess));
        }

        // POST: Ticket/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] TicketEditViewModel model)
        {
            Ticket ticket = await _ticketRepository.GetTicketByIdAsync(model.Id);
            if (ticket == null)
            {
                return NotFound();
            }

            if ((ticket.TicketCreator.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value && await ProjectAccessAsync(ticket.ProjectId)) || await ProjectFunctionAccessAsync(ticket.ProjectId))
            {
                if (ModelState.IsValid)
                {
                    List<Image> imgList = CreateImageList();
                    if (model.ImageList != null)
                    {
                        if (imgList.Count + model.ImageList.Count < 11)
                        {
                            await _ticketRepository.UpdateTicketAsync(model, imgList);
                        }
                    }
                    else
                    {
                        await _ticketRepository.UpdateTicketAsync(model, imgList);
                    }
                    return RedirectToAction(nameof(Details), new RouteValueDictionary(new { controller = "Ticket", action = "Details", model.Id }));
                }
                model = await GetTicketEditViewModelData(ticket, model);
                model.OldTicketCategories = model.SelectedTicketCategories;
                model.NonNullableDeadline = (DateTime)model.Deadline;
                return View(model);
            }
            return RedirectToAction(nameof(NoProjectAccess));

        }
        private async Task<TicketEditViewModel> GetTicketEditViewModelData(Ticket ticket, TicketEditViewModel ticketEditViewModel)
        {
            List<TicketCategory> categoriesOfProject = await _projectRepository.GetAllTicketCategoriesOfProjectAsync(ticket.Project.Id);
            List<SelectListItem> categoriesOfProjectList = new List<SelectListItem>();
            List<int> categoriesOfTicketIds;
            (_, categoriesOfTicketIds) = await _ticketRepository.GetCategoriesOfTicketAsync(ticket.Id, categoriesOfProject);
            foreach (var categoryOfProject in categoriesOfProject)
            {
                categoriesOfProjectList.Add(new SelectListItem() { Text = categoryOfProject.Name, Value = categoryOfProject.Id.ToString() });
            }

            var images = ticket.Images;
            List<SelectableImage> imageList = new List<SelectableImage>();

            foreach (var image in images)
            {
                string imageBase64Data = Convert.ToBase64String(image.ImageData);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                imageList.Add(new SelectableImage() { ImageData = imageDataURL, ImageId = image.Id, IsSelected = false });
            }
            ticketEditViewModel.OldTicketCategories = categoriesOfTicketIds;
            ticketEditViewModel.CategoriesOfProject = categoriesOfProjectList;
            ticketEditViewModel.ImageList = imageList;
            return ticketEditViewModel;
        }
        private List<Image> CreateImageList()
        {
            List<Image> imgList = new List<Image>();
            if (Request.Form.Files.Count < 11)
            {
                foreach (var file in Request.Form.Files)
                {
                    if (file.Length < 4194304)
                    {
                        Image img = new Image();
                        MemoryStream ms = new MemoryStream();
                        file.CopyTo(ms);
                        img.ImageData = ms.ToArray();

                        ms.Close();
                        ms.Dispose();

                        imgList.Add(img);
                    }
                }
            }
            return imgList;
        }
        // POST: Ticket/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            if ((ticket.TicketCreator.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value && await ProjectAccessAsync(ticket.ProjectId)) || await ProjectFunctionAccessAsync(ticket.ProjectId))
            {
                await _ticketRepository.DeleteTicketAsync(id);
            }
            else
            {
                return RedirectToAction(nameof(NoProjectAccess));
            }
            var projectId = ticket.Project.Id;
            return RedirectToAction(nameof(Tickets), new RouteValueDictionary(new { controller = "Ticket", action = "Tickets", Id = projectId }));
        }
    }
}

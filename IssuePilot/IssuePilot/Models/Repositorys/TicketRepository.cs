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
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDashboardRepository _dashboardRepository;
        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public TicketRepository(ApplicationDbContext context, IDashboardRepository dashboardRepository)
        {
            _context = context;
            _dashboardRepository = dashboardRepository;
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets.FindAsync(ticketId);
        }
        public async Task<List<TicketProjectCategory>> GetTicketProjectCategoriesOfProjectAsync(List<TicketCategory> categoriesOfProject)
        {
            List<TicketProjectCategory> ticketProjectCategoriesOfProject = new List<TicketProjectCategory>();
            await foreach (var ticketProjectCategory in _context.TicketProjectCategories)
            {
                foreach (var category in categoriesOfProject)
                {
                    if (category.Id == ticketProjectCategory.TicketCategoryId)
                    {
                        ticketProjectCategoriesOfProject.Add(ticketProjectCategory);
                        break;
                    }
                }
            }
            return ticketProjectCategoriesOfProject;
        }

        public async Task<List<Ticket>> GetTicketsOfProjectAsync(int projectId)
        {
            return await _context.Tickets.Where(r => r.Project.Id == projectId).ToListAsync();
        }

        public async Task<Ticket> CreateTicketAsync(TicketCreateViewModel model, Project project, User user, List<Image> imgList)
        {
            Ticket ticket = new Ticket
            {
                Title = model.Title,
                Description = model.Description,
                CreateDate = DateTime.Now,
                Project = project,
                Deadline = model.Deadline,
                Status = await _context.TicketStatuses.FirstOrDefaultAsync(r => r.Id == TicketStatusId.Open),
                TicketCreator = user,
                Weight = model.Weight,
                Images = imgList
            };
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            if (model.SelectedTicketCategories != null)
            {
                var ticketCategoriesOfProject = await _context.TicketCategories.Where(t => t.Project.Id == project.Id).ToListAsync();
                foreach (var selectedTicketCategory in model.SelectedTicketCategories)
                {
                    if (ticketCategoriesOfProject.FirstOrDefault(c => c.Id == selectedTicketCategory) != null)
                    {
                        TicketProjectCategory ticketProjectCategory = new TicketProjectCategory
                        {
                            TicketCategoryId = selectedTicketCategory,
                            TicketId = ticket.Id
                        };
                        await _context.TicketProjectCategories.AddAsync(ticketProjectCategory);
                    }

                }
            }
            if (model.SelectedAssignees != null)
            {
                await UpdateAssigneesAsync(model.SelectedAssignees, ticket.Id, user.Id);
            }
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task UpdateAssigneesAsync(List<string> selectedAssigneesIds, int ticketId, string currentUserId)
        {
            var listOfOldAssignees = await _context.TicketAssignees.Where(c => c.Ticket.Id == ticketId).ToListAsync();
            var entryCreator = await _context.Users.FirstOrDefaultAsync(r => r.Id == currentUserId);
            var ticket = await _context.Tickets.FirstOrDefaultAsync(r => r.Id == ticketId);
            var projectMembers = ticket.Project.ProjectMemberEntries;
            // add new assignees
            foreach (var assigneeId in selectedAssigneesIds)
            {
                if (listOfOldAssignees.FirstOrDefault(c => c.User.Id == assigneeId) == null && projectMembers.FirstOrDefault(c => c.FK_UserId == assigneeId) != null)
                {
                    TicketAssignee ticketAssignee = new TicketAssignee
                    {
                        FK_UserId = assigneeId,
                        FK_TicketId = ticketId
                    };
                    await _context.TicketAssignees.AddAsync(ticketAssignee);
                    await _context.SaveChangesAsync();
                    var referredUser = await _context.Users.FirstAsync(u => u.Id == assigneeId);
                    await CreateTicketHistoryEntryAsync(ticket, entryCreator, TicketHistoryEntryCaseId.MemberAssigned, ticketAssignee.User);
                    await _dashboardRepository.CreateNewsEntryAsync(ticketAssignee.User, NewsEntryCaseId.AssignedToTicket, ticket.Project, entryCreator, ticket);
                }
            }

            // remove old assignees
            foreach (var oldAssignee in listOfOldAssignees)
            {
                if (!selectedAssigneesIds.Contains(oldAssignee.User.Id))
                {
                    _context.TicketAssignees.Remove(await _context.TicketAssignees.FindAsync(ticketId, oldAssignee.User.Id));
                    await CreateTicketHistoryEntryAsync(ticket, entryCreator, TicketHistoryEntryCaseId.MemberUnassigned, oldAssignee.User);
                    await _dashboardRepository.CreateNewsEntryAsync(oldAssignee.User, NewsEntryCaseId.UnassignedFromTicket, ticket.Project, entryCreator, ticket);
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task AddSelfAsAssigneeAsync(int ticketId, User currentUser)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(r => r.Id == ticketId);
            var listOfOwnAssignments = await _context.TicketAssignees.Where(c => c.User.Id == currentUser.Id).ToListAsync();
            if (listOfOwnAssignments.FirstOrDefault(c => c.Ticket.Id == ticketId) == null)
            {
                TicketAssignee self = new TicketAssignee
                {
                    FK_UserId = currentUser.Id,
                    FK_TicketId = ticketId
                };
                await _context.TicketAssignees.AddAsync(self);
                await CreateTicketHistoryEntryAsync(ticket, currentUser, TicketHistoryEntryCaseId.MemberAssigned, currentUser);
                await _dashboardRepository.CreateNewsEntryAsync(currentUser, NewsEntryCaseId.AssignedToTicket, ticket.Project, currentUser, ticket);
                await _context.SaveChangesAsync();
            }
        }
        public async Task RemoveSelfFromAssigneesAsync(int ticketId, string currentUserId)
        {
            var entryCreator = await _context.Users.FirstOrDefaultAsync(r => r.Id == currentUserId);
            var ticket = await _context.Tickets.FirstOrDefaultAsync(r => r.Id == ticketId);
            var listOfOwnAssignments = await _context.TicketAssignees.Where(c => c.User.Id == currentUserId).ToListAsync();

            if (listOfOwnAssignments.FirstOrDefault(c => c.FK_TicketId == ticketId) != null)
            {
                _context.TicketAssignees.Remove(await _context.TicketAssignees.FindAsync(ticketId, currentUserId));
                await CreateTicketHistoryEntryAsync(ticket, entryCreator, TicketHistoryEntryCaseId.MemberUnassigned, entryCreator);
                await _dashboardRepository.CreateNewsEntryAsync(entryCreator, NewsEntryCaseId.UnassignedFromTicket, ticket.Project, entryCreator, ticket);
                await _context.SaveChangesAsync();
            }
        }
        public async Task CreateTicketHistoryEntryAsync(Ticket ticket, User entryCreator, TicketHistoryEntryCaseId entryCaseId, User referredUser = null)
        {
            TicketHistoryEntry newEntry = new TicketHistoryEntry
            {
                TicketHistoryEntryCaseId = entryCaseId
            };
            if (newEntry.TicketHistoryEntryCaseId == TicketHistoryEntryCaseId.MemberAssigned || newEntry.TicketHistoryEntryCaseId == TicketHistoryEntryCaseId.MemberUnassigned || newEntry.TicketHistoryEntryCaseId == TicketHistoryEntryCaseId.CommentDeleted)
            {
                newEntry.ReferredUser = referredUser;
            }
            newEntry.CreateDate = DateTime.Now;
            newEntry.EntryCreator = entryCreator;
            newEntry.Ticket = ticket;
            await _context.TicketHistoryEntries.AddAsync(newEntry);
            await _context.SaveChangesAsync();
        }
        public async Task<(List<string>, List<int>)> GetCategoriesOfTicketAsync(int ticketId, List<TicketCategory> categoriesOfProject)
        {
            var ticketProjectCategories = await GetTicketProjectCategoriesOfProjectAsync(categoriesOfProject);
            List<string> ticketCategoriesOfTicket = new List<string>();
            List<int> ticketCategoriesOfTicketIds = new List<int>();
            foreach (var ticketProjectCategory in ticketProjectCategories)
            {
                if (ticketProjectCategory.TicketId == ticketId)
                {
                    foreach (var ticketCategory in categoriesOfProject)
                    {
                        if (ticketCategory.Id == ticketProjectCategory.TicketCategoryId)
                        {
                            ticketCategoriesOfTicket.Add(ticketCategory.Name);
                            ticketCategoriesOfTicketIds.Add(ticketCategory.Id);
                            break;
                        }
                    }
                }
            }

            return (ticketCategoriesOfTicket, ticketCategoriesOfTicketIds);
        }
        public async Task<List<Comment>> GetCommentsOfTicketAsync(int id)
        {
            return await _context.Comments.Where(r => r.TicketId == id).ToListAsync();
        }
        public async Task<(List<string>, List<string>)> GetAssigneesAsync(int id, List<User> members)
        {
            var ticketAssigneesOfTicket = await _context.TicketAssignees.Where(r => r.FK_TicketId == id).ToListAsync();
            List<string> assigneesIds = new List<string>();
            List<string> assignees = new List<string>();
            foreach (var ticketAssignee in ticketAssigneesOfTicket)
            {
                if (!ticketAssignee.User.IsDeleted && members.Contains(ticketAssignee.User))
                {
                    assigneesIds.Add(ticketAssignee.FK_UserId);
                    assignees.Add(ticketAssignee.User.UserName);
                }
            }
            return (assignees, assigneesIds);
        }
        public async Task<List<TicketStatus>> GetTicketStatusesAsync()
        {
            return await _context.TicketStatuses.ToListAsync();
        }
        public async Task UpdateTicketStatusAsync(TicketStatusId selectedTicketStatus, string currentUserId, int ticketId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(r => r.Id == currentUserId);
            var ticket = await _context.Tickets.FirstOrDefaultAsync(r => r.Id == ticketId);
            var status = await _context.TicketStatuses.FirstOrDefaultAsync(r => r.Id == selectedTicketStatus);
            ticket.Status = status;
            if (selectedTicketStatus != TicketStatusId.Closed && ticket.ClosedByUser != null && ticket.CloseDate != null)
            {
                ticket.ClosedByUser.ClosedTickets.Remove(ticket);
                ticket.CloseDate = null;
            }
            else
            {
                ticket.CloseDate = DateTime.Now;
                ticket.ClosedByUser = user;
            }
            TicketHistoryEntryCaseId entryCaseId;
            switch (selectedTicketStatus)
            {
                default:
                    entryCaseId = TicketHistoryEntryCaseId.TicketOpened;
                    break;
                case TicketStatusId.InProgress:
                    entryCaseId = TicketHistoryEntryCaseId.TicketInProgress;
                    break;
                case TicketStatusId.Paused:
                    entryCaseId = TicketHistoryEntryCaseId.TicketPaused;
                    break;
                case TicketStatusId.Canceled:
                    entryCaseId = TicketHistoryEntryCaseId.TicketCanceled;
                    break;
                case TicketStatusId.Closed:
                    entryCaseId = TicketHistoryEntryCaseId.TicketClosed;
                    break;
            }
            await CreateTicketHistoryEntryAsync(ticket, user, entryCaseId);
            await _dashboardRepository.CreateNewsEntryForAssigneesAsync(ticket, NewsEntryCaseId.NewStatus, user);
            await _context.SaveChangesAsync();

        }
        public async Task DeleteTicketAsync(int id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(r => r.Id == id);
            ticket.Project.DeletedTicketsCount++;
            _context.TicketProjectCategories.RemoveRange(await _context.TicketProjectCategories.Where(t => t.Ticket.Id == id).ToListAsync());
            var entryList = await _context.NewsEntries.Where(n => n.Ticket.Id == id).ToListAsync();
            if (entryList != null)
            {
                _context.NewsEntries.RemoveRange(entryList);
            }

            _context.Tickets.Remove(await _context.Tickets.FindAsync(id));

            await _context.SaveChangesAsync();
        }
        public async Task UpdateTicketAsync(TicketEditViewModel model, List<Image> imagesToAddList)
        {
            var ticket = await _context.Tickets.FindAsync(model.Id);
            ticket.Title = model.Title;
            ticket.Description = model.Description;
            ticket.Deadline = model.Deadline;
            ticket.Weight = model.Weight;
            foreach (var image in imagesToAddList)
            {
                ticket.Images.Add(image);
            }
            if (model.ImageList != null)
            {
                foreach (var selectableImage in model.ImageList)
                {
                    if (selectableImage.IsSelected)
                    {
                        var image = await _context.Images.FirstOrDefaultAsync(i => i.Id == selectableImage.ImageId);
                        if (image.Id == selectableImage.ImageId)
                        {
                            _context.Images.Remove(image);
                        }
                    }
                }
            }
            if (model.OldTicketCategories == null)
            {
                model.OldTicketCategories = new List<int>();
            }
            if (model.SelectedTicketCategories == null)
            {
                model.SelectedTicketCategories = new List<int>();
            }
            var ticketCategoriesOfProject = await _context.TicketCategories.Where(t => t.Project.Id == ticket.Project.Id).ToListAsync();
            if(model.SelectedTicketCategories.Count < ticketCategoriesOfProject.Count +1 && model.OldTicketCategories.Count < ticketCategoriesOfProject.Count +1)
            {
                var tagsToRemove = model.OldTicketCategories.Except(model.SelectedTicketCategories).ToList();
                foreach (var tagToRemove in tagsToRemove)
                {
                    if (ticketCategoriesOfProject.FirstOrDefault(t => t.Id == tagToRemove) != null)
                    {
                        _context.TicketProjectCategories.Remove(await _context.TicketProjectCategories.FirstOrDefaultAsync(r => (r.TicketCategoryId == tagToRemove) && (r.TicketId == model.Id)));
                    }
                }
                var tagsToAdd = model.SelectedTicketCategories.Except(model.OldTicketCategories).ToList();
                foreach (var tagToAdd in tagsToAdd)
                {
                    if (ticketCategoriesOfProject.FirstOrDefault(t => t.Id == tagToAdd) != null)
                    {
                        TicketProjectCategory ticketProjectCategory = new TicketProjectCategory
                        {
                            TicketCategoryId = tagToAdd,
                            TicketId = model.Id
                        };
                        await _context.TicketProjectCategories.AddAsync(ticketProjectCategory);
                    }
                }
            }
            
            await _context.SaveChangesAsync();
        }
        public async Task AddCommentAsync(string commentInputText, string userId, int ticketId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(r => r.Id == userId);
            Comment comment = new Comment
            {
                Text = commentInputText,
                CreateDate = DateTime.Now,
                Creator = user
            };
            await _context.Comments.AddAsync(comment);
            var ticket = await _context.Tickets.FirstOrDefaultAsync(r => r.Id == ticketId);
            ticket.Comments.Add(comment);
            await CreateTicketHistoryEntryAsync(ticket, user, TicketHistoryEntryCaseId.CommentAdded);
            await _dashboardRepository.CreateNewsEntryForAssigneesAsync(ticket, NewsEntryCaseId.NewComment, comment.Creator, comment);
            await _context.SaveChangesAsync();
        }
        public async Task<string> GetClosedByUserOfTicketAsync(int ticketId)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(r => r.Id == ticketId);
            if (ticket.ClosedByUser == null)
            {
                return null;
            }
            else
            {
                return ticket.ClosedByUser.UserName;
            }
        }
        public async Task<bool> DeleteCommentAsync(int selectedCommentId, User entryCreator)
        {
            Comment comment = await _context.Comments.FirstOrDefaultAsync(r => r.Id == selectedCommentId);
            if (comment != null)
            {
                var ticket = comment.Ticket;
                _context.Comments.Remove(comment);
                await CreateTicketHistoryEntryAsync(ticket, entryCreator, TicketHistoryEntryCaseId.CommentDeleted, comment.Creator);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

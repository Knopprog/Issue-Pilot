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
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectRoleRepository _projectRoleRepository;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public DashboardRepository(ApplicationDbContext context, IProjectRoleRepository projectRoleRepository)
        {
            _projectRoleRepository = projectRoleRepository;
            _context = context;
        }
        //This method is used for StatusChange and NewComment
        public async Task CreateNewsEntryForAssigneesAsync(Ticket ticket, NewsEntryCaseId entryCaseId, User referredUser, Comment comment = null)
        {
            await CreateNewsEntryAsync(ticket.TicketCreator, entryCaseId, ticket.Project, referredUser, ticket, comment);

            foreach (var worker in ticket.TicketAssignees)
            {
                if (worker.User.Id != ticket.TicketCreator.Id)
                {
                    await CreateNewsEntryAsync(worker.User, entryCaseId, ticket.Project, referredUser, ticket, comment);
                }
            }
        }

        public async Task CreateNewsEntryAsync(User owner, NewsEntryCaseId entryCaseId, Project project, User referredUser, Ticket ticket = null, Comment comment = null)
        {
            NewsEntry newsEntry = new NewsEntry
            {
                CreateDate = DateTime.Now,
                NewsEntryCaseId = entryCaseId,
                Seen = false,
                Project = project
            };
            if(ticket != null)
            {
                newsEntry.Ticket = ticket;
            }
            switch (entryCaseId)
            {
                case NewsEntryCaseId.NewProject:
                    newsEntry.ActionId = 1;
                    newsEntry.ControllerId = 1;
                    newsEntry.RouteId = project.Id;
                    newsEntry.ReferredUser = referredUser;
                    break;
                case NewsEntryCaseId.NewComment:
                    newsEntry.ActionId = 2;
                    newsEntry.ControllerId = 1;
                    newsEntry.RouteId = ticket.Id;
                    newsEntry.Comment = comment;
                    newsEntry.ReferredUser = referredUser;
                    break;
                case NewsEntryCaseId.NewStatus:
                    newsEntry.ActionId = 2;
                    newsEntry.ControllerId = 1;
                    newsEntry.RouteId = ticket.Id;
                    newsEntry.Status = ticket.Status;
                    newsEntry.ReferredUser = referredUser;
                    break;
                case NewsEntryCaseId.AssignedToTicket:
                    newsEntry.ActionId = 2;
                    newsEntry.ControllerId = 1;
                    newsEntry.RouteId = ticket.Id;
                    newsEntry.Status = ticket.Status;
                    newsEntry.ReferredUser = referredUser;
                    break;
                case NewsEntryCaseId.RemovedFromProject:
                    newsEntry.ActionId = 3;
                    newsEntry.ControllerId = 2;
                    newsEntry.ReferredUser = referredUser;
                    break;
                case NewsEntryCaseId.AddedToProject:
                    newsEntry.ActionId = 4;
                    newsEntry.ControllerId = 2;
                    newsEntry.RouteId = project.Id;
                    newsEntry.ReferredUser = referredUser;
                    break;
                case NewsEntryCaseId.UnassignedFromTicket:
                    newsEntry.ActionId = 2;
                    newsEntry.ControllerId = 1;
                    newsEntry.RouteId = ticket.Id;
                    newsEntry.Status = ticket.Status;
                    newsEntry.ReferredUser = referredUser;
                    break;
                default:
                    break;
            }
            newsEntry.Owner = owner ?? throw new ArgumentNullException();

            await _context.NewsEntries.AddAsync(newsEntry);
            await _context.SaveChangesAsync();
        }

        public async Task CreateNewsEntryForAllMembersAsync(Project project, Ticket ticket)
        {
            var members = await _projectRoleRepository.GetMembersOfProjectByIdAsync(project.Id);
            foreach (var member in members)
            {
                NewsEntry newsEntry = new NewsEntry
                {
                    CreateDate = DateTime.Now,
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    Seen = false,
                    ActionId = 2,
                    ControllerId = 1,
                    RouteId = ticket.Id,
                    Ticket = ticket,
                    Project = project,
                    Status = ticket.Status,
                    ReferredUser = ticket.TicketCreator
                };
                newsEntry.Owner = member;
                await _context.NewsEntries.AddAsync(newsEntry);
                var dbticket = await _context.Tickets.FirstOrDefaultAsync(r => r.Id == ticket.Id);
                dbticket.NewsEntries.Add(newsEntry);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllEntriesOfUserAsync(string userId)
        {
            var listOfEntries = await _context.NewsEntries.Where(n => n.Owner.Id == userId).ToListAsync();
            _context.NewsEntries.RemoveRange(listOfEntries);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DashboardIndexViewModel>> GetNewsByUserIdAsync(string userId)
        {
            var dbListOfNewsEntries = await _context.NewsEntries.Where(r => r.Owner.Id == userId).OrderByDescending(x => x.CreateDate).ToListAsync();
            List<DashboardIndexViewModel> listModel = new List<DashboardIndexViewModel>();
            foreach (var newsEntry in dbListOfNewsEntries)
            {
                string newsText;
                if(newsEntry.NewsEntryCaseId == NewsEntryCaseId.NewProject)
                {
                    newsText = "Das Projekt „" + newsEntry.Project.Title + "“ wurde von Ihnen erstellt.";
                }
                else
                {
                    if (newsEntry.ReferredUser == newsEntry.Owner)
                    {
                        newsText = newsEntry.NewsEntryCaseId switch
                        {
                            NewsEntryCaseId.NewTicket => "Sie haben das Ticket „" + newsEntry.Ticket.Title + "“ im Projekt „" + newsEntry.Project.Title + "“ erstellt.",
                            NewsEntryCaseId.NewComment => "Sie haben das Ticket „" + newsEntry.Ticket.Title + "“ im Projekt „" + newsEntry.Project.Title + "“ kommentiert.",
                            NewsEntryCaseId.NewStatus => "Sie haben den Status des Tickets „" + newsEntry.Ticket.Title + "“ im Projekt „" + newsEntry.Project.Title + "“ zu „" + newsEntry.Status.Id switch
                            {
                                TicketStatusId.Open => "Offen",
                                TicketStatusId.InProgress => "In Bearbeitung",
                                TicketStatusId.Paused => "Pausiert",
                                TicketStatusId.Canceled => "Abgebrochen",
                                TicketStatusId.Closed => "Geschlossen",
                                _ => "",
                            } + "“ geändert.",
                            NewsEntryCaseId.AssignedToTicket => "Sie haben das Ticket „" + newsEntry.Ticket.Title + "“ des Projekts „" + newsEntry.Ticket.Project.Title + "“ übernommen.",
                            NewsEntryCaseId.UnassignedFromTicket => "Sie haben sich vom Ticket „" + newsEntry.Ticket.Title + "“ des Projekts „" + newsEntry.Ticket.Project.Title + "“ entfernt.",
                            NewsEntryCaseId.AddedToProject => "Sie haben sich dem Projekt „" + newsEntry.Project.Title + "“ hinzugefügt.",
                            NewsEntryCaseId.RemovedFromProject => "Sie haben das Projekt „" + newsEntry.Project.Title + "“ verlassen.",
                            _ => "",
                        };
                    }
                    else
                    {
                        newsText = newsEntry.NewsEntryCaseId switch
                        {
                            NewsEntryCaseId.NewTicket => newsEntry.ReferredUser.UserName + " hat das Ticket „" + newsEntry.Ticket.Title + "“ im Projekt „" + newsEntry.Project.Title + "“ erstellt.",
                            NewsEntryCaseId.NewComment => "Das Ticket „" + newsEntry.Ticket.Title + "“ im Projekt „" + newsEntry.Project.Title + "“ wurde von " + newsEntry.Comment.Creator.UserName + " kommentiert.",
                            NewsEntryCaseId.NewStatus => "Der Status des Tickets „" + newsEntry.Ticket.Title + "“ im Projekt „" + newsEntry.Project.Title + "“ wurde von " + newsEntry.ReferredUser.UserName + " zu „" + newsEntry.Status.Id switch
                            {
                                TicketStatusId.Open => "Offen",
                                TicketStatusId.InProgress => "In Bearbeitung",
                                TicketStatusId.Paused => "Pausiert",
                                TicketStatusId.Canceled => "Abgebrochen",
                                TicketStatusId.Closed => "Geschlossen",
                                _ => "",
                            } + "“ geändert.",
                            NewsEntryCaseId.AssignedToTicket => newsEntry.ReferredUser.UserName + " hat Ihnen das Ticket „" + newsEntry.Ticket.Title + "“ des Projekts „" + newsEntry.Ticket.Project.Title + "“ zugewiesen.",
                            NewsEntryCaseId.UnassignedFromTicket => newsEntry.ReferredUser.UserName + " hat Sie vom Ticket „" + newsEntry.Ticket.Title + "“ des Projekts „" + newsEntry.Ticket.Project.Title + "“ entfernt.",
                            NewsEntryCaseId.AddedToProject => newsEntry.ReferredUser.UserName + " hat Sie dem Projekt „" + newsEntry.Project.Title + "“ hinzugefügt.",
                            NewsEntryCaseId.RemovedFromProject => newsEntry.ReferredUser.UserName + " hat Sie aus dem Projekt „" + newsEntry.Project.Title + "“ entfernt.",
                            _ => "",
                        };
                    }
                }
                
                string action = newsEntry.ActionId switch
                {
                    1 => "Tickets",
                    2 => "Details",
                    3 => "Index",
                    4 => "Members",
                    _ => "",
                };
                string controller = newsEntry.ControllerId switch
                {
                    1 => "Ticket",
                    2 => "Project",
                    _ => "",
                };
                DashboardIndexViewModel model = new DashboardIndexViewModel
                {
                    CreateDate = newsEntry.CreateDate,
                    Id = newsEntry.Id,
                    EntryCaseId = newsEntry.NewsEntryCaseId,
                    NewsText = newsText,
                    Seen = newsEntry.Seen,
                    Action = action,
                    RouteId = newsEntry.RouteId,
                    Controller = controller,
                };
                if (newsEntry.Status != null)
                {
                    model.StatusId = newsEntry.Status.Id;
                }
                listModel.Add(model);
            }
            return listModel;
        }
        public async Task UpdateSeenStatusesAsync(List<DashboardIndexViewModel> listOfNews)
        {
            for (int i = 0; i < listOfNews.Count; i++)
            {
                var entry = await _context.NewsEntries.FindAsync(listOfNews[i].Id);
                entry.Seen = true;
            }
            await _context.SaveChangesAsync();
        }
    }
}

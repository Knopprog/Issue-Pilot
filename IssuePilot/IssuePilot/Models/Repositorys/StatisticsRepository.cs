using IssuePilot.Data;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels.Statistics;
using IssuePilot.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace IssuePilot.Models.Repositorys
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectRoleRepository _projectRoleRepository;
        private readonly IProjectRepository _projectRepository;
        public StatisticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public StatisticsRepository(ApplicationDbContext context, IProjectRoleRepository projectRoleRepository, IProjectRepository projectRepository)
        {
            _context = context;
            _projectRoleRepository = projectRoleRepository;
            _projectRepository = projectRepository;
        }

        public async Task<StatisticsCalendarViewModel> GetCalendarDataAsync(int projectId)
        {
            StatisticsCalendarViewModel statisticsModel = new StatisticsCalendarViewModel
            {
                ListOfTicketCreatedDate = new List<CalendarEventViewModel>(),
                ListOfTicketClosedDate = new List<CalendarEventViewModel>(),
                ListOfTicketDeadlines = new List<CalendarEventViewModel>()
            };
            Project project = await _projectRepository.GetProjectByIdAsync(projectId);
            statisticsModel.ProjectTitle = project.Title;
            statisticsModel.Id = project.Id;
            var listCreateDates = _context.Tickets.Where(c => c.Project.Id == projectId).ToList().GroupBy(c => c.CreateDate.Date).Select(d => new CalendarEventViewModel() { Date = d.Key.Date, TicketsCount = d.Count() });
            foreach (var createDate in listCreateDates)
            {
                statisticsModel.ListOfTicketCreatedDate.Add(createDate);
            }

            var listClosedDates = _context.Tickets.Where(c => c.CloseDate != null && c.Project.Id == projectId).Select(c => new { Closedate = (DateTime)c.CloseDate }).ToList().GroupBy(c => c.Closedate.Date).Select(d => new CalendarEventViewModel() { Date = d.Key.Date, TicketsCount = d.Count() });
            foreach (var closedDate in listClosedDates)
            {
                statisticsModel.ListOfTicketClosedDate.Add(closedDate);
            }

            var listDeadlines = _context.Tickets.Where(c => c.Deadline != null && c.Project.Id == projectId).Select(c => new { Deadline = (DateTime)c.Deadline }).ToList().GroupBy(c => c.Deadline.Date).Select(d => new CalendarEventViewModel() { Date = d.Key.Date, TicketsCount = d.Count() });
            foreach (var deadline in listDeadlines)
            {
                statisticsModel.ListOfTicketDeadlines.Add(deadline);
            }

            return statisticsModel;
        }
        public async Task<StatisticsDetailsViewModel> GetProjectStatisticsDataAsync(int projectId)
        {
            StatisticsDetailsViewModel statisticsModel = new StatisticsDetailsViewModel
            {
                ListNumbersOfTicketStatus = new List<NumbersOfTicketStatus>(),
                ListNumberOfCreatedTicketsByUsers = new List<NumberOfCreatedTicketsByUser>(),
                ListNumberOfTicketsAssignedToUser = new List<NumberOfTicketsAssignedToUser>(),
                ListNumberOfTimesCategoryWasUsed = new List<NumberOfTimesCategoryWasUsed>(),
            };
            var listOfTicketStatus = _context.TicketStatuses.ToList();
            Project project = await _projectRepository.GetProjectByIdAsync(projectId);
            List<User> members = await _projectRoleRepository.GetMembersOfProjectByIdAsync(projectId);
            List<TicketCategory> listOfTicketCategories = await _projectRepository.GetAllTicketCategoriesOfProjectAsync(projectId);
            statisticsModel.ProjectTitle = project.Title;
            statisticsModel.Id = project.Id;

            int closedTicketsCount = 0;
            foreach(var member in members)
            {
                closedTicketsCount += member.ClosedTickets.Count;
            }
            statisticsModel.NumberOfTicketsCreated = _context.Tickets.Where(c => c.Project.Id == projectId).Count();


            foreach (var status in listOfTicketStatus)
            {
                NumbersOfTicketStatus numbersOfTicketStatus = new NumbersOfTicketStatus
                {
                    StatusName = status.Id switch
                    {
                        TicketStatusId.Open => "Offen",
                        TicketStatusId.InProgress => "In Bearbeitung",
                        TicketStatusId.Paused => "Pausiert",
                        TicketStatusId.Canceled => "Abgebrochen",
                        TicketStatusId.Closed => "Geschlossen",
                        _ => "",
                    },
                    NumberOfTicketsWithStatus = _context.Tickets.Where(c => c.Project.Id == projectId && c.Status.Id == status.Id).Count()
                };
                statisticsModel.ListNumbersOfTicketStatus.Add(numbersOfTicketStatus);
            }


            foreach (var member in members)
            {
                NumberOfCreatedTicketsByUser numberOfCreatedTicketsByUser = new NumberOfCreatedTicketsByUser
                {
                    UserName = member.UserName,
                    NumberOfTickets = _context.Tickets.Where(c => c.Project.Id == projectId && c.TicketCreator.Id == member.Id).Count()
                };
                statisticsModel.ListNumberOfCreatedTicketsByUsers.Add(numberOfCreatedTicketsByUser);
            }
            statisticsModel.ListNumberOfCreatedTicketsByUsers = statisticsModel.ListNumberOfCreatedTicketsByUsers.OrderByDescending(o => o.NumberOfTickets).ToList();


            foreach (var user in members)
            {
                NumberOfTicketsAssignedToUser numberOfTicketsAssignedToUser = new NumberOfTicketsAssignedToUser
                {
                    UserName = user.UserName,
                    NumberOfTickets = _context.TicketAssignees.Where(c => c.Ticket.Project.Id == projectId && c.User.Id == user.Id).Count()
                };
                statisticsModel.ListNumberOfTicketsAssignedToUser.Add(numberOfTicketsAssignedToUser);
            }
            statisticsModel.NumberOfDeletedTickets = project.DeletedTicketsCount;


           

            foreach (var category in listOfTicketCategories)
            {
                NumberOfTimesCategoryWasUsed numberOfTimesCategoryWasUsed = new NumberOfTimesCategoryWasUsed
                {
                    NameOfCategory = category.Name,
                    NumberOfCategoryUses = _context.TicketProjectCategories.Where(c => c.Ticket.Project.Id == projectId && c.TicketCategoryId == category.Id).Count()
                };
                statisticsModel.ListNumberOfTimesCategoryWasUsed.Add(numberOfTimesCategoryWasUsed);
            }
            statisticsModel.ListNumberOfTimesCategoryWasUsed = statisticsModel.ListNumberOfTimesCategoryWasUsed.OrderByDescending(o => o.NumberOfCategoryUses).ToList();


            statisticsModel.NumberOfMembers = _context.ProjectMemberEntries.Where(c => c.FK_ProjectId == projectId).Count();


            statisticsModel.NumberOfTicketsPastDeadline = _context.Tickets.Where(c => c.Project.Id == projectId && c.Status.Id != TicketStatusId.Closed && c.Deadline < DateTime.Now).Count();


            var listOfClosedTickets = _context.Tickets.Where(c => c.Project.Id == projectId && c.Status.Id == TicketStatusId.Closed && c.CreateDate != null && c.CloseDate != null).ToList();
            if (listOfClosedTickets != null && listOfClosedTickets.Count > 0)
            {
                var listOfTimeSpans = new List<double>();
                foreach (var ticket in listOfClosedTickets)
                {
                    listOfTimeSpans.Add((ticket.CloseDate - ticket.CreateDate).Value.TotalMilliseconds);
                }
                statisticsModel.AVGProcessingTimeOfTickets = TimeSpan.FromMilliseconds(listOfTimeSpans.Average());
            }

            return statisticsModel;
        }
    }
}

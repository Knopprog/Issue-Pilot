using IssuePilot.Models.ViewModels;
using IssuePilot.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface IDashboardRepository
    {
        Task<List<DashboardIndexViewModel>> GetNewsByUserIdAsync(string userId);
        Task CreateNewsEntryAsync(User owner, NewsEntryCaseId newsTextId, Project project, User referredUser, Ticket ticket = null, Comment comment = null);
        Task CreateNewsEntryForAllMembersAsync(Project project, Ticket ticket);
        Task CreateNewsEntryForAssigneesAsync(Ticket ticket, NewsEntryCaseId newsTextId, User referredUser, Comment comment = null);
        Task UpdateSeenStatusesAsync(List<DashboardIndexViewModel> listOfNews);
        Task DeleteAllEntriesOfUserAsync(string userId);
    }
}

using IssuePilot.Models.ViewModels;
using IssuePilot.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetTicketsOfProjectAsync(int projectId);
        Task<List<TicketProjectCategory>> GetTicketProjectCategoriesOfProjectAsync(List<TicketCategory> categoriesOfProject);
        Task<Ticket> CreateTicketAsync(TicketCreateViewModel model, Project project, User user, List<Image> imgList);
        Task<Ticket> GetTicketByIdAsync(int id);
        Task<(List<string>, List<int>)> GetCategoriesOfTicketAsync(int ticketId, List<TicketCategory> categoriesOfProject);
        Task<List<Comment>> GetCommentsOfTicketAsync(int id);
        Task<(List<string>, List<string>)> GetAssigneesAsync(int id, List<User> members);
        Task<List<TicketStatus>> GetTicketStatusesAsync();
        Task UpdateAssigneesAsync(List<string> selectedAssigneesIds, int ticketId, string currentUserId);
        Task AddSelfAsAssigneeAsync(int ticketId, User currentUser);
        Task UpdateTicketStatusAsync(TicketStatusId selectedTicketStatus, string currentUserId, int ticketId);
        Task DeleteTicketAsync(int id);
        Task UpdateTicketAsync(TicketEditViewModel model, List<Image> imgList);
        Task AddCommentAsync(string commentInputText, string userId, int ticketId);
        Task<string> GetClosedByUserOfTicketAsync(int id);
        Task<bool> DeleteCommentAsync(int selectedCommentId, User entryCreator);
        Task RemoveSelfFromAssigneesAsync(int id, string currentUserId);
    }
}

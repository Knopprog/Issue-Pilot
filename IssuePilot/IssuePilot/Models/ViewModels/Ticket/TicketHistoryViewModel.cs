using IssuePilot.Helper;
using IssuePilot.Models.DBModels;

namespace IssuePilot.Models.ViewModels
{
    public class TicketHistoryViewModel : TicketDataModel
    {
        public PaginatedList<TicketHistoryEntry> TicketHistoryEntriesOfTicket { get; set; }
        public string CreatorName { get; set; }
    }
}

using IssuePilot.Models.DBModels;
using IssuePilot.Helper;
using System.Collections.Generic;

namespace IssuePilot.Models.ViewModels
{
    public class TicketTicketsViewModel
    {
        public Project Project { get; set; }
        public PaginatedList<Ticket> Tickets { get; set; }
        public List<TicketProjectCategory> TicketProjectCategoriesOfProject { get; set; }
        public List<TicketCategory> TicketCategoriesOfProject { get; set; }
        public string CurrentSortOrder { get; set; }
    }
}

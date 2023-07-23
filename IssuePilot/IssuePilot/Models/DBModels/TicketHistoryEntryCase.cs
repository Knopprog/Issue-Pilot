using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.DBModels
{
    public class TicketHistoryEntryCase
    {
        [Key]
        public TicketHistoryEntryCaseId EntryCaseId { get; set; }
        public string Name { get; set; }
        public virtual List<TicketHistoryEntry> TicketHistoryEntries { get; set; } = new List<TicketHistoryEntry>();
    }
}

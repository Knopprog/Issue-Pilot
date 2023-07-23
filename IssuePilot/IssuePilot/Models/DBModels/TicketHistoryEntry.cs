using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models.DBModels
{
    public class TicketHistoryEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual User ReferredUser { get; set; }
        public virtual User EntryCreator { get; set; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        [DisplayFormat(DataFormatString = "{0:dddd, dd/MM/yyyy, H:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }
        public TicketHistoryEntryCaseId TicketHistoryEntryCaseId { get; set; }
        public virtual TicketHistoryEntryCase TicketHistoryEntryCase { get; set; }
    }
}

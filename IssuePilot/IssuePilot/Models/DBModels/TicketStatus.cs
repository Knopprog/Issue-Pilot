using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.DBModels
{
    public class TicketStatus
    {
        [Key]
        public TicketStatusId Id { set; get; }
        [Required]
        public string Name { set; get; }
        public virtual List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
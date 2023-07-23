using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models.DBModels
{
    public class TicketAssignee
    {
        [Key]
        [Column(Order = 1)]
        public int FK_TicketId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string FK_UserId { get; set; }

        [ForeignKey("FK_TicketId")]
        public virtual Ticket Ticket { get; set; }
        [ForeignKey("FK_UserId")]
        public virtual User User { get; set; }
    }
}

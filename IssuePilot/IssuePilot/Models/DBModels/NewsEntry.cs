using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models.DBModels
{
    public class NewsEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy - HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { set; get; }
        public NewsEntryCaseId NewsEntryCaseId { set; get; }
        [Required]
        public virtual NewsEntryCase NewsEntryCase { get; set; }
        public bool Seen { set; get; }
        public virtual User Owner { get; set; }
        public virtual User ReferredUser { get; set; }
        public int? TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public int? ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public int? CommentId { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual TicketStatus Status { get; set; }
        public int? RouteId { get; set; }
        public int ActionId { get; set; }
        public int ControllerId { get; set; }
    }
}

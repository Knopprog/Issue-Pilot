using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models.DBModels
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        [Required]
        [MaxLength(140)]
        public string Title { set; get; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy, HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Deadline { set; get; }
        [MaxLength(5000)]
        public string Description { get; set; }
        [Required]
        public virtual TicketStatus Status { get; set; }
        public TicketStatusId TicketStatusId { get; set; }
        [Required]
        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy, HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { set; get; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy, HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? CloseDate { set; get; }
        public int Weight { get; set; }
        public virtual User ClosedByUser { get; set; }
        public virtual User TicketCreator { get; set; }
        public virtual ICollection<Image> Images { get; set; } = new List<Image>();
        public virtual ICollection<TicketProjectCategory> TicketProjectCategories { get; set; } = new List<TicketProjectCategory>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<TicketHistoryEntry> TicketHistoryEntries { get; set; } = new List<TicketHistoryEntry>();
        public virtual ICollection<TicketAssignee> TicketAssignees { get; set; } = new List<TicketAssignee>();
        public virtual ICollection<NewsEntry> NewsEntries { get; set; } = new List<NewsEntry>();
    }
}

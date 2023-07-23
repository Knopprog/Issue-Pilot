using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models.DBModels
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        [Required]
        [MaxLength(140)]
        public string Title { set; get; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy, HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { set; get; }
        public int DeletedTicketsCount { set; get; }
        public virtual User Creator { get; set; }
        public virtual ICollection<ProjectMemberEntry> ProjectMemberEntries { get; set; } = new List<ProjectMemberEntry>();
        public virtual ICollection<TicketCategory> TicketCategories { get; set; } = new List<TicketCategory>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public virtual ICollection<NewsEntry> NewsEntries { get; set; } = new List<NewsEntry>();
    }
}
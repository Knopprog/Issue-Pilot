using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models.DBModels
{
    public class User : IdentityUser
    {
        [MaxLength(20)]
        public string Firstname { get; set; }
        [MaxLength(20)]        
        public string Surname { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy, HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { set; get; }
        public bool IsDeleted { set; get; }
        public virtual ICollection<ProjectMemberEntry> ProjectMemberEntries { get; set; } = new List<ProjectMemberEntry>();
        [InverseProperty("TicketCreator")]
        public virtual ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();
        [InverseProperty("ClosedByUser")]
        public virtual ICollection<Ticket> ClosedTickets { get; set; } = new List<Ticket>();
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual ICollection<TicketAssignee> TicketAssignees { get; set; } = new List<TicketAssignee>();
    }
}

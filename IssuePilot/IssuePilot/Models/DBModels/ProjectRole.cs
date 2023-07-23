using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models.DBModels
{
    public class ProjectRole
    {
        [Key]
        public ProjectRoleId Id { set; get; }
        [Required]
        public string Title { set; get; }
        public virtual List<ProjectMemberEntry> ProjectMemberEntries { get; set; } = new List<ProjectMemberEntry>();
    }
}
using IssuePilot.Helper;
using IssuePilot.Models.DBModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace IssuePilot.Models.ViewModels
{
    public class ProjectMembersViewModel
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string UserId { get; set; }
        public int ProjectRoleId { get; set; }
        public PaginatedList<ProjectMemberDataModel> Members { get; set; }
        public bool IsOwner { get; set; }
        public bool IsMember { get; set; }
    }
}

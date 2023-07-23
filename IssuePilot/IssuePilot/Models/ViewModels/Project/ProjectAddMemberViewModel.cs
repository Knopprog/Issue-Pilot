using IssuePilot.Helper;
using IssuePilot.Models.DBModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace IssuePilot.Models.ViewModels
{
    public class ProjectAddMemberViewModel
    {
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public int ProjectRoleId { get; set; }
        public PaginatedList<User> NonMemberList { get; set; }
        public List<SelectListItem> ProjectRoleList { get; set; }
    }
}

using IssuePilot.Models.DBModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace IssuePilot.Models.ViewModels
{
    public class ProjectEditMemberViewModel
    {
        public int ProjectId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public int ProjectRoleId { get; set; }
        public ProjectRole ProjectRole { get; set; }
        public List<SelectListItem> ProjectRoleList { get; set; }
    }
}

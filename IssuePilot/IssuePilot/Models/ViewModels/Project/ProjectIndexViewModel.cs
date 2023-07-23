using IssuePilot.Helper;
using IssuePilot.Models.DBModels;

namespace IssuePilot.Models.ViewModels
{
    public class ProjectIndexViewModel: ProjectUpdateViewModel
    {

        public PaginatedList<Project> Projects { get; set; }

    }
}

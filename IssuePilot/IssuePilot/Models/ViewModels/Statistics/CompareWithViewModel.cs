using IssuePilot.Models.DBModels;
using IssuePilot.Helper;

namespace IssuePilot.Models.ViewModels.Statistics
{
    public class CompareWithViewModel
    {
        public PaginatedList<Project> Projects { get; set; }
        public int FirstId { get; set; }
        public string FirstProjectTitle { get; set; }
        public int SecondId { get; set; }
    }
}

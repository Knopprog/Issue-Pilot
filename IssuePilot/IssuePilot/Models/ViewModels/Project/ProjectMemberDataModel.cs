using IssuePilot.Models.DBModels;

namespace IssuePilot.Models.ViewModels
{
    public class ProjectMemberDataModel
    {
        public User User { get; set; }
        public ProjectRole ProjectRole { get; set; }
        public string UserRole { get; set; }
    }
}

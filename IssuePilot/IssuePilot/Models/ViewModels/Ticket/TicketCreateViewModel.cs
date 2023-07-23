using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace IssuePilot.Models.ViewModels
{
    public class TicketCreateViewModel : TicketDataModel
    {
        public List<int> SelectedTicketCategories { get; set; }
        public List<string> SelectedAssignees { get; set; }
        public List<SelectListItem> Members { get; set; }
        public List<SelectListItem> CategoriesOfProject { get; set; }
    }
}

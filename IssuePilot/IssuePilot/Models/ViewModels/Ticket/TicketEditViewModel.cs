using IssuePilot.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace IssuePilot.Models.ViewModels
{
    public class TicketEditViewModel : TicketDataModel
    {
        public List<int> SelectedTicketCategories { get; set; }
        public List<int> OldTicketCategories { get; set; }
        public List<SelectableImage> ImageList { get; set; }
        public List<SelectListItem> CategoriesOfProject { get; set; }
        public DateTime NonNullableDeadline { get; set; }
    }
}

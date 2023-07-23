using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IssuePilot.Models.DBModels;

namespace IssuePilot.Models.ViewModels
{
    public class ProjectOtherViewModel : ProjectUpdateViewModel
    {
        public List<TicketCategory> TicketCategories { get; set; }
        public bool IsOwner { get; set; }
        public string CreatorName { get; set; }
        public int EditCategoryId { set; get; }
        [Required(ErrorMessage = "Eine Kategorie muss einen Namen haben!")]
        [StringLength(50, ErrorMessage = "Eine Kategorie darf nicht mehr als {2} Zeichen enthalten.")]
        public string EditCategoryName { set; get; }
        [Required(ErrorMessage = "Eine Kategorie muss einen Namen haben!")]
        [StringLength(50, ErrorMessage = "Eine Kategorie darf nicht mehr als {2} Zeichen enthalten.")]
        public string CreateCategoryName { set; get; }
        public string ActualTitle { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public class ProjectCategoryViewModel
    {
        public int CategoryId { set; get; }
        [Required(ErrorMessage = "Eine Kategorie muss einen Namen haben!")]
        [StringLength(50, ErrorMessage = "Eine Kategorie darf nicht mehr als {2} Zeichen enthalten.")]
        public string Name { set; get; }
        public int ProjectId { set; get; }
    }
}

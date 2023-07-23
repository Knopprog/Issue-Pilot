using System;
using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public abstract class ProjectDataModel
    {
        public int Id { set; get; }
        [Required(ErrorMessage = "Ein Projekt muss einen Titel haben!")]
        [DataType(DataType.Text)]
        [Display(Name = "Titel")]
        [StringLength(140, ErrorMessage = "Ein Titel darf nicht mehr als {2} Zeichen enthalten.")]
        public string Title { set; get; }
        [StringLength(1000, ErrorMessage = "Eine Beschreibung darf nicht mehr als {2} Zeichen enthalten.")]
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy, HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { set; get; }
    }
}

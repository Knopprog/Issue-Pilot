using System;
using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public class TicketDataModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ein Ticket muss einen Titel haben!")]
        [StringLength(140, ErrorMessage = "Ein Titel darf nicht mehr als {2} Zeichen enthalten.")]
        public string Title { get; set; }
        [StringLength(5000, ErrorMessage = "Eine Beschreibung darf nicht mehr als {2} Zeichen enthalten.")]
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy, H:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Deadline { get; set; }
        public int Weight { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy, H:mm}", ApplyFormatInEditMode = true)]
        public DateTime? CloseDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy, H:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }
        public bool IsProjectOwner { get; set; }
        public bool IsCreator { get; set; }
    }
}

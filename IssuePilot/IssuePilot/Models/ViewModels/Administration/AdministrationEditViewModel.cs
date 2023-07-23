using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public class AdministrationEditViewModel
    {
        [DataType(DataType.Text)]
        public string Id { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Nutzername")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Dies ist ein Pflichtfeld.")]
        [DataType(DataType.Text)]
        [Display(Name = "Vorname")]
        [StringLength(20, ErrorMessage = "Ein Vorname darf nicht mehr als {2} Zeichen enthalten.")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Dies ist ein Pflichtfeld.")]
        [DataType(DataType.Text)]
        [Display(Name = "Nachname")]
        [StringLength(20, ErrorMessage = "Ein Nachname darf nicht mehr als {2} Zeichen enthalten.")]
        public string Surname { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "E-Mail")]
        [StringLength(254, ErrorMessage = "Eine E-Mail-Adresse darf nicht mehr als {2} Zeichen enthalten.")]
        [EmailAddress(ErrorMessage ="Diese E-Mail-Adresse ist ungültig.")]
        public string Email { get; set; }
        public string Role { get; set; }

        [Display(Name = "Aktuelle Rolle")]
        public string CurrentRole { get; set; }
    }
}

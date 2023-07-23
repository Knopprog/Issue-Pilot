using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public class AdministrationCreateViewModel
    {
        [Required(ErrorMessage = "Ein Benutzer muss einen Vornamen haben.")]
        [DataType(DataType.Text)]
        [Display(Name = "Vorname")]
        [StringLength(20, ErrorMessage = "Ein Vorname darf nicht mehr als {2} Zeichen enthalten.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Ein Benutzer muss einen Nachnamen haben.")]
        [DataType(DataType.Text)]
        [Display(Name = "Nachname")]
        [StringLength(20, ErrorMessage = "Ein Nachname darf nicht mehr als {2} Zeichen enthalten.")]
        public string Surname { get; set; }
        [StringLength(254, ErrorMessage = "Eine E-Mail-Adresse darf nicht mehr als {2} Zeichen enthalten.")]
        [DataType(DataType.Text)]
        [Display(Name = "E-Mail")]
        [EmailAddress(ErrorMessage = "Diese E-Mail-Adresse ist ungültig.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ein Benutzer muss ein Passwort haben.")]
        [StringLength(100, ErrorMessage = "Das {0} muss zwischen {2} und {1} Zeichen lang sein.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Passwort bestätigen")]
        [StringLength(100)]
        [Compare("Password", ErrorMessage = "Das neue Passwort stimmt nicht mit dem bestätigten Passwort überein.")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
    }
}

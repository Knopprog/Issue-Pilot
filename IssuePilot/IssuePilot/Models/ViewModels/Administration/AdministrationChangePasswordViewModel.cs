using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public class AdministrationChangePasswordViewModel
    {
        [DataType(DataType.Text)]
        public string Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Nutzername")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Ein Benutzer muss ein Passwort haben.")]
        [StringLength(100, ErrorMessage = "Das {0} muss zwischen {2} und {1} Zeichen lang sein.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Passwort bestätigen")]
        [Compare("Password", ErrorMessage = "Das neue Passwort stimmt nicht mit dem bestätigten Passwort überein.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Aktuelle Rolle")]
        public string CurrentRole { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public class AdministrationDeleteViewModel
    {
        [DataType(DataType.Text)]
        public string Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Nutzername")]
        public string UserName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Vorname")]
        public string Firstname { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Nachname")]
        public string Surname { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }
        [Display(Name = "Rolle")]
        public string Role { get; set; }
    }
}

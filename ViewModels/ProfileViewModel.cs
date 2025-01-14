using System.ComponentModel.DataAnnotations;

namespace Atelier_2.ViewModels
{
    public class ProfileViewModel
    {
        public string Email { get; set; }

        [Display(Name = "Adresse")]
        public string Adresse { get; set; }

        [Display(Name = "Numéro de Téléphone")]
        public string NumeroTelephone { get; set; }
    }
}
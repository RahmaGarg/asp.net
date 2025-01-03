using System.ComponentModel.DataAnnotations;

namespace Atelier_2.ViewModels
{
    public class CreateViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Désignation { get; set; }

        [Required]
        [Display(Name = "Prix en dinar :")]
        public float Prix { get; set; }
        // Durée de garantie en mois
        [Required]
        [Display(Name = "Durée de garantie (en mois) :")]
        public int GarantieDuréeEnMois { get; set; }
        [Required]
        [Display(Name = "Catégorie :")]
        public string Catégorie { get; set; }
    }
}

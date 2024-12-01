using System.ComponentModel.DataAnnotations;

namespace Atelier_2.ViewModels
{
    public class ReclamationViewModel
    {
        [Required(ErrorMessage = "Le produit est requis.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "La description est requise.")]
        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères.")]
        public string Description { get; set; }

        // Date d'achat du produit - la date réelle d'achat pour la réclamation
        [Required(ErrorMessage = "La date d'achat est requise.")]
        [DataType(DataType.Date)]
        public DateTime DateAchat { get; set; }
    }
}

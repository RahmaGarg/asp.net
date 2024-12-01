using System;
using System.ComponentModel.DataAnnotations;

namespace Atelier_2.Models
{
    public class Piece
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom { get; set; } // Nom de la pièce

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix de la pièce doit être positif.")]
        public decimal Prix { get; set; } // Prix de la pièce

        // Clé étrangère vers l'intervention
        public int InterventionId { get; set; }
        public Intervention Intervention { get; set; }
    }
}

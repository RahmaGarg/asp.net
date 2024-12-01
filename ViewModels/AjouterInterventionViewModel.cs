using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Atelier_2.ViewModels
{
    public class AjouterInterventionViewModel
    {
        public int ReclamationId { get; set; } // ID de la réclamation associée

        public bool NecessitePieces { get; set; } // Si des pièces sont nécessaires

        // Liste des pièces à ajouter à l'intervention
        public List<PieceViewModel> Pieces { get; set; } = new List<PieceViewModel>();

        [Required]
        public decimal DureeIntervention { get; set; } // Durée de l'intervention

        [Required]
        public decimal CoutTotal { get; set; } // Coût total de l'intervention

        [Required]
        public DateTime DateIntervention { get; set; } // Date de l'intervention
    }

    public class PieceViewModel
    {
        [Required]
        public string Nom { get; set; } // Nom de la pièce

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix de la pièce doit être positif.")]
        public decimal Prix { get; set; } // Prix de la pièce
    }

}
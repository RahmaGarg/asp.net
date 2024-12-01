using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipelines;

namespace Atelier_2.Models
{
    public class Intervention
    {
        public int Id { get; set; }

        // Clé étrangère vers la réclamation
        public int ReclamationId { get; set; }
        public Reclamation Reclamation { get; set; }
        // Indicateur si des pièces de rechange sont nécessaires
        public bool NecessitePieces { get; set; }

        // Liste des pièces utilisées pour l'intervention
        public ICollection<Piece> PiecesUtilisees { get; set; } = new List<Piece>();

        // Durée de l'intervention (en heures)
        public decimal DureeIntervention { get; set; } // Par exemple, durée en heures

        // Coût total de l'intervention
        public decimal CoutTotal { get; set; }

        // Date de l'intervention
        public DateTime DateIntervention { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
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
        // Propriétés de l'intervention
        public int ReclamationId { get; set; }  // Clé étrangère vers Reclamation
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
        // Clé étrangère pour l'utilisateur qui est le technicien
        public string TechnicienId { get; set; }

        // Navigation vers l'utilisateur (technicien)
        public IdentityUser Technicien { get; set; }
    }
}

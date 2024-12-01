using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Atelier_2.Models
{
    public class Reclamation
    {
        public int Id { get; set; }

        // Clé étrangère vers le produit
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // Description de la réclamation
        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; }
        // Date d'achat du produit
        [Required]
        public DateTime DateAchat { get; set; }
        // Propriétés pour la facturation
        public decimal MontantFacture { get; set; } = 0; // Montant de la facture si produit hors garantie
    

    // Identifiant du client qui a fait la réclamation (utilisation de l'UserId d'IdentityUser)
    public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public DateTime DateReclamation { get; set; }
        // Etat de la réclamation
        // Etat de la réclamation (par défaut à Creee)
        public EtatReclamation Etat { get; set; } = EtatReclamation.Creee;
        public List<Intervention> Interventions { get; set; } // Liste des interventions associées

    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Atelier_2.Models
{
    public class Reclamation
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        public DateTime DateAchat { get; set; }

        public decimal MontantFacture { get; set; } = 0;

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public DateTime DateReclamation { get; set; }
        public EtatReclamation Etat { get; set; } = EtatReclamation.Creee;

        // Association à une seule intervention
        public int? InterventionId { get; set; }
        public Intervention Intervention { get; set; }  // Relation 1:1
    }

}

using Atelier_2.Models;
using Atelier_2.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Atelier_2.Services
{
    public class ReclamationService
    {
        private readonly AppDbContext _context;

        public ReclamationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task TraiterReclamationAsync(int reclamationId)
        {
            var reclamation = await _context.Reclamations
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.Id == reclamationId);

            if (reclamation == null)
            {
                throw new ArgumentException("Réclamation non trouvée");
            }

            // Vérifier si le produit est sous garantie
            bool estSousGarantie = EstSousGarantie(reclamation);

            // Si le produit est hors garantie, traiter l'intervention et calculer la facture
            if (!estSousGarantie)
            {
                // Créer une intervention
                var intervention = new Intervention
                {
                    ReclamationId = reclamationId,
                    DateIntervention = DateTime.Now,
                    DureeIntervention = 2.5m, // Exemple: 2.5 heures d'intervention
                    PiecesUtilisees = await _context.Pieces.Where(p => p.InterventionId == reclamationId).ToListAsync()
                };

                // Calculer le coût des pièces
                decimal totalPieces = intervention.PiecesUtilisees.Sum(p => p.Prix);

                // Calculer le coût de la main-d'œuvre (par exemple 20 dinars/heure)
                decimal totalMainOeuvre = intervention.DureeIntervention * 20m;

                // Calculer le coût total de l'intervention
                intervention.CoutTotal = totalPieces + totalMainOeuvre;

                // Sauvegarder l'intervention
                _context.Interventions.Add(intervention);

                // Mettre à jour la réclamation avec l'état de l'intervention
                reclamation.MontantFacture = intervention.CoutTotal;
                reclamation.Etat = EtatReclamation.InterventionPlanifiee;

                await _context.SaveChangesAsync();
            }
        }

        // Vérifie si le produit est sous garantie
        private bool EstSousGarantie(Reclamation reclamation)
        {
            var produit = reclamation.Product;
            return DateTime.Now <= reclamation.DateAchat.AddMonths(produit.GarantieDuréeEnMois);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Atelier_2.Models;
using Atelier_2.Context;
using Atelier_2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Atelier_2.Controllers
{
    public class InterventionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public InterventionController(AppDbContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<IActionResult> AfficherIntervention(int id)
        {
            // Vérifier si l'intervention existe dans la base de données
            var intervention = await _context.Interventions
                .Include(i => i.Technicien)  // Inclure le technicien associé
                .Include(i => i.Reclamation)  // Inclure la réclamation associée
                .Include(i => i.PiecesUtilisees)  // Inclure les pièces utilisées si nécessaire
                .FirstOrDefaultAsync(i => i.Id == id);

            // Si l'intervention n'existe pas, retourner une page 404
            if (intervention == null)
            {
                return NotFound();
            }

            // Retourner la vue avec l'intervention
            return View(intervention);
        }
       

    // Action GET pour afficher le formulaire d'ajout d'intervention
    public async Task<IActionResult> AjouterIntervention(int reclamationId)
        {
            Console.WriteLine($"Début de l'action GET AjouterIntervention pour la réclamation ID: {reclamationId}");

            var reclamation = await _context.Reclamations
                .FirstOrDefaultAsync(r => r.Id == reclamationId);

            if (reclamation == null)
            {
                Console.WriteLine($"Réclamation avec ID {reclamationId} non trouvée.");
                return NotFound();
            }

            var techniciens = await _userManager.GetUsersInRoleAsync("Technicien");

            var availablePieces = await _context.Pieces
                .Select(p => new PieceSelectionViewModel
                {
                    PieceId = p.Id,
                    Nom = p.Nom,
                    IsChecked = false,  // Par défaut, les pièces ne sont pas sélectionnées
                    Quantity = 1        // Par défaut, la quantité est 1
                })
                .ToListAsync();

            var model = new AjouterInterventionViewModel
            {
                ReclamationId = reclamationId,
                DateIntervention = DateTime.Now,
                Techniciens = techniciens.Select(t => new SelectListItem
                {
                    Value = t.Id,
                    Text = t.UserName
                }).ToList() ?? new List<SelectListItem>(),
                Pieces = availablePieces ?? new List<PieceSelectionViewModel>()
            };

            return View(model);
        }

        // Action POST pour ajouter l'intervention après soumission du formulaire
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjouterIntervention(AjouterInterventionViewModel model)
        {
            // Vérifier la réclamation
            var reclamation = await _context.Reclamations
                .FirstOrDefaultAsync(r => r.Id == model.ReclamationId);

            if (reclamation == null)
            {
                ModelState.AddModelError("ReclamationId", "La réclamation spécifiée n'existe pas.");
                return View(model);
            }

            // Calculer le coût des pièces
            decimal coutPieces = 0;
            if (model.Pieces != null)
            {
                foreach (var piece in model.Pieces)
                {
                    if (piece.IsChecked)
                    {
                        coutPieces += piece.Quantity * piece.Prix;
                        Console.WriteLine($"Pièce: {piece.Nom}, Quantité: {piece.Quantity}, Prix Unitaire: {piece.Prix}, Coût: {piece.Quantity * piece.Prix}");
                    }
                }
            }

            Console.WriteLine($"Coût total des pièces: {coutPieces}");

            // Calculer le coût du technicien
            decimal tarifHoraire = 25.0m; // Tarif horaire défini
            decimal coutTechnicien = model.DureeIntervention * tarifHoraire;
            Console.WriteLine($"Durée intervention: {model.DureeIntervention} heures, Coût technicien: {coutTechnicien}");

            // Calculer le coût total
            decimal coutTotal = coutPieces + coutTechnicien;
            Console.WriteLine($"Coût total intervention: {coutTotal}");

            // Créer une nouvelle intervention
            var intervention = new Intervention
            {
                ReclamationId = model.ReclamationId,
                DateIntervention = model.DateIntervention,
                DureeIntervention = model.DureeIntervention,
                NecessitePieces = model.NecessitePieces,
                CoutTotal = coutTotal,
                TechnicienId = model.TechnicienId
            };

            // Ajouter l'intervention à la base de données
            _context.Interventions.Add(intervention);
            await _context.SaveChangesAsync();

            // Associer l'intervention à la réclamation
            reclamation.InterventionId = intervention.Id;

            // Mettre à jour l'état de la réclamation
            reclamation.Etat = EtatReclamation.InterventionPlanifiee;

            _context.Update(reclamation);
            await _context.SaveChangesAsync();

            return RedirectToAction("AfficherIntervention", new { id = intervention.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Récupérer l'intervention à supprimer
            var intervention = await _context.Interventions
                .Include(i => i.Technicien)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (intervention == null)
            {
                return NotFound();
            }

            return View(intervention); // Retourner une vue pour confirmer la suppression
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var intervention = await _context.Interventions.FindAsync(id);
            if (intervention == null)
            {
                return NotFound();
            }

            // Supprimer l'intervention
            _context.Interventions.Remove(intervention);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageInterventions)); // Rediriger vers la liste des interventions
        }
        public async Task<IActionResult> MesInterventions()
        {
            // Get the current authenticated user (technician)
            var technicienId = _userManager.GetUserId(User);

            if (technicienId == null)
            {
                return Unauthorized(); // If no user is authenticated, return Unauthorized
            }

            // Get all interventions assigned to the authenticated technician
            var interventions = await _context.Interventions
                .Include(i => i.Reclamation)  // Include related reclamation
                        .Include(i => i.Reclamation.Product)  // Inclure le produit de la réclamation
                                .Include(i => i.Reclamation.User)  // Inclure l'utilisateur de la réclamation

                .Where(i => i.TechnicienId == technicienId) // Filter by technician
                .ToListAsync();

            return View(interventions); // Return a view with the list of interventions
        }

        public async Task<IActionResult> ManageInterventions()
        {
            // Récupérer toutes les interventions, avec leurs techniciens et réclamations associées
            var interventions = await _context.Interventions
                .Include(i => i.Technicien) // Inclure les techniciens
                .Include(i => i.Reclamation) // Inclure les réclamations associées
                .ToListAsync();

            // Retourner la vue avec les données
            return View(interventions);
        }



    }

}

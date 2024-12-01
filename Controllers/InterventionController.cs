using Microsoft.AspNetCore.Mvc;
using Atelier_2.Models;
using Atelier_2.Context;
using Atelier_2.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Pour utiliser FirstOrDefaultAsync

namespace Atelier_2.Controllers
{
    public class InterventionController : Controller
    {
        private readonly AppDbContext _context;

        public InterventionController(AppDbContext context)
        {
            _context = context;
        }

        // Action pour afficher le formulaire d'ajout d'une intervention pour une réclamation spécifique
        public async Task<IActionResult> AjouterIntervention(int reclamationId)
        {
            // Récupérer la réclamation associée à cet ID de manière asynchrone
            var reclamation = await _context.Reclamations
                .FirstOrDefaultAsync(r => r.Id == reclamationId);

            if (reclamation == null)
            {
                return NotFound(); // Si la réclamation n'existe pas
            }

            // Créer un modèle vide pour l'ajout d'intervention, avec l'ID de la réclamation
            var model = new AjouterInterventionViewModel
            {
                ReclamationId = reclamationId,
                DateIntervention = DateTime.Now, // Date par défaut
                Pieces = new List<PieceViewModel>() // Liste vide pour commencer
            };

            return View(model); // Retourner la vue avec le modèle
        }

        // Action pour enregistrer l'intervention après soumission du formulaire
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjouterIntervention(AjouterInterventionViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Créer une nouvelle intervention à partir du modèle reçu
                var intervention = new Intervention
                {
                    ReclamationId = model.ReclamationId,
                    DateIntervention = model.DateIntervention,
                    DureeIntervention = model.DureeIntervention,
                    CoutTotal = model.CoutTotal,
                    NecessitePieces = model.NecessitePieces,
                    PiecesUtilisees = model.NecessitePieces && model.Pieces.Any()
                        ? model.Pieces.Select(p => new Piece { Nom = p.Nom, Prix = p.Prix }).ToList()
                        : null // Si aucune pièce n'est nécessaire, cette propriété reste nulle
                };

                // Ajouter l'intervention dans la base de données
                _context.Interventions.Add(intervention);
                await _context.SaveChangesAsync();

                // Rediriger vers la page de détails de la réclamation après avoir ajouté l'intervention
                return RedirectToAction("Details", "Reclamation", new { id = model.ReclamationId });
            }

            // Retourner la vue avec le modèle si les données sont invalides
            return View(model);
        }
    }
}

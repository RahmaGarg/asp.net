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
            Console.WriteLine("Début de l'action POST AjouterIntervention");

            // Vérifie si le modèle est null ou invalide
            if (model == null)
            {
                Console.WriteLine("Le modèle reçu est null.");
                ModelState.AddModelError("", "Le modèle est null.");
                return View(model);
            }

            // Vérifie si la propriété Pieces est null ou vide
            if (model.Pieces == null || !model.Pieces.Any(p => p.IsChecked)) // S'assurer qu'au moins une pièce est sélectionnée
            {
                Console.WriteLine("Aucune pièce sélectionnée.");
                ModelState.AddModelError("", "Aucune pièce sélectionnée.");
                return View(model);
            }

            // Vérifie que les quantités sont valides pour les pièces sélectionnées
            foreach (var piece in model.Pieces.Where(p => p.IsChecked))
            {
                if (piece.Quantity <= 0)
                {
                    ModelState.AddModelError("", $"La quantité pour la pièce {piece.Nom} est invalide.");
                    return View(model);
                }
            }

            // Calcul du coût total pour les pièces sélectionnées
            decimal coutTotal = model.Pieces
                .Where(p => p.IsChecked)
                .Sum(p => p.Quantity * p.Prix);

            // Créer l'objet Intervention et le sauvegarder
            var intervention = new Intervention
            {
                ReclamationId = model.ReclamationId,
                DateIntervention = model.DateIntervention,
                DureeIntervention = model.DureeIntervention,
                CoutTotal = coutTotal,
                NecessitePieces = model.NecessitePieces,
                TechnicienId = model.TechnicienId
            };

            // Sauvegarder l'intervention dans la base de données
            _context.Interventions.Add(intervention);
            await _context.SaveChangesAsync();

            return RedirectToAction("AfficherIntervention", new { id = intervention.Id });
        }
    

    }

}

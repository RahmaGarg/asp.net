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
                .Include(i => i.Reclamation)  // Inclure la réclamation associée (si applicable)
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
            // Debug: vérifier les valeurs transmises
            Console.WriteLine($"TechnicienId: {model.TechnicienId}");
            Console.WriteLine($"ReclamationId: {model.ReclamationId}");
            Console.WriteLine($"DateIntervention: {model.DateIntervention}");
            Console.WriteLine($"DureeIntervention: {model.DureeIntervention}");
            Console.WriteLine($"NecessitePieces: {model.NecessitePieces}");
            Console.WriteLine($"CoutTotal: {model.CoutTotal}");

            // Récupérer la réclamation pour vérifier que l'ID existe bien dans la base de données
            var reclamation = await _context.Reclamations
                .FirstOrDefaultAsync(r => r.Id == model.ReclamationId);

            if (reclamation == null)
            {
                ModelState.AddModelError("ReclamationId", "La réclamation spécifiée n'existe pas.");
                return View(model);  // Si la réclamation n'existe pas, on retourne la vue avec l'erreur
            }

            // Calcul du coût total pour les pièces sélectionnées
            decimal coutTotal = model.Pieces
                .Where(p => p.IsChecked)
                .Sum(p => p.Quantity * p.Prix);

            // Créer l'objet Intervention
            var intervention = new Intervention
            {
                ReclamationId = model.ReclamationId,  // Utiliser l'ID de réclamation du modèle
                DateIntervention = model.DateIntervention,
                DureeIntervention = model.DureeIntervention,
                NecessitePieces = model.NecessitePieces,
                CoutTotal = coutTotal,
                TechnicienId = model.TechnicienId
            };

            // Sauvegarder l'intervention dans la base de données
            _context.Interventions.Add(intervention);
            await _context.SaveChangesAsync();

            return RedirectToAction("AfficherIntervention", new { id = intervention.Id });
        }




    }

}

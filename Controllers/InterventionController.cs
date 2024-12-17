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

        // Injection de AppDbContext, UserManager et IConfiguration dans le constructeur
        public InterventionController(AppDbContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IActionResult> AjouterIntervention(int reclamationId)
        {
            // Récupérer la réclamation associée à cet ID
            var reclamation = await _context.Reclamations
                .FirstOrDefaultAsync(r => r.Id == reclamationId);

            if (reclamation == null)
            {
                return NotFound(); // Si la réclamation n'existe pas
            }

            // Récupérer tous les utilisateurs ayant le rôle "Technicien"
            var techniciens = await _userManager.GetUsersInRoleAsync("Technicien");

            // Exemple de pièces disponibles, à remplacer par des données réelles de votre base
            var availablePieces = await _context.Pieces
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nom
                })
                .ToListAsync();

            // Créer un modèle pour l'ajout d'intervention
            var model = new AjouterInterventionViewModel
            {
                ReclamationId = reclamationId,
                DateIntervention = DateTime.Now, // Date par défaut
                Techniciens = techniciens.Select(t => new SelectListItem
                {
                    Value = t.Id,
                    Text = t.UserName
                }).ToList(),
                AvailablePieces = availablePieces // Remplir la liste des pièces disponibles
            };

            return View(model);
        }

        // Action POST pour ajouter l'intervention après soumission du formulaire
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjouterIntervention(AjouterInterventionViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Récupérer le tarif horaire à partir de la configuration
                decimal tarifHoraire = decimal.Parse(_configuration["AppSettings:TarifHoraire"]);

                // Calculer le coût de l'intervention basé sur le tarif horaire et la durée de l'intervention
                decimal coutTechnicien = model.DureeIntervention * tarifHoraire;

                // Calculer le coût des pièces utilisées
                decimal coutPieces = 0;
                if (model.NecessitePieces && model.Pieces.Any())
                {
                    coutPieces = model.Pieces.Sum(p => p.Prix);
                }

                // Calculer le coût total de l'intervention
                decimal coutTotal = coutTechnicien + coutPieces;

                // Créer l'intervention
                var intervention = new Intervention
                {
                    ReclamationId = model.ReclamationId,
                    DateIntervention = model.DateIntervention,
                    DureeIntervention = model.DureeIntervention,
                    CoutTotal = coutTotal, // Coût total calculé
                    NecessitePieces = model.NecessitePieces,
                    TechnicienId = model.TechnicienId // Assigner le technicien sélectionné
                };

                // Si des pièces sont nécessaires et que des pièces ont été sélectionnées, ajouter ces pièces à l'intervention
                if (model.NecessitePieces && model.Pieces.Any())
                {
                    var pieces = model.Pieces.Select(p => new Piece
                    {
                        Nom = p.Nom,
                        Prix = p.Prix
                    }).ToList();

                    intervention.PiecesUtilisees = pieces;
                }

                // Ajouter l'intervention dans la base de données
                _context.Interventions.Add(intervention);
                await _context.SaveChangesAsync();

                // Rediriger vers la vue qui affiche l'intervention
                return RedirectToAction("AfficherIntervention", new { id = intervention.Id });
            }

            // Si le modèle est invalide, retourner à la vue avec les données nécessaires
            var techniciens = await _userManager.GetUsersInRoleAsync("Technicien");
            model.Techniciens = techniciens.Select(t => new SelectListItem
            {
                Value = t.Id,
                Text = t.UserName
            }).ToList();

            // Recharger la liste des pièces disponibles
            model.AvailablePieces = await _context.Pieces
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nom
                })
                .ToListAsync();

            return View(model);
        }

        // Action pour afficher l'intervention ajoutée
        public async Task<IActionResult> AfficherIntervention(int id)
        {
            var intervention = await _context.Interventions
                .Include(i => i.PiecesUtilisees)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (intervention == null)
            {
                return NotFound(); // Si l'intervention n'est pas trouvée
            }

            return View(intervention);
        }

    }
}

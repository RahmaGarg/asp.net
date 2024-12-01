using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Atelier_2.Models;
using Atelier_2.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System;
using Atelier_2.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Atelier_2.Controllers
{
    public class ReclamationController : Controller
    {
        private readonly AppDbContext _context;

        public ReclamationController(AppDbContext context)
        {
            _context = context;
        }

        // Afficher le formulaire pour ajouter une réclamation
        public IActionResult AjouterReclamation()
        {
            // Charger tous les produits pour que le client puisse les choisir dans le formulaire
            ViewBag.Produits = new SelectList(_context.Products, "Id", "Désignation");
            return View();
        }

        // Ajouter une nouvelle réclamation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TraiterReclamation(Reclamation reclamation)
        {
            if (ModelState.IsValid)
            {
                var existingReclamation = await _context.Reclamations
                                                        .FirstOrDefaultAsync(r => r.Id == reclamation.Id);

                if (existingReclamation == null)
                {
                    return NotFound();  // Si la réclamation n'existe pas
                }

                // Mettre à jour l'état de la réclamation
                existingReclamation.Etat = reclamation.Etat;

                // Sauvegarder les modifications dans la base de données
                _context.Update(existingReclamation);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "L'état de la réclamation a été mis à jour avec succès.";
                return RedirectToAction("All");  // Rediriger vers la liste des réclamations
            }

            return View(reclamation);
        }

        // Action pour afficher toutes les réclamations de tous les clients
        public async Task<IActionResult> All()
        {
            // Récupérer toutes les réclamations avec les informations du produit et du client
            var reclamations = await _context.Reclamations
                .Include(r => r.Product)  // Inclure les informations du produit
                .Include(r => r.User)     // Inclure les informations du client (User)
                .ToListAsync();

            return View(reclamations);  // Passer les réclamations à la vue
        }
        public async Task<IActionResult> TraiterReclamation(int id)
        {
            // Récupérer la réclamation à traiter
            var reclamation = await _context.Reclamations
                                             .Include(r => r.Product)  // Inclure le produit si nécessaire
                                             .FirstOrDefaultAsync(r => r.Id == id);

            if (reclamation == null)
            {
                return NotFound();  // Si la réclamation n'existe pas
            }

            // Retourner la vue pour traiter la réclamation
            return View(reclamation);
        }

        // Afficher la liste des réclamations d'un client
        public async Task<IActionResult> MesReclamations()
        {
            // Récupérer l'ID du client connecté
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Vérifier si l'utilisateur est authentifié
            if (string.IsNullOrEmpty(clientId))
            {
                return RedirectToAction("Login", "Account");  // Redirect to login page if not authenticated
            }

            // Récupérer les réclamations du client
            var reclamations = await _context.Reclamations
                                              .Where(r => r.UserId == clientId)
                                              .Include(r => r.Product) // Inclure le produit associé à chaque réclamation
                                              .ToListAsync();

            return View(reclamations);
        }
    }
}

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
        public async Task<IActionResult> AjouterReclamation(ReclamationViewModel viewModel)
        {
            Console.WriteLine("Début de la méthode AjouterReclamation");

            if (ModelState.IsValid)
            {
                Console.WriteLine("ModelState est valide");

                // Vérifiez si l'utilisateur est connecté
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    Console.WriteLine("L'utilisateur n'est pas connecté");
                    return RedirectToAction("Login", "Account");
                }

                Console.WriteLine($"Utilisateur connecté : {userId}");

                // Créer une nouvelle réclamation
                var reclamation = new Reclamation
                {
                    ProductId = viewModel.ProductId,
                    Description = viewModel.Description,
                    DateAchat = viewModel.DateAchat,
                    DateReclamation = DateTime.Now,
                    UserId = userId
                };

                Console.WriteLine("Nouvelle réclamation créée avec les données suivantes :");
                Console.WriteLine($"ProductId : {viewModel.ProductId}");
                Console.WriteLine($"Description : {viewModel.Description}");
                Console.WriteLine($"DateAchat : {viewModel.DateAchat}");
                Console.WriteLine($"DateReclamation : {reclamation.DateReclamation}");
                Console.WriteLine($"UserId : {reclamation.UserId}");

                try
                {
                    _context.Reclamations.Add(reclamation);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Réclamation ajoutée avec succès à la base de données.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'ajout à la base de données : {ex.Message}");
                    return View(viewModel);
                }

                TempData["SuccessMessage"] = "Réclamation ajoutée avec succès.";
                return RedirectToAction("MesReclamations");
            }

            Console.WriteLine("ModelState est invalide. Voici les erreurs :");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Erreur : {error.ErrorMessage}");
            }

            // Si le modèle est invalide, recharger la liste des produits
            ViewBag.Produits = new SelectList(_context.Products, "Id", "Désignation");
            return View(viewModel);
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

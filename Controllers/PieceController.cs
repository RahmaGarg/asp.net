using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Atelier_2.Models;
using Atelier_2.Context;
using Atelier_2.ViewModels;

namespace Atelier_2.Controllers
{
    public class PieceController : Controller
    {
        private readonly AppDbContext _context;

        public PieceController(AppDbContext context)
        {
            _context = context;
        }

        // Action GET pour afficher le formulaire d'ajout d'une intervention et des pièces
        public IActionResult AjouterIntervention()
        {
            var model = new AjouterInterventionViewModel
            {
                // Initialisation du ViewModel
                NecessitePieces = false, // Par défaut, l'intervention n'a pas besoin de pièces
                Pieces = new List<PieceViewModel>() // Liste vide de pièces
            };

            return View(model);
        }

        // Action POST pour ajouter l'intervention avec des pièces
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjouterIntervention(AjouterInterventionViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Créer l'entité Intervention
                var intervention = new Intervention
                {
                    ReclamationId = model.ReclamationId,
                    NecessitePieces = model.NecessitePieces,
                    DureeIntervention = model.DureeIntervention,
                    CoutTotal = model.CoutTotal,
                    DateIntervention = model.DateIntervention
                };

                // Si des pièces sont nécessaires, on ajoute ces pièces à l'intervention
                if (model.NecessitePieces && model.Pieces.Any())
                {
                    var pieces = model.Pieces.Select(p => new Piece
                    {
                        Nom = p.Nom,
                        Prix = p.Prix,
                    }).ToList();

                    intervention.PiecesUtilisees = pieces;
                }

                // Ajouter l'intervention dans la base de données
                _context.Interventions.Add(intervention);
                await _context.SaveChangesAsync();

                // Rediriger vers la page de gestion des interventions
                return RedirectToAction("ManageInterventions");
            }

            // Si le modèle est invalide, retourner à la vue avec les données nécessaires
            return View(model);
        }

        // Action GET pour afficher la liste des interventions
        public IActionResult ManageInterventions()
        {
            var interventions = _context.Interventions.ToList();
            return View(interventions);
        }
    }
}

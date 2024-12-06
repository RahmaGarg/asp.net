using Atelier_2.Models;
using Atelier_2.Models.Repositories;
using Atelier_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using System.Linq;

namespace Atelier_2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PieceController : Controller
    {
        private readonly IRepository<Piece> pieceRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        // Constructor for dependency injection
        public PieceController(IRepository<Piece> pieceRepository, IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.pieceRepository = pieceRepository;
        }

        // GET: PieceController
        public ActionResult Index()
        {
            var pieces = pieceRepository.GetAll();
            return View(pieces);  // Pass the pieces to the view
        }

        // GET: PieceController/Details/5
        public ActionResult Details(int id)
        {
            // Fetch the piece from the repository using the provided id
            var piece = pieceRepository.Get(id);

            // Check if the piece exists
            if (piece == null)
            {
                return NotFound(); // Return a 404 Not Found response if piece not found
            }

            // Pass the piece to the view
            return View(piece);
        }

        // GET: PieceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PieceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePieceViewModel model)
        {
            if (ModelState.IsValid)
            {
                Piece newPiece = new Piece
                {
                    Nom = model.Nom,
                    Prix = model.Prix
                };

                // Add the new piece to the repository
                pieceRepository.Add(newPiece);
                return RedirectToAction("Details", new { id = newPiece.Id });  // Redirect to Details after creation
            }
            return View(model);  // Return to create view if validation fails
        }

        // GET: PieceController/Edit/5
        public ActionResult Edit(int id)
        {
            Piece piece = pieceRepository.Get(id);
            if (piece == null)
            {
                return NotFound();  // Handle case when piece does not exist
            }

            // Create EditPieceViewModel populated with existing piece data
            EditPieceViewModel pieceEditViewModel = new EditPieceViewModel
            {
                Id = piece.Id,
                Nom = piece.Nom,
                Prix = piece.Prix
            };

            return View(pieceEditViewModel);  // Pass EditPieceViewModel to the view
        }

        // POST: PieceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPieceViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the piece being edited from the repository
                Piece piece = pieceRepository.Get(model.Id);
                if (piece == null)
                {
                    return NotFound();  // Handle case when piece is not found
                }

                // Update piece object with the new data
                piece.Nom = model.Nom;
                piece.Prix = model.Prix;

                // Update the piece in the repository
                Piece updatedPiece = pieceRepository.Update(piece);
                if (updatedPiece != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
            return View(model);  // Return to the edit view if validation fails
        }

        // GET: PieceController/Delete/5
        public ActionResult Delete(int id)
        {
            Piece piece = pieceRepository.Get(id);
            if (piece == null)
            {
                return NotFound();  // Handle case when piece does not exist
            }

            return View(piece);  // Pass piece to the view for confirmation
        }

        // POST: PieceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            Piece piece = pieceRepository.Delete(id);
            if (piece != null)
            {
                return RedirectToAction(nameof(Index));  // Redirect to index after successful deletion
            }
            return NotFound();  // If the piece does not exist, return NotFound
        }
    }
}

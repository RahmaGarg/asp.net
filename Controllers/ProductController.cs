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

    public class ProductController : Controller
    {
        private readonly IRepository<Product> productRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        // Constructor for dependency injection
        public ProductController(IRepository<Product> productRepository, IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.productRepository = productRepository;
        }

        // GET: ProductController
        public ActionResult Index()
        {
            var products = productRepository.GetAll();
            return View(products);  // Pass the products to the view
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            // Fetch the product from the repository using the provided id
            var product = productRepository.Get(id);

            // Check if the product exists
            if (product == null)
            {
                return NotFound(); // Return a 404 Not Found response if product not found
            }

            // Pass the product to the view
            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product newProduct = new Product
                {
                    Désignation = model.Désignation,
                    Prix = model.Prix,
                    Catégorie = model.Catégorie,  // Fixed: changed from 'Category' to 'Catégorie'
                };
                productRepository.Add(newProduct);
                return RedirectToAction("Details", new { id = newProduct.Id });  // Fixed: Changed "details" to "Details"
            }
            return View(model);
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            Product product = productRepository.Get(id);
            if (product == null)
            {
                return NotFound();  // Handle case when product does not exist
            }

            EditViewModel productEditViewModel = new EditViewModel
            {
                Id = product.Id,
                Désignation = product.Désignation,
                Prix = product.Prix,
                Catégorie = product.Catégorie,
                GarantieDuréeEnMois = product.GarantieDuréeEnMois,// Fixed: changed from 'Categorie' to 'Catégorie'
            };
            return View(productEditViewModel);  // Pass the EditViewModel to the view
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the product being edited from the database
                Product product = productRepository.Get(model.Id);
                if (product == null)
                {
                    return NotFound();  // Handle case when product is not found
                }

                // Update the product object with the data in the model object
                product.Désignation = model.Désignation;
                product.Prix = model.Prix;
                product.Catégorie = model.Catégorie;
                product.GarantieDuréeEnMois=model.GarantieDuréeEnMois; // Ajoutez cette ligne



                // Update the product in the repository
                Product updatedProduct = productRepository.Update(product);
                if (updatedProduct != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
            return View(model);
        }

       

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            Product product = productRepository.Get(id);
            if (product == null)
            {
                return NotFound();  // Handle case when product does not exist
            }

            return View(product);  // Pass product to the view for confirmation
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            Product product = productRepository.Delete(id);
            if (product != null)
            {
                return RedirectToAction(nameof(Index));  // Redirect to index after successful deletion
            }
            return NotFound();  // If the product does not exist, return NotFound
        }
    }
}

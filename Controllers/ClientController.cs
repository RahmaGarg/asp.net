using Atelier_2.Models;
using Atelier_2.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atelier_2.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IClientRepository clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        // GET: ClientController
        public ActionResult Index()
        {
            var clients = clientRepository.GetAll();
            return View(clients);  // Display the list of clients
        }

        // GET: ClientController/Details/5
        public ActionResult Details(int id)
        {
            var client = clientRepository.GetById(id);
            if (client == null)
            {
                return NotFound();  // Return 404 if client is not found
            }
            return View(client);  // Display the details of a client
        }

        // GET: ClientController/Create
        public ActionResult Create()
        {
            return View();  // Return a view to create a new client
        }

        // POST: ClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                clientRepository.Add(client);  // Add the new client to the repository
                return RedirectToAction(nameof(Index));  // Redirect to the list of clients
            }
            return View(client);  // If the model is not valid, return to the create view
        }

        // GET: ClientController/Edit/5
        public ActionResult Edit(int id)
        {
            var client = clientRepository.GetById(id);
            if (client == null)
            {
                return NotFound();  // Return 404 if client is not found
            }
            return View(client);  // Return a view to edit the client
        }

        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Client client)
        {
            if (id != client.ClientId)
            {
                return BadRequest();  // Return 400 if the IDs do not match
            }

            if (ModelState.IsValid)
            {
                clientRepository.Edit(client);  // Update the client in the repository
                return RedirectToAction(nameof(Index));  // Redirect to the list of clients
            }
            return View(client);  // If the model is not valid, return to the edit view
        }

        // GET: ClientController/Delete/5
        public ActionResult Delete(int id)
        {
            var client = clientRepository.GetById(id);
            if (client == null)
            {
                return NotFound();  // Return 404 if client is not found
            }
            return View(client);  // Return a view to confirm deletion
        }

        // POST: ClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {

            var client = clientRepository.GetById(id);
            if (client != null)
            {
                clientRepository.Delete(client);  // Delete the client from the repository
                return RedirectToAction(nameof(Index));  // Redirect to the list of clients
            }
            return NotFound();  // If the client does not exist, return 404
        }
    }
}

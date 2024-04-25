using BankingApp__easygames.Data;
using BankingApp__easygames.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BankingApp__easygames.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var clients = _context.Clients.ToList();
            return View(clients);
        }

        public IActionResult AddClient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddClient(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                _context.SaveChanges();
                // Set success message in TempData
                // TempData["SuccessMessage"] = "Client has been created successfully.";
                return RedirectToAction("Index") ;
            }
            return View(client);
        }

      
        [Authorize] 
        public IActionResult EditClient(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }


        [HttpPost]
        public IActionResult EditClient(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the client entity is being tracked
                    var existingClient = _context.Clients.Find(client.ClientID);
                    if (existingClient == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing client
                    existingClient.Name = client.Name;
                    existingClient.Surname = client.Surname;
                    existingClient.ClientBalance = client.ClientBalance;

                    // Update the client in the database
                    _context.Update(existingClient);
                    _context.SaveChanges();

                    
                    return RedirectToAction("Index", "Clients");
                }
                catch (Exception ex)
                {
                 
                    ModelState.AddModelError("", "Error editing client. Please try again.");
                    return View(client);
                }
            }
            return View(client);
        }


        [Authorize] 
        [HttpPost]
        public IActionResult DeleteClient(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }
            _context.Clients.Remove(client);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}

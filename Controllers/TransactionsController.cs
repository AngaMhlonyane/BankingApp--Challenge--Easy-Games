using BankingApp__easygames.Data;
using BankingApp__easygames.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace BankingApp__easygames.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var transactions = _context.Transactions
                .Include(t => t.TransactionType) 
                .Include(t => t.Client) 
                .ToList();

            return View(transactions);
        }

        public IActionResult AddTransaction()
        {
            // Retrieve clients from the database
            var clients = _context.Clients.ToList();

            // Convert clients to SelectListItem for dropdown
            var clientItems = clients.Select(c => new SelectListItem
            {
                Value = c.ClientID.ToString(),
                Text = $"{c.Name} {c.Surname}"
            }).ToList();

            // Add an option for selecting a client
           // clientItems.Insert(0, new SelectListItem { Value = "", Text = "Select Client" });

            // Assign the client items to ViewBag.Clients
            ViewBag.Clients = clientItems;

            // Create a new Transaction object
            var transaction = new Transaction();

            // Pass the Transaction object to the view
            return View(transaction);
        }


        /*public IActionResult AddTransaction(int? clientId)
        {
            if (clientId.HasValue)
            {
                var client = _context.Clients.Find(clientId);
                if (client == null)
                {
                    return NotFound();
                }

                var transaction = new Transaction { ClientID = clientId.Value };
                return View(transaction);
            }
            else
            {
                var transaction = new Transaction();
                return View(transaction);
            }
        }*/

        [HttpPost]
        public IActionResult AddTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the client associated with the transaction
                var client = _context.Clients.Find(transaction.ClientID);
                if (client == null)
                {
                    return NotFound("Client not found.");
                }

                // Retrieve the transaction type
                var transactionType = _context.TransactionTypes.Find(Convert.ToInt32(transaction.TransactionTypeID));
                if (transactionType == null)
                {
                    return NotFound("Transaction type not found.");
                }

                // Update the client balance based on the transaction type
                if (transactionType.TransactionTypeName == TransactionTypeName.debit)
                {
                    // Ensure client has sufficient balance for debit transaction
                    if (client.ClientBalance < transaction.Amount)
                    {
                        ModelState.AddModelError("", "Insufficient balance for debit transaction.");
                        return View(transaction);
                    }

                    client.ClientBalance -= transaction.Amount;
                }
                else if (transactionType.TransactionTypeName == TransactionTypeName.credit)
                {
                    client.ClientBalance += transaction.Amount;
                }
                else
                {
                    return BadRequest("Invalid transaction type.");
                }

                // Save the updated client balance
                _context.Update(client);
                _context.SaveChanges();

                // Save the transaction
                _context.Transactions.Add(transaction);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // If ModelState is not valid, return the view with validation errors
            return View(transaction);
        }

    }
}

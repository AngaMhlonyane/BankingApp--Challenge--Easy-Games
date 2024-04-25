using BankingApp__easygames.Data;
using BankingApp__easygames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp__easygames.Controllers
{
    public class TransactionTypesController : Controller


        {
            private readonly ApplicationDbContext _context;

            public TransactionTypesController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: TransactionTypes
            public async Task<IActionResult> Index()
            {
                var transactionTypes = await _context.TransactionTypes.ToListAsync();
                return View(transactionTypes);
            }

            // GET: TransactionTypes/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: TransactionTypes/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("TransactionTypeID,TransactionTypeName")] TransactionType transactionType)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(transactionType);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
                return View(transactionType);
            }
        }
    }

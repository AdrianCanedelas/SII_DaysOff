using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SII_DaysOff.Models;

namespace SII_DaysOff.Controllers
{
    public class RequestsController : Controller
    {
        private readonly DbContextBD _context;

        public RequestsController(DbContextBD context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            var dbContextBD = _context.Requests.Include(r => r.CreatedByNavigation).Include(r => r.ModifiedByNavigation).Include(r => r.Reason).Include(r => r.Status).Include(r => r.User);
            return View(await dbContextBD.ToListAsync());
        }

        public async Task<IActionResult> ManageIndex()
        {
            Console.WriteLine("pruebaIndex");
            var dbContextBD = _context.Requests;
            return View(await dbContextBD.ToListAsync());
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests
                .Include(r => r.CreatedByNavigation)
                .Include(r => r.ModifiedByNavigation)
                .Include(r => r.Reason)
                .Include(r => r.Status)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }

        [HttpGet]
        public async Task<JsonResult> GetUsersReasonsAndStatuses()
        {
            var users = await _context.AspNetUsers.Select(u => new { Id = u.Id, UserName = u.UserName }).ToListAsync();
            var reasons = await _context.Reasons.Select(r => new { Id = r.ReasonId, Name = r.Name }).ToListAsync();
            var statuses = await _context.Statuses.Select(s => new { Id = s.StatusId, Name = s.Name }).ToListAsync();

            return Json(new { users = users, reasons = reasons, statuses = statuses });
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
			Console.WriteLine("1");
			ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "ReasonId");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View(new Requests());
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,UserId,ReasonId,StatusId,RequestDate,StartDate,EndDate,HalfDayStart,HalfDayEnd,Comments,CreatedBy,CreationDate,ModifiedBy,ModificationDate")] Requests requests)
        {
			Console.WriteLine("2");
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var fieldName = entry.Key;
                    var fieldValue = entry.Value;

                    foreach (var error in fieldValue.Errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        Console.WriteLine($"Error en el campo: {fieldName}. Mensaje: {errorMessage}");
                    }
                }
            }
			if (ModelState.IsValid)
            {
				Console.WriteLine("3");
				requests.RequestId = Guid.NewGuid();
                _context.Add(requests);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.ModifiedBy);
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "ReasonId", requests.ReasonId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", requests.StatusId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.UserId);
			Console.WriteLine("4");
            var allRequests = await _context.Requests.ToListAsync();

            return View("~/Views/Home/Main.cshtml", allRequests);
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests.FindAsync(id);
            if (requests == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.ModifiedBy);
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "ReasonId", requests.ReasonId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", requests.StatusId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.UserId);
            return View(requests);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RequestId,UserId,ReasonId,StatusId,RequestDate,StartDate,EndDate,HalfDayStart,HalfDayEnd,Comments,CreatedBy,CreationDate,ModifiedBy,ModificationDate")] Requests requests)
        {
            if (id != requests.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requests);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestsExists(requests.RequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.ModifiedBy);
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "ReasonId", requests.ReasonId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", requests.StatusId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.UserId);
            return View(requests);
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Manage(int? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests.FindAsync(id);
            if (requests == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.ModifiedBy);
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "ReasonId", requests.ReasonId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", requests.StatusId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.UserId);
            return View(requests);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(Guid id, [Bind("IdRequest,IdUser,IdAdmin,IdReason,RequestDate,StartDate,EndDate,TotalDays,HalfDayStart,HalfDayEnd,Status")] Requests requests, string option)
        {
            if (id != requests.RequestId)
            {
                Console.WriteLine("1");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Console.WriteLine("2");
                try
                {
                    _context.Update(requests);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestsExists(requests.RequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            Console.WriteLine("3");
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.ModifiedBy);
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "ReasonId", requests.ReasonId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", requests.StatusId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.UserId);
            return View(requests);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests
                .Include(r => r.CreatedByNavigation)
                .Include(r => r.ModifiedByNavigation)
                .Include(r => r.Reason)
                .Include(r => r.Status)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Requests == null)
            {
                return Problem("Entity set 'DbContextBD.Requests'  is null.");
            }
            var requests = await _context.Requests.FindAsync(id);
            if (requests != null)
            {
                _context.Requests.Remove(requests);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestsExists(Guid id)
        {
          return (_context.Requests?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }
    }
}

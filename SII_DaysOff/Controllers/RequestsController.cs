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
            var dbContextBD = _context.Requests.Include(r => r.CreatedByNavigation).Include(r => r.ModifiedByNavigation).Include(r => r.Reason).Include(r => r.Request).Include(r => r.Status);
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
                .Include(r => r.Request)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "ReasonId");
            ViewData["RequestId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId");
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,UserId,ReasonId,StatusId,RequestDate,StartDate,EndDate,HalfDayStart,HalfDayEnd,Comments,CreatedBy,CreationDate,ModifiedBy,ModificationDate")] Requests requests)
        {
            if (ModelState.IsValid)
            {
                requests.RequestId = Guid.NewGuid();
                _context.Add(requests);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.ModifiedBy);
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "ReasonId", requests.ReasonId);
            ViewData["RequestId"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.RequestId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", requests.StatusId);
            return View(requests);
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
            ViewData["RequestId"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.RequestId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", requests.StatusId);
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
            ViewData["RequestId"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.RequestId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", requests.StatusId);
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
                .Include(r => r.Request)
                .Include(r => r.Status)
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

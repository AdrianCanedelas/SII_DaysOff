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
            var dbContextBD = _context.Requests.Include(r => r.IdAdminNavigation).Include(r => r.IdReasonNavigation).Include(r => r.IdUserNavigation);
            return View(await dbContextBD.ToListAsync());
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests
                .Include(r => r.IdAdminNavigation)
                .Include(r => r.IdReasonNavigation)
                .Include(r => r.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.IdRequest == id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            Console.WriteLine("1111111111111");
            ViewData["IdAdmin"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["IdReason"] = new SelectList(_context.Reasons, "IdReason", "IdReason");
            ViewData["IdUser"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRequest,IdUser,IdAdmin,IdReason,RequestDate,StartDate,EndDate,TotalDays,HalfDayStart,HalfDayEnd,Status")] Requests requests)
        {
			Console.WriteLine("22222222222222222");
			if (ModelState.IsValid)
            {
                _context.Add(requests);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            Console.WriteLine("aaa");
            ViewData["IdAdmin"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.IdAdmin);
            Console.WriteLine("bbb");
            ViewData["IdReason"] = new SelectList(_context.Reasons, "IdReason", "IdReason", requests.IdReason);
            Console.WriteLine("ccc");
            ViewData["IdUser"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.IdUser);
            Console.WriteLine("ddd");
            return View(requests);
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["IdAdmin"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.IdAdmin);
            ViewData["IdReason"] = new SelectList(_context.Reasons, "IdReason", "IdReason", requests.IdReason);
            ViewData["IdUser"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.IdUser);
            return View(requests);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRequest,IdUser,IdAdmin,IdReason,RequestDate,StartDate,EndDate,TotalDays,HalfDayStart,HalfDayEnd,Status")] Requests requests)
        {
            if (id != requests.IdRequest)
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
                    if (!RequestsExists(requests.IdRequest))
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
            ViewData["IdAdmin"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.IdAdmin);
            ViewData["IdReason"] = new SelectList(_context.Reasons, "IdReason", "IdReason", requests.IdReason);
            ViewData["IdUser"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.IdUser);
            return View(requests);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests
                .Include(r => r.IdAdminNavigation)
                .Include(r => r.IdReasonNavigation)
                .Include(r => r.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.IdRequest == id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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

        private bool RequestsExists(int id)
        {
          return (_context.Requests?.Any(e => e.IdRequest == id)).GetValueOrDefault();
        }
    }
}

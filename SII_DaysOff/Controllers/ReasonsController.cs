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
    public class ReasonsController : Controller
    {
        private readonly DbContextBD _context;

        public ReasonsController(DbContextBD context)
        {
            _context = context;
        }

        // GET: Reasons
        public async Task<IActionResult> Index()
        {
              return _context.Reasons != null ? 
                          View(await _context.Reasons.ToListAsync()) :
                          Problem("Entity set 'DbContextBD.Reasons'  is null.");
        }

        // GET: Reasons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reasons == null)
            {
                return NotFound();
            }

            var reasons = await _context.Reasons
                .FirstOrDefaultAsync(m => m.IdReason == id);
            if (reasons == null)
            {
                return NotFound();
            }

            return View(reasons);
        }

        // GET: Reasons/Create
        public IActionResult Create()
        {
			Console.WriteLine("creado reason1");
			return View();
        }

        // POST: Reasons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReason,ReasonName,DaysAssigned")] Reasons reasons)
        {
            Console.WriteLine("creado reason2");
            if (ModelState.IsValid)
            {
                _context.Add(reasons);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reasons);
        }

        // GET: Reasons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reasons == null)
            {
                return NotFound();
            }

            var reasons = await _context.Reasons.FindAsync(id);
            if (reasons == null)
            {
                return NotFound();
            }
            return View(reasons);
        }

        // POST: Reasons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReason,ReasonName,DaysAssigned")] Reasons reasons)
        {
            if (id != reasons.IdReason)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reasons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReasonsExists(reasons.IdReason))
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
            return View(reasons);
        }

        // GET: Reasons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reasons == null)
            {
                return NotFound();
            }

            var reasons = await _context.Reasons
                .FirstOrDefaultAsync(m => m.IdReason == id);
            if (reasons == null)
            {
                return NotFound();
            }

            return View(reasons);
        }

        // POST: Reasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reasons == null)
            {
                return Problem("Entity set 'DbContextBD.Reasons'  is null.");
            }
            var reasons = await _context.Reasons.FindAsync(id);
            if (reasons != null)
            {
                _context.Reasons.Remove(reasons);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReasonsExists(int id)
        {
          return (_context.Reasons?.Any(e => e.IdReason == id)).GetValueOrDefault();
        }
    }
}

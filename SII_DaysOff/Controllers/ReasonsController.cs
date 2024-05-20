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
            var dbContextBD = _context.Reasons.Include(r => r.CreatedByNavigation).Include(r => r.ModifiedByNavigation);
            return View(await dbContextBD.ToListAsync());
        }

        // GET: Reasons/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Reasons == null)
            {
                return NotFound();
            }

            var reasons = await _context.Reasons
                .Include(r => r.CreatedByNavigation)
                .Include(r => r.ModifiedByNavigation)
                .FirstOrDefaultAsync(m => m.ReasonId == id);
            if (reasons == null)
            {
                return NotFound();
            }

            return View(reasons);
        }

        // GET: Reasons/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Reasons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReasonId,Name,Description,CreatedBy,CreationDate,ModifiedBy,ModificationDate")] Reasons reasons)
        {
            if (ModelState.IsValid)
            {
                reasons.ReasonId = Guid.NewGuid();
                _context.Add(reasons);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", reasons.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", reasons.ModifiedBy);
            return View(reasons);
        }

        // GET: Reasons/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
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
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", reasons.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", reasons.ModifiedBy);
            return View(reasons);
        }

        // POST: Reasons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ReasonId,Name,Description,CreatedBy,CreationDate,ModifiedBy,ModificationDate")] Reasons reasons)
        {
            if (id != reasons.ReasonId)
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
                    if (!ReasonsExists(reasons.ReasonId))
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
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", reasons.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", reasons.ModifiedBy);
            return View(reasons);
        }

        // GET: Reasons/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Reasons == null)
            {
                return NotFound();
            }

            var reasons = await _context.Reasons
                .Include(r => r.CreatedByNavigation)
                .Include(r => r.ModifiedByNavigation)
                .FirstOrDefaultAsync(m => m.ReasonId == id);
            if (reasons == null)
            {
                return NotFound();
            }

            return View(reasons);
        }

        // POST: Reasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
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

        private bool ReasonsExists(Guid id)
        {
          return (_context.Reasons?.Any(e => e.ReasonId == id)).GetValueOrDefault();
        }
    }
}

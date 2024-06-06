using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SII_DaysOff.Data;
using SII_DaysOff.Models;

namespace SII_DaysOff.Controllers
{
    public class StatusesController : Controller
    {
        private readonly DbContextBD _context;

        public StatusesController(DbContextBD context)
        {
            _context = context;
        }

        // GET: Statuses
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? numPage, string currentFilter, int registerCount)
        {
            //Ordenación
            ViewData["NameOrder"] = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewData["DescriptionOrder"] = sortOrder == "Description" ? "Description_desc" : "Description";

            //Cuadro de búsqueda
            ViewData["CurrentFilter"] = searchString;

            var statuses = _context.Statuses.Include(r => r.CreatedByNavigation).Include(r => r.ModifiedByNavigation).AsQueryable();
            //Paginacion
            if (searchString != null) numPage = 1;
            else searchString = currentFilter;

            if (!String.IsNullOrEmpty(searchString))
            {
                statuses = statuses.Where(r => r.Name.Contains(searchString)
                || r.Description.Contains(searchString));
            }

            ViewData["CurrentOrder"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;
            if (registerCount == 0) registerCount = 5;
            ViewData["RegisterCount"] = registerCount;

            switch (sortOrder)
            {
                default:
                    statuses = statuses.OrderBy(r => r.Name);
                    break;
                case "Name_desc":
                    statuses = statuses.OrderByDescending(r => r.Name);
                    break;
                case "Description_desc":
                    statuses = statuses.OrderByDescending(r => r.Description);
                    break;
                case "Description":
                    statuses = statuses.OrderBy(r => r.Description);
                    break;
            }

            return View(await PaginatedList<Statuses>.CreateAsync(statuses.AsNoTracking(), numPage ?? 1, registerCount));
        }

        // GET: Statuses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Statuses == null)
            {
                return NotFound();
            }

            var statuses = await _context.Statuses
                .Include(s => s.CreatedByNavigation)
                .Include(s => s.ModifiedByNavigation)
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (statuses == null)
            {
                return NotFound();
            }

            return View(statuses);
        }

        // GET: Statuses/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Statuses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusId,Name,Description,CreatedBy,CreationDate,ModifiedBy,ModificationDate")] Statuses statuses)
        {
            if (ModelState.IsValid)
            {
                statuses.StatusId = Guid.NewGuid();
                _context.Add(statuses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", statuses.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", statuses.ModifiedBy);
            return View(statuses);
        }

        // GET: Statuses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Statuses == null)
            {
                return NotFound();
            }

            var statuses = await _context.Statuses.FindAsync(id);
            if (statuses == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", statuses.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", statuses.ModifiedBy);
            return View(statuses);
        }

        // POST: Statuses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("StatusId,Name,Description,CreatedBy,CreationDate,ModifiedBy,ModificationDate")] Statuses statuses)
        {
            if (id != statuses.StatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statuses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusesExists(statuses.StatusId))
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
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", statuses.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", statuses.ModifiedBy);
            return View(statuses);
        }

        // GET: Statuses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Statuses == null)
            {
                return NotFound();
            }

            var statuses = await _context.Statuses
                .Include(s => s.CreatedByNavigation)
                .Include(s => s.ModifiedByNavigation)
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (statuses == null)
            {
                return NotFound();
            }

            return View(statuses);
        }

        // POST: Statuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Statuses == null)
            {
                return Problem("Entity set 'DbContextBD.Statuses'  is null.");
            }
            var statuses = await _context.Statuses.FindAsync(id);
            if (statuses != null)
            {
                _context.Statuses.Remove(statuses);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusesExists(Guid id)
        {
          return (_context.Statuses?.Any(e => e.StatusId == id)).GetValueOrDefault();
        }
    }
}

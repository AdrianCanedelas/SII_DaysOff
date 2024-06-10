using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SII_DaysOff.Areas.Identity.Data;
using SII_DaysOff.Data;
using SII_DaysOff.Models;

namespace SII_DaysOff.Controllers
{
    public class VacationDaysController : Controller
    {
        private readonly DbContextBD _context;
        private UserManager<ApplicationUser> _userManager;

        public VacationDaysController(DbContextBD context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: VacationDays
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? numPage, string currentFilter, int registerCount)
        {
			Console.WriteLine("\n\n\n\n SortOrder -> " + sortOrder);
			//Ordenación
			ViewData["YearOrder"] = String.IsNullOrEmpty(sortOrder) ? "Year_desc" : "";
			ViewData["DaysVacationOrder"] = sortOrder == "DaysVacation" ? "DaysVacation_desc" : "DaysVacation";

			//Cuadro de búsqueda
			ViewData["CurrentFilter"] = searchString;

			var vacationDays = _context.VacationDays.Include(r => r.CreatedByNavigation).Include(r => r.ModifiedByNavigation).AsQueryable();

			//Paginacion
			if (searchString != null) numPage = 1;
			else searchString = currentFilter;

			if (!String.IsNullOrEmpty(searchString))
			{
				vacationDays = vacationDays.Where(r => r.Year.Contains(searchString)
				|| r.DayVacations.ToString().Contains(searchString));
			}

			ViewData["CurrentOrder"] = sortOrder;
			ViewData["CurrentFilter"] = searchString;
			if (registerCount == 0) registerCount = 5;
			ViewData["RegisterCount"] = registerCount;

			ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Email");
			ViewData["YearId"] = new SelectList(_context.VacationDays, "Year", "Year");

			switch (sortOrder)
			{
				default:
					Console.WriteLine("1");
					vacationDays = vacationDays.OrderBy(r => r.Year);
					break;
				case "Year_desc":
					Console.WriteLine("2");
					vacationDays = vacationDays.OrderByDescending(r => r.Year);
					break;
				case "DaysVacation_desc":
					Console.WriteLine("3");
					vacationDays = vacationDays.OrderByDescending(r => r.DayVacations);
					break;
				case "DaysVacation":
					Console.WriteLine("4");
					vacationDays = vacationDays.OrderBy(r => r.DayVacations);
					break;
			}

			return View(await PaginatedList<VacationDays>.CreateAsync(vacationDays.AsNoTracking(), numPage ?? 1, registerCount));
        }

        // GET: VacationDays/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.VacationDays == null)
            {
                return NotFound();
            }

            var vacationDays = await _context.VacationDays
                .Include(v => v.CreatedByNavigation)
                .Include(v => v.ModifiedByNavigation)
                .FirstOrDefaultAsync(m => m.Year == id);
            if (vacationDays == null)
            {
                return NotFound();
            }

            return View(vacationDays);
        }

        // GET: VacationDays/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: VacationDays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Year,DayVacations")] VacationDays vacationDays)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                vacationDays.CreatedBy = user.Id;
                vacationDays.ModifiedBy = user.Id;
                vacationDays.CreationDate = DateTime.Now;
                vacationDays.ModificationDate = DateTime.Now;
                _context.Add(vacationDays);
                await _context.SaveChangesAsync();
                return LocalRedirect("~/Home/Main");
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", vacationDays.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", vacationDays.ModifiedBy);
            return LocalRedirect("~/Home/Main");
        }

        // GET: VacationDays/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.VacationDays == null)
            {
                return NotFound();
            }

            var vacationDays = await _context.VacationDays.FindAsync(id);
            if (vacationDays == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", vacationDays.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", vacationDays.ModifiedBy);
            return View(vacationDays);
        }

        // POST: VacationDays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Year,DayVacations,CreatedBy,CreationDate,ModifiedBy,ModificationDate")] VacationDays vacationDays)
        {
            if (id != vacationDays.Year)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacationDays);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacationDaysExists(vacationDays.Year))
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
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", vacationDays.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", vacationDays.ModifiedBy);
            return View(vacationDays);
        }

        // GET: VacationDays/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.VacationDays == null)
            {
                return NotFound();
            }

            var vacationDays = await _context.VacationDays
                .Include(v => v.CreatedByNavigation)
                .Include(v => v.ModifiedByNavigation)
                .FirstOrDefaultAsync(m => m.Year == id);
            if (vacationDays == null)
            {
                return NotFound();
            }

            return View(vacationDays);
        }

        // POST: VacationDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.VacationDays == null)
            {
                return Problem("Entity set 'DbContextBD.VacationDays'  is null.");
            }
            var vacationDays = await _context.VacationDays.FindAsync(id);
            if (vacationDays != null)
            {
                _context.VacationDays.Remove(vacationDays);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacationDaysExists(string id)
        {
          return (_context.VacationDays?.Any(e => e.Year == id)).GetValueOrDefault();
        }
    }
}

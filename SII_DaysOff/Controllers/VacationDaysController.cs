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
		public readonly IHttpContextAccessor _contextAccessor;

		public VacationDaysController(DbContextBD context, UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccesor)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccesor;
        }

        // GET: VacationDays
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? numPage, string currentFilter, int registerCount)
        {
			ViewData["YearOrder"] = String.IsNullOrEmpty(sortOrder) ? "Year_desc" : "";
			ViewData["DaysVacationOrder"] = sortOrder == "DaysVacation" ? "DaysVacation_desc" : "DaysVacation";

			ViewData["CurrentFilter"] = searchString;

			var vacationDays = _context.VacationDays.Include(r => r.CreatedByNavigation).Include(r => r.ModifiedByNavigation).AsQueryable();

			if (searchString != null && !searchString.Equals("-1"))
			{
				numPage = 1;
				_contextAccessor.HttpContext.Session.SetString("searchStringVacationDays", searchString);
			}
			else if (searchString == null)
			{
				_contextAccessor.HttpContext.Session.SetString("searchStringVacationDays", "");
			}

			if (!String.IsNullOrEmpty(_contextAccessor.HttpContext.Session.GetString("searchStringVacationDays")))
			{
				vacationDays = vacationDays.Where(r => r.Year.Contains(_contextAccessor.HttpContext.Session.GetString("searchStringVacationDays"))
				|| r.DayVacations.ToString().Contains(_contextAccessor.HttpContext.Session.GetString("searchStringVacationDays")));
			}

			ViewData["CurrentOrder"] = sortOrder;
			ViewData["CurrentFilter"] = _contextAccessor.HttpContext.Session.GetString("searchStringVacationDays");
			if (registerCount == 0) _contextAccessor.HttpContext.Session.SetInt32("registerCountVacationDays", 5);
			else if (registerCount == null) _contextAccessor.HttpContext.Session.SetInt32("registerCountVacationDays", 5);
			else if (registerCount != 11) _contextAccessor.HttpContext.Session.SetInt32("registerCountVacationDays", registerCount);
			ViewData["RegisterCount"] = _contextAccessor.HttpContext.Session.GetInt32("registerCountVacationDays");
			if (ViewData["RegisterCount"] == null) ViewData["RegisterCount"] = 5;

			ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Email");
			ViewData["YearId"] = new SelectList(_context.VacationDays, "Year", "Year");

			switch (sortOrder)
			{
				default:
					vacationDays = vacationDays.OrderBy(r => r.Year);
					break;
				case "Year_desc":
					vacationDays = vacationDays.OrderByDescending(r => r.Year);
					break;
				case "DaysVacation_desc":
					vacationDays = vacationDays.OrderByDescending(r => r.DayVacations);
					break;
				case "DaysVacation":
					vacationDays = vacationDays.OrderBy(r => r.DayVacations);
					break;
			}

			var registerCountVacationDays = 5;
			if (_contextAccessor.HttpContext.Session.GetInt32("registerCountVacationDays") != null) registerCountVacationDays = (int)_contextAccessor.HttpContext.Session.GetInt32("registerCountVacationDays");
			var paginatedVacationDays = await PaginatedList<VacationDays>.CreateAsync(vacationDays.AsNoTracking(), numPage ?? 1, registerCountVacationDays);
			var viewModel = new MainViewModel
			{
				VacationDays = paginatedVacationDays,
				TotalRequest = vacationDays.Count(),
				PageSize = registerCountVacationDays,
			};

			return View(viewModel);
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

				TempData["toastMessage"] = "The vacation days has been created";

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
					var user = await _userManager.GetUserAsync(User);

					vacationDays.ModificationDate = DateTime.Now;
					vacationDays.ModifiedBy = user.Id;

					_context.Update(vacationDays);
                    await _context.SaveChangesAsync();
					TempData["toastMessage"] = "The vacation days has been edited";
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
			TempData["toastMessage"] = "The vacation days has been deleted";
			return RedirectToAction(nameof(Index));
        }

        private bool VacationDaysExists(string id)
        {
          return (_context.VacationDays?.Any(e => e.Year == id)).GetValueOrDefault();
        }
    }
}

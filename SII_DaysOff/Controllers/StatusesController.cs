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
    public class StatusesController : Controller
    {
        private readonly DbContextBD _context;
        private UserManager<ApplicationUser> _userManager;
		public readonly IHttpContextAccessor _contextAccessor;

		public StatusesController(DbContextBD context, UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccesor)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccesor;
        }

        // GET: Statuses
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? numPage, string currentFilter, int registerCount)
        {
            ViewData["NameOrder"] = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewData["DescriptionOrder"] = sortOrder == "Description" ? "Description_desc" : "Description";

			ViewData["CurrentFilter"] = searchString;

			var statuses = _context.Statuses.Include(r => r.CreatedByNavigation).Include(r => r.ModifiedByNavigation).AsQueryable();

			if (searchString != null && !searchString.Equals("-1"))
			{
				numPage = 1;
				_contextAccessor.HttpContext.Session.SetString("searchStringStatus", searchString);
			}
			else if (searchString == null)
			{
				_contextAccessor.HttpContext.Session.SetString("searchStringStatus", "");
			}

			if (!String.IsNullOrEmpty(_contextAccessor.HttpContext.Session.GetString("searchStringStatus")))
            {
                statuses = statuses.Where(r => r.Name.Contains(_contextAccessor.HttpContext.Session.GetString("searchStringStatus"))
                || r.Description.Contains(_contextAccessor.HttpContext.Session.GetString("searchStringStatus")));
            }

            ViewData["CurrentOrder"] = sortOrder;
			ViewData["CurrentFilter"] = _contextAccessor.HttpContext.Session.GetString("searchStringStatus");
			if (registerCount == 0) _contextAccessor.HttpContext.Session.SetInt32("registerCountStatus", 5);
			else if (registerCount == null) _contextAccessor.HttpContext.Session.SetInt32("registerCountStatus", 5);
			else if (registerCount != 11) _contextAccessor.HttpContext.Session.SetInt32("registerCountStatus", registerCount);
			ViewData["RegisterCount"] = _contextAccessor.HttpContext.Session.GetInt32("registerCountStatus");
			if (ViewData["RegisterCount"] == null) ViewData["RegisterCount"] = 5;

			ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Email");
			ViewData["YearId"] = new SelectList(_context.VacationDays, "Year", "Year");

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

			var registerCountStatus = 5;
			if (_contextAccessor.HttpContext.Session.GetInt32("registerCountStatus") != null) registerCountStatus = (int)_contextAccessor.HttpContext.Session.GetInt32("registerCountStatus");
			var paginatedStatuses = await PaginatedList<Statuses>.CreateAsync(statuses.AsNoTracking(), numPage ?? 1, registerCountStatus);
			var viewModel = new MainViewModel
			{
				Statuses = paginatedStatuses,
				TotalRequest = statuses.Count(),
				PageSize = registerCountStatus,
			};

			return View(viewModel);
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
        public async Task<IActionResult> Create([Bind("Name,Description")] Statuses statuses)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                statuses.StatusId = Guid.NewGuid();
                statuses.CreatedBy = user.Id;
                statuses.ModifiedBy = user.Id;
                statuses.CreationDate = DateTime.Now;
                statuses.ModificationDate = DateTime.Now;

                _context.Add(statuses);
                await _context.SaveChangesAsync();

				TempData["toastMessage"] = "The status has been created";

				return LocalRedirect("~/Home/Main");
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", statuses.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", statuses.ModifiedBy);
            return LocalRedirect("~/Home/Main");
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
					var user = await _userManager.GetUserAsync(User);

					statuses.ModificationDate = DateTime.Now;
					statuses.ModifiedBy = user.Id;

					_context.Update(statuses);
                    await _context.SaveChangesAsync();

					TempData["toastMessage"] = "The status has been edited";
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

			TempData["toastMessage"] = "The status has been deleted";

			return RedirectToAction(nameof(Index));
        }

        private bool StatusesExists(Guid id)
        {
          return (_context.Statuses?.Any(e => e.StatusId == id)).GetValueOrDefault();
        }
    }
}

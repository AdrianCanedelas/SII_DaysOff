using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SII_DaysOff.Areas.Identity.Data;
using SII_DaysOff.Data;
using SII_DaysOff.Models;

namespace SII_DaysOff.Controllers
{
    public class ReasonsController : Controller
    {
        private readonly DbContextBD _context;
        private UserManager<ApplicationUser> _userManager;

        public ReasonsController(DbContextBD context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reasons
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? numPage, string currentFilter, int registerCount)
        {
			ViewData["NameOrder"] = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
			ViewData["DescriptionOrder"] = sortOrder == "Description" ? "Description_desc" : "Description";

			ViewData["CurrentFilter"] = searchString;

			var reasons = _context.Reasons.Include(r => r.CreatedByNavigation).Include(r => r.ModifiedByNavigation).AsQueryable();

			if (searchString != null) numPage = 1;
			else searchString = currentFilter;

			if (!String.IsNullOrEmpty(searchString))
			{
                reasons = reasons.Where(r => r.Name.Contains(searchString)
                || r.Description.Contains(searchString));
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
					reasons = reasons.OrderBy(r => r.Name);
					break;
				case "Name_desc":
					reasons = reasons.OrderByDescending(r => r.Name);
					break;
				case "Description_desc":
					reasons = reasons.OrderByDescending(r => r.Description);
					break;
				case "Description":
					reasons = reasons.OrderBy(r => r.Description);
					break;
			}

			var paginatedReasons = await PaginatedList<Reasons>.CreateAsync(reasons.AsNoTracking(), numPage ?? 1, registerCount);
			var viewModel = new MainViewModel
			{
				Reasons = paginatedReasons,
				TotalRequest = reasons.Count(),
				PageSize = registerCount,
			};

			return View(viewModel);
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
        public async Task<IActionResult> Create([Bind("Name,Description")] Reasons reasons)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                reasons.ReasonId = Guid.NewGuid();
                reasons.CreatedBy = user.Id;
                reasons.ModifiedBy = user.Id;
                reasons.CreationDate = DateTime.Now;
                reasons.ModificationDate = DateTime.Now;

                _context.Add(reasons);
                await _context.SaveChangesAsync();

				TempData["toastMessage"] = "The reasons has been created";

				return LocalRedirect("~/Home/Main");
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", reasons.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", reasons.ModifiedBy);
            return LocalRedirect("~/Home/Main");
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
					var user = await _userManager.GetUserAsync(User);

					reasons.ModificationDate = DateTime.Now;
					reasons.ModifiedBy = user.Id;

					_context.Update(reasons);
                    await _context.SaveChangesAsync();
					TempData["toastMessage"] = "The reasons has been edited";
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
			TempData["toastMessage"] = "The reasons has been deleted";
			return RedirectToAction(nameof(Index));
        }

        private bool ReasonsExists(Guid id)
        {
          return (_context.Reasons?.Any(e => e.ReasonId == id)).GetValueOrDefault();
        }
    }
}

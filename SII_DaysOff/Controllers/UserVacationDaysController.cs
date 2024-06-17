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
    public class UserVacationDaysController : Controller
    {
        private readonly DbContextBD _context;
		private UserManager<ApplicationUser> _userManager;

		public UserVacationDaysController(DbContextBD context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserVacationDays
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? numPage, string currentFilter, int registerCount)
        {
			ViewData["UserOrder"] = String.IsNullOrEmpty(sortOrder) ? "User_desc" : "";
			ViewData["YearOrder"] = sortOrder == "Year" ? "Year_desc" : "Year";
			ViewData["AcquiredDaysOrder"] = sortOrder == "AcquiredDays" ? "AcquiredDays_desc" : "AcquiredDays";
			ViewData["AdditionalDaysOrder"] = sortOrder == "AdditionalDays" ? "AdditionalDays_desc" : "AdditionalDays";

			ViewData["CurrentFilter"] = searchString;

			var userVacationDays = _context.UserVacationDays.Include(r => r.CreatedByNavigation).Include(r => r.ModifiedByNavigation).Include(r => r.User).AsQueryable();

			if (searchString != null) numPage = 1;
			else searchString = currentFilter;

			if (!String.IsNullOrEmpty(searchString))
			{
				userVacationDays = userVacationDays.Where(r => r.User.Name.Contains(searchString)
				|| r.Year.Contains(searchString)
				|| r.AcquiredDays.ToString().Contains(searchString)
				|| r.AdditionalDays.ToString().Contains(searchString));
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
					userVacationDays = userVacationDays.OrderBy(r => r.User.Name);
					break;
				case "User_desc":
					userVacationDays = userVacationDays.OrderByDescending(r => r.User.Name);
					break;
				case "Year_desc":
					userVacationDays = userVacationDays.OrderByDescending(r => r.Year);
					break;
				case "Year":
					userVacationDays = userVacationDays.OrderBy(r => r.Year);
					break;
				case "AcquiredDays_desc":
					userVacationDays = userVacationDays.OrderByDescending(r => r.AcquiredDays);
					break;
				case "AcquiredDays":
					userVacationDays = userVacationDays.OrderBy(r => r.AcquiredDays);
					break;
				case "AdditionalDays_desc":
					userVacationDays = userVacationDays.OrderByDescending(r => r.AdditionalDays);
					break;
				case "AdditionalDays":
					userVacationDays = userVacationDays.OrderBy(r => r.AdditionalDays);
					break;
			}

			var paginatedUserVacationDays = await PaginatedList<UserVacationDays>.CreateAsync(userVacationDays.AsNoTracking(), numPage ?? 1, registerCount);
			var viewModel = new MainViewModel
			{
				UserVacationDays = paginatedUserVacationDays,
				TotalRequest = userVacationDays.Count(),
				PageSize = registerCount,
			};

			return View(viewModel);
        }

        public SelectList getYearsByUserId(Guid id)
        {
            var totalYears = _context.VacationDays.Select(v => v.Year).Distinct().ToList();
            var userSelectedYears = _context.UserVacationDays.Where(u => u.UserId.Equals(id)).Select(u => u.Year).ToList();

            return new SelectList(totalYears.Where(v => !userSelectedYears.Contains(v)).ToList());
        }

        // GET: UserVacationDays/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.UserVacationDays == null)
            {
                return NotFound();
            }

            var userVacationDays = await _context.UserVacationDays
                .Include(u => u.CreatedByNavigation)
                .Include(u => u.ModifiedByNavigation)
                .Include(u => u.User)
                .Include(u => u.YearNavigation)
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (userVacationDays == null)
            {
                return NotFound();
            }

            return View(userVacationDays);
        }

		public void yearSelectList(Guid selectedUserId)
		{
			var occupedYears = _context.UserVacationDays
				.Where(u => u.UserId.Equals(selectedUserId))
				.Select(u => u.Year)
				.ToList();

			var allYears = _context.VacationDays
				.Select(v => v.Year)
				.Distinct()
				.ToList();

			var availableYears = allYears.Except(occupedYears).ToList();
			ViewData["YearId"] = new SelectList(availableYears);
		}


		// GET: UserVacationDays/Create
		public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["Year"] = new SelectList(_context.VacationDays, "Year", "Year");

            return View();
        }

        // POST: UserVacationDays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Year,AcquiredDays,AdditionalDays,CreatedBy,CreationDate,ModifiedBy,ModificationDate")] UserVacationDays userVacationDays)
        {
            if (ModelState.IsValid)
			{
				var user = await _userManager.GetUserAsync(User);

				userVacationDays.CreatedBy = user.Id;
				userVacationDays.ModifiedBy = user.Id;
				userVacationDays.CreationDate = DateTime.Now;
				userVacationDays.ModificationDate = DateTime.Now;

                _context.Add(userVacationDays);
                await _context.SaveChangesAsync();

				TempData["toastMessage"] = "The user vacation days has been created";

				return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", userVacationDays.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", userVacationDays.ModifiedBy);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", userVacationDays.UserId);
            ViewData["Year"] = new SelectList(_context.VacationDays, "Year", "Year", userVacationDays.Year);
            return View(userVacationDays);
        }

        // GET: UserVacationDays/Edit/5
        public async Task<IActionResult> Edit(Guid? id, string year)
        {
            if (id == null || _context.UserVacationDays == null)
            {
                return NotFound();
            }

            if (year == null || _context.UserVacationDays == null)
            {
                return NotFound();
            }

            var userVacationDays = _context.UserVacationDays.Where(u => u.UserId.Equals(id) && u.Year.Equals(year)).FirstOrDefault();
            if (userVacationDays == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", userVacationDays.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", userVacationDays.ModifiedBy);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Email", userVacationDays.UserId);
            ViewData["Year"] = new SelectList(_context.VacationDays, "Year", "Year", userVacationDays.Year);
            return View(userVacationDays);
        }

        // POST: UserVacationDays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,Year,AcquiredDays,AdditionalDays,CreatedBy,CreationDate,ModifiedBy,ModificationDate")] UserVacationDays userVacationDays)
        {
            if (id != userVacationDays.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
				{
					var user = await _userManager.GetUserAsync(User);

					userVacationDays.ModificationDate = DateTime.Now;
					userVacationDays.ModifiedBy = user.Id;

                    var userVacationDaysInsert = new UserVacationDays();
                    userVacationDaysInsert.UserId = userVacationDays.UserId;
                    userVacationDaysInsert.Year = userVacationDays.Year;
					userVacationDaysInsert.AcquiredDays = userVacationDays.AcquiredDays;
					userVacationDaysInsert.AdditionalDays = userVacationDays.AdditionalDays;
                    userVacationDaysInsert.CreatedBy = userVacationDays.CreatedBy;
                    userVacationDaysInsert.CreationDate = userVacationDays.CreationDate;
                    userVacationDaysInsert.ModifiedBy = userVacationDays.ModifiedBy;
					userVacationDaysInsert.ModificationDate = userVacationDays.ModificationDate;

					var userVacationDaysTemp = _context.UserVacationDays.Where(u => u.Year.Equals(userVacationDays.Year)).Where(u => u.UserId.Equals(userVacationDays.UserId)).FirstOrDefault();
					
					if (userVacationDaysTemp != null)
					{
						_context.UserVacationDays.Remove(userVacationDaysTemp);
						await _context.SaveChangesAsync();
					}

					_context.Add(userVacationDays);
					await _context.SaveChangesAsync();

					TempData["toastMessage"] = "The user vacation days has been edited";
				}
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserVacationDaysExists(userVacationDays.UserId))
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
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", userVacationDays.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", userVacationDays.ModifiedBy);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", userVacationDays.UserId);
            ViewData["Year"] = new SelectList(_context.VacationDays, "Year", "Year", userVacationDays.Year);
            return View(userVacationDays);
        }

        // GET: UserVacationDays/Delete/5
        public async Task<IActionResult> Delete(Guid? id, string year)
        {
            if (id == null || _context.UserVacationDays == null)
            {
                return NotFound();
            }

            if (year == null || _context.UserVacationDays == null)
            {
                return NotFound();
            }

            var userVacationDays = _context.UserVacationDays
                .Include(u => u.CreatedByNavigation)
                .Include(u => u.ModifiedByNavigation)
                .Include(u => u.User)
                .Include(u => u.YearNavigation)
                .Where(u => u.UserId.Equals(id) && u.Year.Equals(year)).FirstOrDefault();
            if (userVacationDays == null)
            {
                return NotFound();
            }

            return View(userVacationDays);
        }

        // POST: UserVacationDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.UserVacationDays == null)
            {
                return Problem("Entity set 'DbContextBD.UserVacationDays'  is null.");
            }
            var userVacationDays = await _context.UserVacationDays.FindAsync(id);
            if (userVacationDays != null)
            {
                _context.UserVacationDays.Remove(userVacationDays);
            }
            
            await _context.SaveChangesAsync();

			TempData["toastMessage"] = "The user vacation days has been deleted";

			return RedirectToAction(nameof(Index));
        }

        private bool UserVacationDaysExists(Guid id)
        {
          return (_context.UserVacationDays?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}

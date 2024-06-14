﻿using System;
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
			Console.WriteLine("\n\n\n\n SortOrder -> " + sortOrder);
			//Ordenación
			ViewData["UserOrder"] = String.IsNullOrEmpty(sortOrder) ? "User_desc" : "";
			ViewData["YearOrder"] = sortOrder == "Year" ? "Year_desc" : "Year";
			ViewData["AcquiredDaysOrder"] = sortOrder == "AcquiredDays" ? "AcquiredDays_desc" : "AcquiredDays";
			ViewData["AdditionalDaysOrder"] = sortOrder == "AdditionalDays" ? "AdditionalDays_desc" : "AdditionalDays";

			//Cuadro de búsqueda
			ViewData["CurrentFilter"] = searchString;

			var userVacationDays = _context.UserVacationDays.Include(r => r.CreatedByNavigation).Include(r => r.ModifiedByNavigation).Include(r => r.User).AsQueryable();

			//Paginacion
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
					Console.WriteLine("1");
					userVacationDays = userVacationDays.OrderBy(r => r.User.Name);
					break;
				case "User_desc":
					Console.WriteLine("2");
					userVacationDays = userVacationDays.OrderByDescending(r => r.User.Name);
					break;
				case "Year_desc":
					Console.WriteLine("3");
					userVacationDays = userVacationDays.OrderByDescending(r => r.Year);
					break;
				case "Year":
					Console.WriteLine("4");
					userVacationDays = userVacationDays.OrderBy(r => r.Year);
					break;
				case "AcquiredDays_desc":
					Console.WriteLine("3");
					userVacationDays = userVacationDays.OrderByDescending(r => r.AcquiredDays);
					break;
				case "AcquiredDays":
					Console.WriteLine("4");
					userVacationDays = userVacationDays.OrderBy(r => r.AcquiredDays);
					break;
				case "AdditionalDays_desc":
					Console.WriteLine("3");
					userVacationDays = userVacationDays.OrderByDescending(r => r.AdditionalDays);
					break;
				case "AdditionalDays":
					Console.WriteLine("4");
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
			Console.WriteLine("entra yearSelectList");

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
            Console.WriteLine("\n\n\n\n\n\nYear 11111");
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", userVacationDays.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", userVacationDays.ModifiedBy);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", userVacationDays.UserId);
            ViewData["Year"] = new SelectList(_context.VacationDays, "Year", "Year", userVacationDays.Year);
            return View(userVacationDays);
        }

        // GET: UserVacationDays/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            Console.WriteLine("\n\nUserVacationDays entraGET");
            if (id == null || _context.UserVacationDays == null)
            {
                return NotFound();
            }

            var userVacationDays = await _context.UserVacationDays.FindAsync(id);
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
        public async Task<IActionResult> Delete(Guid? id)
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
            return RedirectToAction(nameof(Index));
        }

        private bool UserVacationDaysExists(Guid id)
        {
          return (_context.UserVacationDays?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}

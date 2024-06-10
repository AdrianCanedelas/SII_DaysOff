﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SII_DaysOff.Areas.Identity.Data;
using SII_DaysOff.Data;
using SII_DaysOff.Models;
using System.Diagnostics;

namespace SII_DaysOff.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly DbContextBD _context;
		private UserManager<ApplicationUser> _userManager;

		/*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

		public HomeController(DbContextBD context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			Console.WriteLine("\n\n Index Main");
			ViewData["notShow"] = false;
			return View();
		}

		public async Task<IActionResult> MainAsync(string sortOrder, string searchString, int? numPage, string currentFilter, string optionStatus, string year, int registerCount)
		{

			Console.WriteLine("\n\n  Main directamente");
			Console.WriteLine("\n\n\n\n\nyear --> " + year);
			if (year == null) year = DateTime.Now.Year.ToString();
			if (optionStatus != null && optionStatus != "") ViewData["Status"] = optionStatus;
			var currentOptionStatus = ViewData["status"];
			Console.WriteLine("status --> " + currentOptionStatus);
			Console.WriteLine("currentFilter --> " + currentFilter);
			Console.WriteLine("searchString --> " + searchString);
			Console.WriteLine("sortOrder --> " + sortOrder);

			// Ordenación
			ViewData["YearSelected"] = year;

			ViewData["ReasonOrder"] = String.IsNullOrEmpty(sortOrder) ? "Reason_desc" : "";
			ViewData["StartDayOrder"] = sortOrder == "StartDay" ? "StartDay_desc" : "StartDay";
			ViewData["HalfDayStartOrder"] = sortOrder == "HalfDayStart" ? "HalfDayStart_desc" : "HalfDayStart";
			ViewData["EndDayOrder"] = sortOrder == "EndDay" ? "EndDay_desc" : "EndDay";
			ViewData["HalfDayEndOrder"] = sortOrder == "HalfDayEnd" ? "HalfDayEnd_desc" : "HalfDayEnd";
			ViewData["RequestDayOrder"] = sortOrder == "RequestDay" ? "RequestDay_desc" : "RequestDay";
			ViewData["CommentsOrder"] = sortOrder == "Comments" ? "Comments_desc" : "Comments";
			ViewData["StatusOrder"] = sortOrder == "Status" ? "Status_desc" : "Status";
			Console.WriteLine("\n\n  1");
			// Cuadro de búsqueda
			//ViewData["CurrentFilter"] = searchString;

			// Cargar requests
			var user = await _userManager.Users
				.Include(u => u.UserVacationDays)
				.Include(u => u.UserVacationDays.YearNavigation)
				.FirstOrDefaultAsync(u => u.Id == Guid.Parse(_userManager.GetUserId(User)));
			ViewData["notShow"] = false;
			Console.WriteLine("\n\n  2");
			var statusId = _context.Statuses.FirstOrDefault(s => s.Name.Equals(currentOptionStatus == null ? "Pending" : currentOptionStatus))?.StatusId;
			var userId = _context.AspNetUsers.FirstOrDefault(u => u.Name.Equals(user.Name))?.Id;

			if (statusId == null || userId == null)
			{
				Console.WriteLine("\n\n  3 -- " + statusId + " - " + userId);
				return View(new MainViewModel());
			}
			Console.WriteLine("\n\n  4");
			var requests = _context.Requests
				.Include(r => r.Reason)
				.Include(r => r.Status)
				.Include(r => r.User)
				.Include(r => r.User.UserVacationDays)
				.Include(r => r.User.UserVacationDays.YearNavigation)
				.Where(r => r.StatusId == statusId)
				.Where(r => r.UserId == userId)
				.AsQueryable();
			Console.WriteLine("\n\n  5");
			if (year != null) requests = requests.Where(r => r.RequestDate.Year.ToString().Equals(year));

			ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "Name");
			ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "Name");
			ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Email");
			ViewData["YearId"] = new SelectList(_context.VacationDays, "Year", "Year");

			// Paginacion
			if (searchString != null) numPage = 1;
			else searchString = currentFilter;

			if (!String.IsNullOrEmpty(searchString))
			{
				requests = requests.Where(r => r.Reason.Name.Contains(searchString)
					|| r.StartDate.ToString().Contains(searchString)
					|| r.EndDate.ToString().Contains(searchString)
					|| r.RequestDate.ToString().Contains(searchString)
					|| r.Comments.Contains(searchString)
					|| r.Status.Name.Contains(searchString));
			}

			ViewData["CurrentOrder"] = sortOrder;
			ViewData["CurrentFilter"] = searchString;
			if (registerCount == 0) registerCount = 5;
			ViewData["RegisterCount"] = registerCount;

			switch (sortOrder)
			{
				case "":
					requests = requests.OrderBy(r => r.Reason.Name);
					break;
				case "Reason_desc":
					requests = requests.OrderByDescending(r => r.Reason.Name);
					break;
				case "StartDay_desc":
					requests = requests.OrderByDescending(r => r.StartDate);
					break;
				case "StartDay":
					requests = requests.OrderBy(r => r.StartDate);
					break;
				case "HalfDayStart":
					requests = requests.OrderBy(r => r.HalfDayStart);
					break;
				case "HalfDayStart_desc":
					requests = requests.OrderByDescending(r => r.HalfDayStart);
					break;
				case "EndDay":
					requests = requests.OrderBy(r => r.EndDate);
					break;
				case "EndDay_desc":
					requests = requests.OrderByDescending(r => r.EndDate);
					break;
				case "HalfDayEnd":
					requests = requests.OrderBy(r => r.HalfDayEnd);
					break;
				case "HalfDayEnd_desc":
					requests = requests.OrderByDescending(r => r.HalfDayEnd);
					break;
				case "RequestDay":
					requests = requests.OrderBy(r => r.RequestDate);
					break;
				case "RequestDay_desc":
					requests = requests.OrderByDescending(r => r.RequestDate);
					break;
				case "Comments":
					requests = requests.OrderBy(r => r.Comments);
					break;
				case "Comments_desc":
					requests = requests.OrderByDescending(r => r.Comments);
					break;
				case "Status":
					requests = requests.OrderBy(r => r.Status.Name);
					break;
				case "Status_desc":
					requests = requests.OrderByDescending(r => r.Status.Name);
					break;
			}

			var managerUserIds = _context.Users
				.Where(u => u.Manager == user.Id)
				.Select(u => u.Id)
				.ToList();

			var pendingRequests = _context.Requests
				.ToList()
				.Where(r => r.StatusId == (_context.Statuses.FirstOrDefault(s => s.Name.Equals("Pending"))?.StatusId))
				.Where(r => managerUserIds.Contains(r.UserId))
				.Where(r => r.RequestDate.Year.ToString().Equals(year))
				.Count();

			ViewData["PendingRequests"] = pendingRequests;

			var paginatedRequests = await PaginatedList<Requests>.CreateAsync(requests.AsNoTracking(), numPage ?? 1, registerCount);
			var logedUser = await _userManager.Users
				.Include(u => u.UserVacationDays)
				.Include(u => u.UserVacationDays.YearNavigation)
				.Where(u => u.UserVacationDays.Year == year)
				.FirstOrDefaultAsync(u => u.Id == Guid.Parse(_userManager.GetUserId(User)));

			//var adminRole = _context.Roles
			//	.FirstOrDefault(r => r.Name.Equals("Admin"));
			
			//var adminId = new Guid();
			//if (adminRole != null)
			//{
			//	adminId = adminRole.RoleId;
			//}
			//else
			//{
			//	throw new Exception("El rol de administrador no existe.");
			//}

			var viewModel = new MainViewModel
			{
				User = logedUser,
				Requests = paginatedRequests,
				TotalRequest = requests.Count(),
				PageSize = registerCount,
				//AdminId = adminId
			};

			return View(viewModel);
		}


		/*public async Task<IActionResult> MainAsync(string sortOrder, string searchString, int? numPage, string currentFilter, string optionStatus = "Pending")
		{
			ViewData["status"] = optionStatus;
			Console.WriteLine("\n\n\n\n\nstatus --> " + optionStatus);
			Console.WriteLine("currentFilter --> " + currentFilter);
			Console.WriteLine("searchString --> " + searchString);
			Console.WriteLine("sortOrder --> " + sortOrder);

			// Ordenación
			ViewData["ReasonOrder"] = String.IsNullOrEmpty(sortOrder) ? "Reason_desc" : "";
			ViewData["StartDayOrder"] = sortOrder == "StartDay" ? "StartDay_desc" : "StartDay";
			ViewData["HalfDayStartOrder"] = sortOrder == "HalfDayStart" ? "HalfDayStart_desc" : "HalfDayStart";
			ViewData["EndDayOrder"] = sortOrder == "EndDay" ? "EndDay_desc" : "EndDay";
			ViewData["HalfDayEndOrder"] = sortOrder == "HalfDayEnd" ? "HalfDayEnd_desc" : "HalfDayEnd";
			ViewData["RequestDayOrder"] = sortOrder == "RequestDay" ? "RequestDay_desc" : "RequestDay";
			ViewData["CommentsOrder"] = sortOrder == "Comments" ? "Comments_desc" : "Comments";
			ViewData["StatusOrder"] = sortOrder == "Status" ? "Status_desc" : "Status";

			// Cuadro de búsqueda
			ViewData["CurrentFilter"] = searchString;

			var user = await _userManager.GetUserAsync(User);
			ViewData["notShow"] = false;

			var statusId = _context.Statuses.FirstOrDefault(s => s.Name.Equals(optionStatus))?.StatusId;
			var userId = _context.AspNetUsers.FirstOrDefault(u => u.Name.Equals(user.Name))?.Id;

			if (statusId == null || userId == null)
			{
				return View(new List<Requests>());
			}

			var requests = _context.Requests
				.Include(r => r.Reason)
				.Include(r => r.Status)
				.Where(r => r.StatusId == statusId)
				.Where(r => r.UserId == userId)
				.AsQueryable();

			ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "Name");
			ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "Name");
			ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Email");

			if (searchString != null) numPage = 1;

			if (!String.IsNullOrEmpty(searchString))
			{
				requests = requests.Where(r => r.Reason.Name.Contains(searchString)
					|| r.StartDate.ToString().Contains(searchString)
					|| r.EndDate.ToString().Contains(searchString)
					|| r.RequestDate.ToString().Contains(searchString)
					|| r.Comments.Contains(searchString)
					|| r.Status.Name.Contains(searchString));
			}

			ViewData["CurrentOrder"] = sortOrder;
			ViewData["CurrentFilter"] = currentFilter;

			switch (sortOrder)
			{
				case "Reason_desc":
					requests = requests.OrderByDescending(r => r.Reason.Name);
					break;
				case "StartDay_desc":
					requests = requests.OrderByDescending(r => r.StartDate);
					break;
				case "StartDay":
					requests = requests.OrderBy(r => r.StartDate);
					break;
				case "HalfDayStart":
					requests = requests.OrderBy(r => r.HalfDayStart);
					break;
				case "HalfDayStart_desc":
					requests = requests.OrderByDescending(r => r.HalfDayStart);
					break;
				case "EndDay":
					requests = requests.OrderBy(r => r.EndDate);
					break;
				case "EndDay_desc":
					requests = requests.OrderByDescending(r => r.EndDate);
					break;
				case "HalfDayEnd":
					requests = requests.OrderBy(r => r.HalfDayEnd);
					break;
				case "HalfDayEnd_desc":
					requests = requests.OrderByDescending(r => r.HalfDayEnd);
					break;
				case "RequestDay":
					requests = requests.OrderBy(r => r.RequestDate);
					break;
				case "RequestDay_desc":
					requests = requests.OrderByDescending(r => r.RequestDate);
					break;
				case "Comments":
					requests = requests.OrderBy(r => r.Comments);
					break;
				case "Comments_desc":
					requests = requests.OrderByDescending(r => r.Comments);
					break;
				case "Status":
					requests = requests.OrderBy(r => r.Status.Name);
					break;
				case "Status_desc":
					requests = requests.OrderByDescending(r => r.Status.Name);
					break;
			}

			var managerUserIds = _context.Users
				.Where(u => u.Manager == user.Id)
				.Select(u => u.Id)
				.ToList();

			var pendingRequests = _context.Requests
				.ToList()
				.Where(r => r.StatusId == (_context.Statuses.FirstOrDefault(s => s.Name.Equals("Pending"))?.StatusId))
				.Where(r => managerUserIds.Contains(r.UserId))
				.Count();

			ViewData["PendingRequests"] = pendingRequests;

			int registerCount = 5;

			return View(await PaginatedList<Requests>.CreateAsync(requests.AsNoTracking(), numPage?? 1, registerCount));
		}*/

		public IActionResult Reasons()
		{
			ViewData["notShow"] = false;
			var reasons = _context.Reasons.ToList();
			return View(reasons);
		}

		public IActionResult Privacy()
		{
			ViewData["notShow"] = false;
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

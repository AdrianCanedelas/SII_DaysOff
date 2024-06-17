using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
		public readonly IHttpContextAccessor _contextAccessor;

		/*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

		public HomeController(DbContextBD context, UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor)
		{
			_context = context;
			_userManager = userManager;
			_contextAccessor = contextAccessor;
		}

		public IActionResult Index()
		{
			ViewData["notShow"] = false;
			return View();
		}

		public async Task<IActionResult> MainAsync(string sortOrder, string searchString, int? numPage, string optionStatus, string year, int registerCount)
		{
			if (optionStatus != null && optionStatus != "") ViewData["Status"] = optionStatus;
			var currentOptionStatus = ViewData["status"];

			if (_contextAccessor.HttpContext.Session.GetString("sessionYear") == null) _contextAccessor.HttpContext.Session.SetString("sessionYear", DateTime.Now.Year+"");
			if (year != null) _contextAccessor.HttpContext.Session.SetString("sessionYear", year);

			ViewData["YearSelected"] = _contextAccessor.HttpContext.Session.GetString("sessionYear");
			ViewData["ReasonOrder"] = String.IsNullOrEmpty(sortOrder) ? "Reason_desc" : "";
			ViewData["StartDayOrder"] = sortOrder == "StartDay" ? "StartDay_desc" : "StartDay";
			ViewData["HalfDayStartOrder"] = sortOrder == "HalfDayStart" ? "HalfDayStart_desc" : "HalfDayStart";
			ViewData["EndDayOrder"] = sortOrder == "EndDay" ? "EndDay_desc" : "EndDay";
			ViewData["HalfDayEndOrder"] = sortOrder == "HalfDayEnd" ? "HalfDayEnd_desc" : "HalfDayEnd";
			ViewData["RequestDayOrder"] = sortOrder == "RequestDay" ? "RequestDay_desc" : "RequestDay";
			ViewData["CommentsOrder"] = sortOrder == "Comments" ? "Comments_desc" : "Comments";
			ViewData["StatusOrder"] = sortOrder == "Status" ? "Status_desc" : "Status";

			var logedUser = await _userManager.Users
				.Include(u => u.UserVacationDays)
				.Include(u => u.UserVacationDays.YearNavigation)
				.Where(u => u.UserVacationDays.Year == _contextAccessor.HttpContext.Session.GetString("sessionYear"))
				.FirstOrDefaultAsync(u => u.Id == Guid.Parse(_userManager.GetUserId(User)));

			var user = await _userManager.Users
				.Include(u => u.UserVacationDays)
				.Include(u => u.UserVacationDays.YearNavigation)
				.FirstOrDefaultAsync(u => u.Id == Guid.Parse(_userManager.GetUserId(User)));

			ViewData["notShow"] = false;

			var statusId = _context.Statuses.FirstOrDefault(s => s.Name.Equals(currentOptionStatus == null ? "Pending" : currentOptionStatus))?.StatusId;
			var userId = _context.AspNetUsers.FirstOrDefault(u => u.Name.Equals(user.Name))?.Id;

			if (statusId == null || userId == null)
			{
				return View(new MainViewModel());
			}

			var requests = _context.Requests
				.Include(r => r.Reason)
				.Include(r => r.Status)
				.Include(r => r.User)
				.Where(r => r.StatusId == statusId)
				.Where(r => r.UserId == userId)
				.AsQueryable();

			if (_contextAccessor.HttpContext.Session.GetString("sessionYear") != null) requests = requests.Where(r => r.RequestDate.Year.ToString().Equals(_contextAccessor.HttpContext.Session.GetString("sessionYear")));

			ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "Name");
			ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "Name");
			ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Email");
			ViewData["YearId"] = new SelectList(_context.VacationDays, "Year", "Year");

			// Paginacion
			if (searchString != null && !searchString.Equals("-1"))
			{
				numPage = 1;
				_contextAccessor.HttpContext.Session.SetString("searchStringMain", searchString);
			} else if(searchString == null)
			{
				_contextAccessor.HttpContext.Session.SetString("searchStringMain", "");
			}

			if (!String.IsNullOrEmpty(_contextAccessor.HttpContext.Session.GetString("searchStringMain")))
			{
				requests = requests.Where(r => r.Reason.Name.Contains(_contextAccessor.HttpContext.Session.GetString("searchStringMain"))
					|| r.StartDate.ToString().Contains(_contextAccessor.HttpContext.Session.GetString("searchStringMain"))
					|| r.EndDate.ToString().Contains(_contextAccessor.HttpContext.Session.GetString("searchStringMain"))
					|| r.RequestDate.ToString().Contains(_contextAccessor.HttpContext.Session.GetString("searchStringMain"))
					|| r.Comments.Contains(_contextAccessor.HttpContext.Session.GetString("searchStringMain"))
					|| r.Status.Name.Contains(_contextAccessor.HttpContext.Session.GetString("searchStringMain")));
			}

			ViewData["CurrentOrder"] = sortOrder;
			ViewData["CurrentFilter"] = _contextAccessor.HttpContext.Session.GetString("searchStringMain");
			if (registerCount == 0) _contextAccessor.HttpContext.Session.SetInt32("registerCountMain", 5);
			else if(registerCount == null) _contextAccessor.HttpContext.Session.SetInt32("registerCountMain", 5);
			else if(registerCount != 11) _contextAccessor.HttpContext.Session.SetInt32("registerCountMain", registerCount);
			ViewData["RegisterCount"] = _contextAccessor.HttpContext.Session.GetInt32("registerCountMain");
			if (ViewData["RegisterCount"] == null) ViewData["RegisterCount"] = 5;

			switch (sortOrder)
			{
				case "":
					requests = requests.OrderBy(r => r.Reason);
					break;
				case "Reason_desc":
					requests = requests.OrderByDescending(r => r.Reason);
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
				.Where(r => r.RequestDate.Year.ToString().Equals(_contextAccessor.HttpContext.Session.GetString("sessionYear")))
				.Count();

			ViewData["PendingRequests"] = pendingRequests;

			var registerCountMain = 5;
			if (_contextAccessor.HttpContext.Session.GetInt32("registerCountMain") != null) registerCountMain = (int)_contextAccessor.HttpContext.Session.GetInt32("registerCountMain");
			var paginatedRequests = await PaginatedList<Requests>.CreateAsync(requests.AsNoTracking(), numPage ?? 1, registerCountMain);
			var viewModel = new MainViewModel
			{
				User = logedUser,
				Requests = paginatedRequests,
				TotalRequest = requests.Count(),
				PageSize = registerCountMain,
				Year = _contextAccessor.HttpContext.Session.GetString("sessionYear"),
				AdminId = _context.Roles.Where(r => r.Name.Equals("Admin")).Select(r => r.Id).FirstOrDefault()
			};

			return View(viewModel);
		}

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

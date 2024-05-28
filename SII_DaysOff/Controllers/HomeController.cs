using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SII_DaysOff.Areas.Identity.Data;
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

        public HomeController (DbContextBD context, UserManager<ApplicationUser> userManager)
        {
			_context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewData["notShow"] = false;
            return View();
        }
        
        public async Task<IActionResult> MainAsync(string sortOrder, string searchString, string optionStatus = "Pending")
        {
            //Ordenación
            ViewData["ReasonOrder"] = String.IsNullOrEmpty(sortOrder) ? "Reason_desc" : "";
			ViewData["StartDayOrder"] = sortOrder == "StartDay" ? "StartDay_desc" : "StartDay";
			ViewData["HalfDayStartOrder"] = sortOrder == "HalfDayStart" ? "HalfDayStart_desc" : "HalfDayStart";
			ViewData["EndDayOrder"] = sortOrder == "EndDay" ? "EndDay_desc" : "EndDay";
			ViewData["HalfDayEndOrder"] = sortOrder == "HalfDayEnd" ? "HalfDayEnd_desc" : "HalfDayEnd";
			ViewData["RequestDayOrder"] = sortOrder == "RequestDay" ? "RequestDay_desc" : "RequestDay";
			ViewData["CommentsOrder"] = sortOrder == "Comments" ? "Comments_desc" : "Comments";
			ViewData["StatusOrder"] = sortOrder == "Status" ? "Status_desc" : "Status";

			//Cuadro de búsqueda
			ViewData["CurrentFilter"] = searchString;

			var user = await _userManager.GetUserAsync(User);
            ViewData["notShow"] = false;
            var requests = _context.Requests
                .Include(r => r.Reason)
				.Include(r => r.Status)
                .ToList()
                .Where(r => r.StatusId == (_context.Statuses.FirstOrDefault(s => s.Name.Equals(optionStatus))?.StatusId))
                .Where(r => r.UserId == (_context.AspNetUsers.FirstOrDefault(u => u.Name.Equals(user.Name))?.Id));

            ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "Name");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "Name");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Email");

            switch(sortOrder)
            {
                case "Rreason_desc":
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

            /**/
			var managerUserIds = _context.Users
				.Where(u => u.Manager == user.Id)
				.Select(u => u.Id)
				.ToList();
			var pendingRequests = _context.Requests
				.ToList()
				.Where(r => r.StatusId == (_context.Statuses.FirstOrDefault(s => s.Name.Equals("Pending"))?.StatusId))
				.Where(r => managerUserIds.Contains(r.UserId)).Count();
			ViewData["PendingRequests"] = pendingRequests;

            return View(requests);
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

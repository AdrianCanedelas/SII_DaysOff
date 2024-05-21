using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SII_DaysOff.Areas.Identity.Data;
using SII_DaysOff.Models;
using System.Diagnostics;

namespace SII_DaysOff.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly DbContextBD _context;

		/*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        public HomeController (DbContextBD context)
        {
			_context = context;
		}

        public IActionResult Index()
        {
            ViewData["notShow"] = false;
            return View();
        }
        
        public IActionResult Main(string optionStatus)
        {
            Console.WriteLine("optionStatus -> " + optionStatus);
            ViewData["notShow"] = false;
            var requests = _context.Requests.ToList().Where(r => r.Status.Equals(optionStatus));
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

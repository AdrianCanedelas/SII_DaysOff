using Microsoft.AspNetCore.Mvc;
using SII_DaysOff.Models;
using System.Diagnostics;

namespace SII_DaysOff.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Privacy()
        {
            ViewData["notShow"] = false;
            return View();
        }
        
        public IActionResult Main()
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

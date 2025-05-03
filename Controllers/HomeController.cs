using Microsoft.AspNetCore.Mvc;
using ST10263027_PROG7311_POE.Models;
using System.Diagnostics;

namespace ST10263027_PROG7311_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
//***************************************************************************************************//
//Employee-specific views
        public IActionResult EmployeeLogin()
        {
            return View();
        }
        public IActionResult EmployeeDashboard()
        {
            if (HttpContext.Session.GetInt32("EmployeeId") == null)
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }
//***************************************************************************************************//

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//***********************************************End of file*****************************************//

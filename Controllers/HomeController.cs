using Microsoft.AspNetCore.Mvc;
using ST10263027_PROG7311_POE.Models;
using ST10263027_PROG7311_POE.Services;
using System.Diagnostics;

namespace ST10263027_PROG7311_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FarmerService _farmerService;

        public HomeController(ILogger<HomeController> logger, FarmerService farmerService)
        {
            _logger = logger;
            _farmerService = farmerService;
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
        //Farmer-specific views
        public IActionResult FarmerLogin()
        {
            return View();
        }

        public IActionResult FarmerDashboard()
        {
            var farmerId = HttpContext.Session.GetInt32("FarmerId");
            if (farmerId == null) return RedirectToAction("FarmerLogin");

            var username = HttpContext.Session.GetString("FarmerUsername");
            ViewBag.WelcomeMessage = $"Welcome, {username}!";

            var products = _farmerService.GetFarmerProducts(farmerId.Value);
            return View("~/Views/Home/FarmerDashboard.cshtml", products);
        }
        //***************************************************************************************************//
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
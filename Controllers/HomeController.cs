using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ST10263027_PROG7311_POE.Models;
using ST10263027_PROG7311_POE.Services;
using System.Diagnostics;

namespace ST10263027_PROG7311_POE.Controllers
{
    //Main controller for handling general application views and shared functionality
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FarmerService _farmerService;

        //Initializes controller with logger and farmer service
        public HomeController(ILogger<HomeController> logger, FarmerService farmerService)
        {
            _logger = logger;
            _farmerService = farmerService;
        }

        //Home page upon launching the application
        public IActionResult Index()
        {
            return View();
        }

        //Privacy policy page
        public IActionResult Privacy()
        {
            return View();
        }

        //***************************************************************************************************//
        // Employee-specific views

        //Employee login page view
        public IActionResult EmployeeLogin()
        {
            return View();
        }

        //Employee dashboard view
        public IActionResult EmployeeDashboard()
        {
            if (HttpContext.Session.GetInt32("EmployeeId") == null)
            {
                return RedirectToAction("Login", "Employee");
            }
            return View();
        }
        //***************************************************************************************************//
        // Farmer-specific views

        //Farmer login page view
        public IActionResult FarmerLogin()
        {
            return View();
        }

        //Farmer dashboard view
        public IActionResult FarmerDashboard()
        {
            var farmerId = HttpContext.Session.GetInt32("FarmerId");
            if (farmerId == null) return RedirectToAction("FarmerLogin");

            var username = HttpContext.Session.GetString("FarmerUsername");
            ViewBag.WelcomeMessage = $"Welcome, {username}!";//displays the user name of the farmer along with the welcome message

            var products = _farmerService.GetFarmerProducts(farmerId.Value);
            return View("~/Views/Home/FarmerDashboard.cshtml", products);
        }
        //***************************************************************************************************//

        // Error handling page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //***************************************************************************************************//

        // Logout functionality that clears the session and returns the issue to the home page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
//***********************************************End of file*****************************************//
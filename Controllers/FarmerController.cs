using Microsoft.AspNetCore.Mvc;
using ST10263027_PROG7311_POE.Models;
using ST10263027_PROG7311_POE.Services;
using ST10263027_PROG7311_POE.Repository;
using System;

namespace ST10263027_PROG7311_POE.Controllers
{
    public class FarmerController : Controller
    {
        private readonly FarmerService _farmerService;

        public FarmerController(FarmerRepository farmerRepository)
        {
            _farmerService = new FarmerService(farmerRepository);
        }

        // GET: /Farmer/Login
        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in, redirect to dashboard
            if (HttpContext.Session.GetInt32("FarmerId") != null)
            {
                return RedirectToAction("FarmerDashboard");
            }
            return View();
        }

        // POST: /Farmer/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            try
            {
                // Validate login credentials
                var farmer = _farmerService.FarmerLogin(username, password);

                // Store farmer ID in session
                HttpContext.Session.SetInt32("FarmerId", farmer.FarmerId);
                HttpContext.Session.SetString("FarmerUsername", farmer.FarmerUserName);

                // Redirect to dashboard on success
                return RedirectToAction("FarmerDashboard");
            }
            catch (ArgumentException ex)
            {
                ViewBag.Error = ex.Message;
                ViewData["Username"] = username;
                return View();
            }
            catch (UnauthorizedAccessException ex)
            {
                ViewBag.Error = ex.Message;
                ViewData["Username"] = username;
                return View();
            }
            catch (Exception)
            {
                ViewBag.Error = "An error occurred during login. Please try again.";
                return View();
            }
        }

        // GET: /Farmer/FarmerDashboard
        public IActionResult FarmerDashboard()
        {
            // Check if user is logged in
            var farmerId = HttpContext.Session.GetInt32("FarmerId");
            if (farmerId == null)
            {
                return RedirectToAction("Login");
            }

            var username = HttpContext.Session.GetString("FarmerUsername");
            ViewBag.WelcomeMessage = $"Welcome, {username}!";

            // Get list of farmer's products for the dashboard
            var products = _farmerService.GetFarmerProducts(farmerId.Value);

            // Explicitly load the view from /Views/Home/
            return View("~/Views/Home/FarmerDashboard.cshtml", products);
        }

        // POST: /Farmer/AddProduct
        //Claude AI was used to fix this method that was not properly saving uploading and saving a product to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(Product product)
        {
            var farmerId = HttpContext.Session.GetInt32("FarmerId");
            if (farmerId == null) return RedirectToAction("Login");

            try
            {
                // Set FarmerId from session
                product.FarmerId = farmerId.Value;

                // Remove FarmerId and Farmer from ModelState validation
                ModelState.Remove("FarmerId");
                ModelState.Remove("Farmer");

                if (ModelState.IsValid)
                {
                    _farmerService.AddProduct(product);
                    TempData["SuccessMessage"] = "Product added successfully!";
                    return RedirectToAction("FarmerDashboard");
                }

                // Handle validation errors
                ViewBag.ProductError = "Please correct the errors and try again.";
                return View("~/Views/Home/FarmerDashboard.cshtml", _farmerService.GetFarmerProducts(farmerId.Value));
            }
            catch (Exception ex)
            {
                ViewBag.ProductError = $"Error: {ex.Message}";
                return View("~/Views/Home/FarmerDashboard.cshtml", _farmerService.GetFarmerProducts(farmerId.Value));
            }
        }

        // GET: /Farmer/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
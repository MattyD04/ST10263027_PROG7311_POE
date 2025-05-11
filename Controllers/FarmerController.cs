using Microsoft.AspNetCore.Mvc;
using ST10263027_PROG7311_POE.Models;
using ST10263027_PROG7311_POE.Services;
using ST10263027_PROG7311_POE.Repository;
using System;

namespace ST10263027_PROG7311_POE.Controllers
{
    
    //Controller for handling farmer-related operations including login, dashboard, and product management
    
    public class FarmerController : Controller
    {
        // Service layer for farmer-related business logic
        private readonly FarmerService _farmerService;

    
        /// Constructor that initializes the FarmerService with a FarmerRepository
        public FarmerController(FarmerRepository farmerRepository)
        {
            _farmerService = new FarmerService(farmerRepository);
        }

        //***************************************************************************************//
       
        /// Displays the farmer login page and redirects to dashboard if farmer is already logged in
       
        [HttpGet]
        public IActionResult Login()
        {
            // Check if farmer is already authenticated
            if (HttpContext.Session.GetInt32("FarmerId") != null)
            {
                return RedirectToAction("FarmerDashboard");
            }
            return View();
        }
        //***************************************************************************************//

        //this method handles the login functionality for the farmer
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            try
            {
                //Authenticate farmer using service layer
                var farmer = _farmerService.FarmerLogin(username, password);

                //Store farmer authentication details in session
                HttpContext.Session.SetInt32("FarmerId", farmer.FarmerId);
                HttpContext.Session.SetString("FarmerUsername", farmer.FarmerUserName);

                //Redirect to dashboard upon successful login
                return RedirectToAction("FarmerDashboard");
            }
            catch (ArgumentException ex)
            {
                //Handle invalid input errors
                ViewBag.Error = ex.Message;//message that displays notifying the issue what the issue was
                ViewData["Username"] = username;
                return View();
            }
            catch (UnauthorizedAccessException ex)
            {
                //Handle authentication failures
                ViewBag.Error = ex.Message; //message displays indicating what the issue was
                ViewData["Username"] = username;
                return View();
            }
            catch (Exception)
            {
                //Handle unexpected errors
                ViewBag.Error = "An error occurred during login. Please try again.";
                return View();
            }
        }
        //***************************************************************************************//

        /// Displays the farmer dashboard with their products
        public IActionResult FarmerDashboard()
        {
            //Check authentication status
            var farmerId = HttpContext.Session.GetInt32("FarmerId");
            if (farmerId == null)
            {
                return RedirectToAction("Login");
            }

            //Personalize dashboard with username so the user feels like special 
            var username = HttpContext.Session.GetString("FarmerUsername");
            ViewBag.WelcomeMessage = $"Welcome, {username}!";

            //Retrieve farmer's products for displaying it in the farmer dashboard
            var products = _farmerService.GetFarmerProducts(farmerId.Value);

            //Explicitly specify the view location
            return View("~/Views/Home/FarmerDashboard.cshtml", products);
        }
        //***************************************************************************************//

        //Claude AI was used to fix issues with product saving functionality
        //this method handles the functionality of a farmer adding a product via the dashboard
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(Product product)
        {
            //Verify authentication
            var farmerId = HttpContext.Session.GetInt32("FarmerId");
            if (farmerId == null) return RedirectToAction("Login");

            try
            {
                //Associate product with the farmer that is logged in to keep the dashboard personalised
                product.FarmerId = farmerId.Value;

                //Adjust ModelState validation to exclude navigation properties
                ModelState.Remove("FarmerId");
                ModelState.Remove("Farmer");

                if (ModelState.IsValid)
                {
                    //Add product through service layer
                    _farmerService.AddProduct(product);
                    TempData["SuccessMessage"] = "Product added successfully!";//message displays if a product is added successfully
                    return RedirectToAction("FarmerDashboard");//return back to the farmer dashboard once product added successfully
                }

                //Handle validation errors
                ViewBag.ProductError = "Please correct the errors and try again.";
                return View("~/Views/Home/FarmerDashboard.cshtml", _farmerService.GetFarmerProducts(farmerId.Value));
            }
            catch (Exception ex)
            {
                //Handle unexpected errors
                ViewBag.ProductError = $"Error: {ex.Message}"; //message displays if a product is not added successfully
                return View("~/Views/Home/FarmerDashboard.cshtml", _farmerService.GetFarmerProducts(farmerId.Value));
            }
        }
    }
}
//***********************************************End of file*****************************************//
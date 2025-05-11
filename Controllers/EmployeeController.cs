using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ST10263027_PROG7311_POE.Models;
using ST10263027_PROG7311_POE.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ST10263027_PROG7311_POE.Controllers
{
    //This controller handles employee-related actions received from the service file where the business logic is
    public class EmployeeController : Controller
    {
        //Service for handling employee-related business logic
        private readonly EmployeeService _employeeService;

        //Constructor injecting EmployeeService dependency
        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        //Displays the employee login view
        public IActionResult Login()
        {
            return View("~/Views/Home/EmployeeLogin.cshtml");
        }

        //***************************************************************************************//
        //Handles POST request for employee login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            //Preserve username in case of error
            ViewData["Username"] = username;

            //Validate inputs
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Username and password are required.";
                return View("~/Views/Home/EmployeeLogin.cshtml");
            }

            try
            {
                //Attempt to login or register employee
                var employee = _employeeService.LoginOrRegisterEmployee(username, password);

                //Store employee details in session
                HttpContext.Session.SetInt32("EmployeeId", employee.EmployeeId);
                HttpContext.Session.SetString("Username", employee.userName);

                //Redirect to dashboard on success
                return RedirectToAction("EmployeeDashboard", "Home");
            }
            catch (Exception ex)
            {
                //Handle specific error cases
                if (ex.Message.Contains("email"))
                    ViewBag.UsernameError = ex.Message;
                else if (ex.Message.Contains("Password"))
                    ViewBag.PasswordError = ex.Message;
                else
                    ViewBag.Error = ex.Message;

                return View("~/Views/Home/EmployeeLogin.cshtml");
            }
        }

        //***************************************************************************************//
        // Handles POST request to add a new farmer to the localdb
        [HttpPost]
        public IActionResult AddFarmer(Farmer farmer)
        {
            //Validate model state
            if (!ModelState.IsValid)
            {
                return View("~/Views/Home/EmployeeDashboard.cshtml", farmer);
            }

            try
            {
                //Attempt to add farmer
                _employeeService.AddFarmer(farmer);
                ViewBag.Success = "Farmer added successfully!"; //message that displays if a farmer is added successfully
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message; //messages displays if a farmer could not be added successfully
            }
            return View("~/Views/Home/EmployeeDashboard.cshtml");//returns back to the employee dashboard if a farmer is not added successfully
        }

        //***************************************************************************************//

        //Handles GET request to retrieve farmers with their products, with optional filters that the employee will be able to choose from depending on what they want to filter by
        //corrections made by Claude AI as there was an issue with displaying and filtering products in the EmployeeDashboard.cshtml filr
        [HttpGet]
        public IActionResult GetFarmersWithProducts(string farmerName = null, string productCategory = null,
                                                  string fromDate = null, string toDate = null)
        {
            try
            {
                //Parse date strings to DateTime objects if provided
                DateTime? fromDateObj = !string.IsNullOrEmpty(fromDate) ? DateTime.Parse(fromDate) : null;
                DateTime? toDateObj = !string.IsNullOrEmpty(toDate) ? DateTime.Parse(toDate) : null;

                //Get filtered farmers with products from service
                var farmersWithProducts = _employeeService.GetFarmersWithProducts(farmerName, productCategory, fromDateObj, toDateObj);

                //Return JSON response with data
                return Json(new
                {
                    success = true,
                    data = farmersWithProducts,
                    count = farmersWithProducts.Count
                });
            }
            catch (Exception ex)
            {
                //Return error response if exception occurs
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
//***********************************************End of file*****************************************//
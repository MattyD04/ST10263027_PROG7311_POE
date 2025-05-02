using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ST10263027_PROG7311_POE.Models;
using ST10263027_PROG7311_POE.Services;
using System;

namespace ST10263027_PROG7311_POE.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public IActionResult Login()
        {
            return View("~/Views/Home/EmployeeLogin.cshtml");
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            ViewData["Username"] = username;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Username and password are required.";
                return View("~/Views/Home/EmployeeLogin.cshtml");
            }

            try
            {
                var employee = _employeeService.LoginOrRegisterEmployee(username, password);

                HttpContext.Session.SetInt32("EmployeeId", employee.EmployeeId);
                HttpContext.Session.SetString("Username", employee.userName);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("email"))
                    ViewBag.UsernameError = ex.Message;
                else if (ex.Message.Contains("Password"))
                    ViewBag.PasswordError = ex.Message;
                else
                    ViewBag.Error = ex.Message;

                return View("~/Views/Home/EmployeeLogin.cshtml");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Employee");
        }
    }
}

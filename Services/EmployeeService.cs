using ST10263027_PROG7311_POE.Models;
using ST10263027_PROG7311_POE.Repository;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text.RegularExpressions;

namespace ST10263027_PROG7311_POE.Services
{
    //This file contains the business logic for the employee controller and acting as an intermediary between the controller and repository
    public class EmployeeService
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeService(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        //***************************************************************************************//
        //This method handles the login or registration of an employee
        //Debugging and corrections done by Claude AI
        public Employee LoginOrRegisterEmployee(string username, string password)
        {
            //Validate username format by calling the IsValidEmail method
            if (!IsValidEmail(username))
            {
                throw new Exception("Username must be a valid email address containing '@' symbol.");//message that displays if a user enters an invalid email address without the "@" symbol"
            }

            //Validate password strength by calling the IsValidPassword method
            if (!IsPasswordValid(password))
            {
                throw new Exception("Password must contain at least 8 characters, 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.");
            } //message that displays if the password does not meet certain specifications

            var employee = _employeeRepository.GetEmployeeByUsernameAndPassword(username, password);

            if (employee == null)
            {
                if (_employeeRepository.EmployeeExists(username))
                {
                    throw new Exception("Username already exists but password doesn't match.");//if the password does not match and the username is corrrect, this message is displayed
                }

                var newEmployee = new Employee //create a new employee
                {
                    userName = username, //employee username
                    password = password //employee password
                };

                _employeeRepository.AddEmployee(newEmployee); //call the AddEmployee method from the repository to add the new employee to the database
                return newEmployee;
            }

            return employee;
        }
        //***************************************************************************************//
        //Method that has regex for ensuring the email address is valid
        //Corrections by Claude AI
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                //Basic email validation regex that checks for @ symbol and basic structure
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        
        //***************************************************************************************//
        //Method for ensuring that a farmer's contact number has 10 digits and starts with a 0
        //Corrections by Claude AI
        private bool IsValidContactNumber(string contactNumber)
        {
            if (string.IsNullOrWhiteSpace(contactNumber))
                return false;

            //Validates the phone number format to ensure it is 10 digits and starts with 0
            return Regex.IsMatch(contactNumber, @"^0[0-9]{9}$");
        }
        //***************************************************************************************//
        //This method handles the validation of a password according to specifications
        //Corrections done by Claude AI
        private bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            try
            {
                //Regex that ensures passwords has: minimum 8 characters, at least 1 uppercase letter and 1 lowercase letter, 1 number and 1 special character
                return Regex.IsMatch(password,
                    @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
                    RegexOptions.None, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        //***************************************************************************************//
        //This method handles the addition of a new farmer by an employee in the employee dashboard
        public void AddFarmer(Farmer farmer)
        {
            //Validate required fields
            if (string.IsNullOrWhiteSpace(farmer.FarmerUserName))
                throw new Exception("Farmer username is required"); //message that displays if the username is not entered

            if (string.IsNullOrWhiteSpace(farmer.FarmerPassword))
                throw new Exception("Password is required");//message that displays if the password is not entered

            //Validate username format
            if (!IsValidEmail(farmer.FarmerUserName))
                throw new Exception("Farmer username must be a valid email address"); //message that displays if the email address is not entered correctly

            // Validate password strength
            if (!IsPasswordValid(farmer.FarmerPassword))
                throw new Exception("Password must contain at least 8 characters, including 1 uppercase, 1 lowercase, 1 number, and 1 special character"); //message that displays if the password does not meet certain specifications

            // Validate contact number format
            if (!IsValidContactNumber(farmer.FarmerContactNum))
                throw new Exception("Contact number must be 10 digits and start with 0"); //message that displays if the contact number is not entered correctly

            // Check for existing farmer
            if (_employeeRepository.FarmerExists(farmer.FarmerUserName))
                throw new Exception("Farmer username already exists"); //message that displays if the username already exists

            //Add farmer to database by calling the AddFarmer method in the repository
            _employeeRepository.AddFarmer(farmer);
        }
        //***************************************************************************************//
        //This ViewModel combines the farmer fields with the product fields for displaying a farmer's details in the employee dashboard
        public class FarmerProductViewModel
        {
            public int FarmerId { get; set; } //Field for the FarmerId
            public string FarmerUserName { get; set; } //Field for a farmer's username
            public string FarmerContactNum { get; set; } //Field for a farmer's contact number
            public string ProductName { get; set; } //Field for the name of a product
            public string ProductCategory { get; set; } //Field for the category of a product
            public DateTime? ProductionDate { get; set; } //Field for the date of production for a product
        }

        //***************************************************************************************//
        //This method handles the retrieval of all farmers along with their products
        //Corrections done by Claude AI as there were issues with displaying and filtering products in the EmployeeDashboard.cshtml file
        public List<FarmerProductViewModel> GetFarmersWithProducts(string farmerName = null, string productCategory = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // Get all farmers and their products from repository
            var farmersWithProducts = _employeeRepository.GetFarmersWithProducts();

            // Create a list to store filtered results
            var filteredResults = farmersWithProducts.ToList();

            // Filter by farmer name if provided by user
            if (!string.IsNullOrWhiteSpace(farmerName))
            {
                filteredResults = filteredResults.Where(fp =>
                    fp.FarmerUserName != null &&
                    fp.FarmerUserName.IndexOf(farmerName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            //Filter by product category if provided by user 
            if (!string.IsNullOrWhiteSpace(productCategory))
            {
                filteredResults = filteredResults.Where(fp =>
                    fp.ProductCategory != null &&
                    fp.ProductCategory.Equals(productCategory, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            //Filter by date range if provided by user 
            if (fromDate.HasValue)
            {
                filteredResults = filteredResults.Where(fp =>
                    fp.ProductionDate.HasValue &&
                    fp.ProductionDate.Value.Date >= fromDate.Value.Date).ToList();
            }

            if (toDate.HasValue)
            {
                filteredResults = filteredResults.Where(fp =>
                    fp.ProductionDate.HasValue &&
                    fp.ProductionDate.Value.Date <= toDate.Value.Date).ToList();
            }

            //Handle null production dates by putting them at the end
            var orderedResults = filteredResults
                .OrderBy(fp => fp.ProductionDate.HasValue ? 0 : 1) //Items with dates come first
                .ThenBy(fp => fp.ProductionDate)
                .ThenBy(fp => fp.FarmerUserName)
                .ToList();

            return orderedResults;
        }
    }
}
//***********************************************End of file*****************************************//
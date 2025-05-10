using ST10263027_PROG7311_POE.Models;
using ST10263027_PROG7311_POE.Repository;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ST10263027_PROG7311_POE.Services
{
    public class EmployeeService
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeService(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        //***************************************************************************************//
        public Employee LoginOrRegisterEmployee(string username, string password)
        {
            // Validate username format
            if (!IsValidEmail(username))
            {
                throw new Exception("Username must be a valid email address containing '@' symbol.");
            }

            // Validate password strength
            if (!IsPasswordValid(password))
            {
                throw new Exception("Password must contain at least 8 characters, including 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.");
            }

            var employee = _employeeRepository.GetEmployeeByUsernameAndPassword(username, password);

            if (employee == null)
            {
                if (_employeeRepository.EmployeeExists(username))
                {
                    throw new Exception("Username already exists but password doesn't match.");
                }

                var newEmployee = new Employee
                {
                    userName = username,
                    password = password
                };

                _employeeRepository.AddEmployee(newEmployee);
                return newEmployee;
            }

            return employee;
        }
        //***************************************************************************************//
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
        public void AddFarmer(Farmer farmer)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(farmer.FarmerUserName))
                throw new Exception("Farmer username is required");

            if (string.IsNullOrWhiteSpace(farmer.FarmerPassword))
                throw new Exception("Password is required");

            // Validate username format
            if (!IsValidEmail(farmer.FarmerUserName))
                throw new Exception("Farmer username must be a valid email address");

            // Validate password strength
            if (!IsPasswordValid(farmer.FarmerPassword))
                throw new Exception("Password must contain at least 8 characters, including 1 uppercase, 1 lowercase, 1 number, and 1 special character");

            // Validate contact number format
            if (!IsValidContactNumber(farmer.FarmerContactNum))
                throw new Exception("Contact number must be 10 digits and start with 0");

            // Check for existing farmer
            if (_employeeRepository.FarmerExists(farmer.FarmerUserName))
                throw new Exception("Farmer username already exists");

            // Add farmer to database
            _employeeRepository.AddFarmer(farmer);
        }
        //***************************************************************************************//
        private bool IsValidContactNumber(string contactNumber)
        {
            if (string.IsNullOrWhiteSpace(contactNumber))
                return false;

            //Validates the phone number format to ensure it is 10 digits and starts with 0
            return Regex.IsMatch(contactNumber, @"^0[0-9]{9}$");
        }
        //***************************************************************************************//
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
        public class FarmerProductViewModel
        {
            public int FarmerId { get; set; }
            public string FarmerUserName { get; set; }
            public string FarmerContactNum { get; set; }
            public string ProductName { get; set; }
            public string ProductCategory { get; set; }
            public DateTime? ProductionDate { get; set; }
        }

        //***************************************************************************************//
        public List<FarmerProductViewModel> GetFarmersWithProducts(string farmerName = null, string productCategory = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // Get all farmers and their products
            var farmersWithProducts = _employeeRepository.GetFarmersWithProducts();

            // Apply filters
            var filteredResults = farmersWithProducts.AsQueryable();

            // Filter by farmer name if provided
            if (!string.IsNullOrWhiteSpace(farmerName))
            {
                filteredResults = filteredResults.Where(fp =>
                    fp.FarmerUserName.Contains(farmerName, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by product category if provided
            if (!string.IsNullOrWhiteSpace(productCategory))
            {
                filteredResults = filteredResults.Where(fp =>
                    fp.ProductCategory.Equals(productCategory, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by date range if provided
            if (fromDate.HasValue)
            {
                filteredResults = filteredResults.Where(fp => fp.ProductionDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                filteredResults = filteredResults.Where(fp => fp.ProductionDate <= toDate.Value);
            }

            return filteredResults.ToList();
        }
    }
}
//***********************************************End of file*****************************************//
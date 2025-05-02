using ST10263027_PROG7311_POE.Models;
using ST10263027_PROG7311_POE.Repository;
using System;
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
    }
}
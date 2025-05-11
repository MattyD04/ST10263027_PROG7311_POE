using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ST10263027_PROG7311_POE.Models;
using ST10263027_PROG7311_POE.Services;

namespace ST10263027_PROG7311_POE.Repository
{
    //This class handles the create, read, update and delete (CRUD) operations for the Employee table in the local database
    public class EmployeeRepository
    {
        //Connection string to connect to the database
        private readonly string _connectionString;

        //Constructor that initializes the connection string
        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        //***************************************************************************************//
        // Method to add a new employee to the database
        //Corrections and debugging done by Claude AI
        public void AddEmployee(Employee employee)
        {
            //Using statement ensures proper disposal of resources
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                //SQL query to insert employee data and return the generated ID
                command.CommandText = @"
                    INSERT INTO Employees (userName, password)
                    VALUES (@userName, @password);
                    SELECT CAST(SCOPE_IDENTITY() AS int);
                ";

                //Add parameters to prevent SQL injection (suggested by Claude AI for security purposes)
                command.Parameters.AddWithValue("@userName", employee.userName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@password", employee.password ?? (object)DBNull.Value);

                connection.Open();
                //Execute the query and set the returned ID to the employee object
                employee.EmployeeId = (int)command.ExecuteScalar();
            }
        }
        //***************************************************************************************//

        //Method to retrieve an employee by their ID in the database
        //Corrections and debugging done by Claude AI
        public Employee GetEmployeeById(int employeeId)
        {
            Employee employee = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //SQL query to select employee by ID
                string sql = "SELECT * FROM Employees WHERE EmployeeId = @EmployeeId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //If a record is found, create an Employee object
                        if (reader.Read())
                        {
                            employee = new Employee
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                userName = reader["userName"]?.ToString(),
                                password = reader["password"]?.ToString()
                            };
                        }
                    }
                }
            }

            return employee;//returns an employee object
        }
        //***************************************************************************************//

        //Method to retrieve an employee by their username
        //Corrections and debugging done by Claude AI
        public Employee GetEmployeeByUsername(string username)
        {
            Employee employee = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Employees WHERE userName = @userName"; //SQL query to select employee by username

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@userName", username ?? (object)DBNull.Value);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new Employee
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                userName = reader["userName"]?.ToString(),
                                password = reader["password"]?.ToString()
                            };
                        }
                    }
                }
            }

            return employee;
        }
        //***************************************************************************************//

        // Method to retrieve an employee by both username and password (for login)
        //Corrections and debugging done by Claude AI
        public Employee GetEmployeeByUsernameAndPassword(string username, string password)
        {
            Employee employee = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Employees WHERE userName = @userName AND password = @password"; //SQL query to select employee by username and password

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@userName", username ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@password", password ?? (object)DBNull.Value);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new Employee
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                userName = reader["userName"]?.ToString(),
                                password = reader["password"]?.ToString()
                            };
                        }
                    }
                }
            }

            return employee;
        }
        //***************************************************************************************//

        //Method to check if an employee with the given username already exists in the database (for when an employee is logging in again)
        //Corrections and debugging done by Claude AI
        public bool EmployeeExists(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(1) FROM Employees WHERE userName = @userName"; //SQL query to check if employee exists by username

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@userName", username ?? (object)DBNull.Value);
                    connection.Open();
                    
                    return (int)command.ExecuteScalar() > 0; //Returns true if count is greater than 0 (employee exists)
                }
            }
        }
        //***************************************************************************************//

        // The below methods had to be placed here as there were issues when trying to place them in the FarmerRepository file

        // Method to check if a farmer with the given username already exists
        //Corrections and debugging done by Claude AI
        public bool FarmerExists(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(1) FROM Farmers WHERE FarmerUserName = @FarmerUserName"; 

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FarmerUserName", username ?? (object)DBNull.Value);
                    connection.Open();
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }

        // Method to add a new farmer to the database
        //Corrections and debugging done by Claude AI
        public void AddFarmer(Farmer farmer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
            INSERT INTO Farmers (FarmerUserName, FarmerPassword, FarmerContactNum)
            VALUES (@UserName, @Password, @ContactNum);
            SELECT CAST(SCOPE_IDENTITY() AS int);"; //SQL query to insert a new farmer and return the auto-generated FarmerId

                command.Parameters.AddWithValue("@UserName", farmer.FarmerUserName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Password", farmer.FarmerPassword ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ContactNum", farmer.FarmerContactNum ?? (object)DBNull.Value);

                connection.Open();
                farmer.FarmerId = (int)command.ExecuteScalar();
            }
        }
        //***************************************************************************************//

        //Method to retrieve all farmers along with their products for the ability for employees to see farmers with the related products
        //Debugging and corrections done by Claude AI
        public List<EmployeeService.FarmerProductViewModel> GetFarmersWithProducts()
        {
            var farmersWithProducts = new List<EmployeeService.FarmerProductViewModel>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    //SQL query to join Farmers and Products tables in the db
                    string query = @"
                SELECT f.FarmerId, f.FarmerUserName, f.FarmerContactNum, 
                       p.ProductName, p.ProductCategory, p.ProductionDate
                FROM Farmers f
                LEFT JOIN Products p ON f.FarmerId = p.FarmerId
                ORDER BY f.FarmerUserName, p.ProductionDate DESC";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create view model for each record
                                var viewModel = new EmployeeService.FarmerProductViewModel
                                {
                                    FarmerId = Convert.ToInt32(reader["FarmerId"]),
                                    FarmerUserName = reader["FarmerUserName"].ToString(),
                                    FarmerContactNum = reader["FarmerContactNum"].ToString(),
                                    ProductName = reader["ProductName"] != DBNull.Value ? reader["ProductName"].ToString() : "No products",
                                    ProductCategory = reader["ProductCategory"] != DBNull.Value ? reader["ProductCategory"].ToString() : "N/A",
                                    ProductionDate = reader["ProductionDate"] != DBNull.Value ?
                                        Convert.ToDateTime(reader["ProductionDate"]) : (DateTime?)null
                                };

                                farmersWithProducts.Add(viewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Log exception details here and display error
                throw new Exception("Error retrieving farmers and products: " + ex.Message);
            }

            return farmersWithProducts;
        }
    }
}
//***********************************************End of file*****************************************//
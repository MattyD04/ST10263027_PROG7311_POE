using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ST10263027_PROG7311_POE.Models;
using ST10263027_PROG7311_POE.Services;

namespace ST10263027_PROG7311_POE.Repository
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    INSERT INTO Employees (userName, password)
                    VALUES (@userName, @password);
                    SELECT CAST(SCOPE_IDENTITY() AS int);
                ";

                command.Parameters.AddWithValue("@userName", employee.userName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@password", employee.password ?? (object)DBNull.Value);

                connection.Open();
                employee.EmployeeId = (int)command.ExecuteScalar();
            }
        }
        //***************************************************************************************//
        public Employee GetEmployeeById(int employeeId)
        {
            Employee employee = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Employees WHERE EmployeeId = @EmployeeId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
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
        public Employee GetEmployeeByUsername(string username)
        {
            Employee employee = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Employees WHERE userName = @userName";

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
        public Employee GetEmployeeByUsernameAndPassword(string username, string password)
        {
            Employee employee = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Employees WHERE userName = @userName AND password = @password";

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
        public bool EmployeeExists(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(1) FROM Employees WHERE userName = @userName";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@userName", username ?? (object)DBNull.Value);
                    connection.Open();
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
        //***************************************************************************************//
        //The below methods had to be placed here as there were issues when trying to place them in the FarmerRepository file
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

        public void AddFarmer(Farmer farmer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
            INSERT INTO Farmers (FarmerUserName, FarmerPassword, FarmerContactNum)
            VALUES (@UserName, @Password, @ContactNum);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

                command.Parameters.AddWithValue("@UserName", farmer.FarmerUserName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Password", farmer.FarmerPassword ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ContactNum", farmer.FarmerContactNum ?? (object)DBNull.Value);

                connection.Open();
                farmer.FarmerId = (int)command.ExecuteScalar();
            }
        }
        // Add this method to your EmployeeRepository class

        public List<EmployeeService.FarmerProductViewModel> GetFarmersWithProducts()
        {
            var farmersWithProducts = new List<EmployeeService.FarmerProductViewModel>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

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
                // Log exception details here
                throw new Exception("Error retrieving farmers and products: " + ex.Message);
            }

            return farmersWithProducts;
        }
    }
}
//***********************************************End of file*****************************************//

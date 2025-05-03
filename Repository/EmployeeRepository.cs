using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ST10263027_PROG7311_POE.Models;

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
    }
}
//***********************************************End of file*****************************************//

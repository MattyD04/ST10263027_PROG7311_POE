using System;
using System.Data;
using System.Data.SqlClient;
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
            {
                string sql = @"
                    INSERT INTO Employees (EmployeeId, userName, password)
                    VALUES (@EmployeeId, @userName, @password)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    command.Parameters.AddWithValue("@userName", employee.userName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@password", employee.password ?? (object)DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
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
                                userName = reader["userName"] == DBNull.Value ? null : reader["userName"].ToString(),
                                password = reader["password"] == DBNull.Value ? null : reader["password"].ToString()
                            };
                        }
                    }
                }
            }

            return employee;
        }

        
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Employees";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                userName = reader["userName"] == DBNull.Value ? null : reader["userName"].ToString(),
                                password = reader["password"] == DBNull.Value ? null : reader["password"].ToString()
                            });
                        }
                    }
                }
            }

            return employees;
        }

       
        public void UpdateEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                    UPDATE Employees
                    SET userName = @userName,
                        password = @password
                    WHERE EmployeeId = @EmployeeId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@userName", employee.userName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@password", employee.password ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Employee not found.");
                    }
                }
            }
        }

        
        public void DeleteEmployee(int employeeId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Employee not found.");
                    }
                }
            }
        }
    }
}
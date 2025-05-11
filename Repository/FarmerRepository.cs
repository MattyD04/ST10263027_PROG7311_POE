using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ST10263027_PROG7311_POE.Models;

namespace ST10263027_PROG7311_POE.Repository
{
    //This file handles all the create, read, update, delete (CRUD) operations for the managing a farmer and products
    public class FarmerRepository
    {
        private readonly string _connectionString;

        public FarmerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        //***************************************************************************************//
        //This method is used to add a new farmer to the database
        public void AddFarmer(Farmer farmer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    INSERT INTO Farmers (FarmerUserName, FarmerPassword, FarmerContactNum)
                    VALUES (@FarmerUserName, @FarmerPassword, @FarmerContactNum);
                    SELECT CAST(SCOPE_IDENTITY() AS int); 
                "; //This line retrieves the ID of the newly inserted farmer

                command.Parameters.AddWithValue("@FarmerUserName", farmer.FarmerUserName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FarmerPassword", farmer.FarmerPassword ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FarmerContactNum", farmer.FarmerContactNum ?? (object)DBNull.Value);

                connection.Open();
                farmer.FarmerId = (int)command.ExecuteScalar();
            }
        }
        //***************************************************************************************//
        //This method handles the retrieval of a farmer from the database using FarmerId in the farmer table
        //Debugging and corrections done by DeepSeek AI
        public Farmer GetFarmerById(int farmerId)
        {
            Farmer farmer = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Farmers WHERE FarmerId = @FarmerId"; // SQL query to select a farmer by ID

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FarmerId", farmerId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            farmer = new Farmer
                            {
                                FarmerId = Convert.ToInt32(reader["FarmerId"]),
                                FarmerUserName = reader["FarmerUserName"]?.ToString(),
                                FarmerPassword = reader["FarmerPassword"]?.ToString(),
                                FarmerContactNum = reader["FarmerContactNum"]?.ToString()
                            };
                        }
                    }
                }
            }

            return farmer;
        }
        //***************************************************************************************//
        //Method for retrieving a farmer from the database using their username
        public Farmer GetFarmerByUsername(string username)
        {
            Farmer farmer = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Farmers WHERE FarmerUserName = @FarmerUserName"; // SQL query for retrieving a farmer by username

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FarmerUserName", username ?? (object)DBNull.Value);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            farmer = new Farmer
                            {
                                FarmerId = Convert.ToInt32(reader["FarmerId"]),
                                FarmerUserName = reader["FarmerUserName"]?.ToString(),
                                FarmerPassword = reader["FarmerPassword"]?.ToString(),
                                FarmerContactNum = reader["FarmerContactNum"]?.ToString()
                            };
                        }
                    }
                }
            }

            return farmer;
        }
        //***************************************************************************************//
        //Method for reading the password and username of a farmer from the database when they login
        //Debugging and corrections done by DeepSeek AI
        public Farmer GetFarmerByUsernameAndPassword(string username, string password)
        {
            Farmer farmer = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Farmers WHERE FarmerUserName = @FarmerUserName AND FarmerPassword = @FarmerPassword"; // SQL query to select a farmer by username and password

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FarmerUserName", username ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FarmerPassword", password ?? (object)DBNull.Value);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            farmer = new Farmer
                            {
                                FarmerId = Convert.ToInt32(reader["FarmerId"]),
                                FarmerUserName = reader["FarmerUserName"]?.ToString(),
                                FarmerPassword = reader["FarmerPassword"]?.ToString(),
                                FarmerContactNum = reader["FarmerContactNum"]?.ToString()
                            };
                        }
                    }
                }
            }

            return farmer;
        }
        //***************************************************************************************//
        //Method for checking a farmer exists from the database using their username
        public bool FarmerExists(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(1) FROM Farmers WHERE FarmerUserName = @FarmerUserName"; // SQL query to check if a farmer exists by username

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FarmerUserName", username ?? (object)DBNull.Value);
                    connection.Open();
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
        //***************************************************************************************//
        //Method for updating a farmer's details in the database
        public void UpdateFarmer(Farmer farmer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                    UPDATE Farmers 
                    SET FarmerUserName = @FarmerUserName, 
                        FarmerPassword = @FarmerPassword, 
                        FarmerContactNum = @FarmerContactNum 
                    WHERE FarmerId = @FarmerId"; // SQL query to update a farmer's details

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FarmerId", farmer.FarmerId);
                    command.Parameters.AddWithValue("@FarmerUserName", farmer.FarmerUserName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FarmerPassword", farmer.FarmerPassword ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FarmerContactNum", farmer.FarmerContactNum ?? (object)DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        //***************************************************************************************//
        //Deleting a farmer from the database using the FarmerId
        public void DeleteFarmer(int farmerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Farmers WHERE FarmerId = @FarmerId"; // SQL query to delete a farmer by ID

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FarmerId", farmerId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        //***************************************************************************************//
        //Method to retrieve all the farmers from the database along with their related information such as contact number
        //Debugging and corrections done by DeepSeek AI
        public List<Farmer> GetAllFarmers()
        {
            List<Farmer> farmers = new List<Farmer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Farmers"; // SQL query to select all farmers

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Farmer farmer = new Farmer
                            {
                                FarmerId = Convert.ToInt32(reader["FarmerId"]),
                                FarmerUserName = reader["FarmerUserName"]?.ToString(),
                                FarmerPassword = reader["FarmerPassword"]?.ToString(),
                                FarmerContactNum = reader["FarmerContactNum"]?.ToString()
                            };

                            farmers.Add(farmer);
                        }
                    }
                }
            }

            return farmers;
        }
        //***************************************************************************************//
        //The below methods are used to manage the products of a farmer 

        //This method handles the addition of a new product to the database submitted by a farmer
        //Debugging and corrections done by DeepSeek AI
        public void AddProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    INSERT INTO Products (ProductName, ProductCategory, ProductionDate, FarmerId)
                    VALUES (@ProductName, @ProductCategory, @ProductionDate, @FarmerId);
                    SELECT CAST(SCOPE_IDENTITY() AS int);
                ";  //SQL query to add a new product and retrieve its ID

                command.Parameters.AddWithValue("@ProductName", product.ProductName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ProductCategory", product.ProductCategory ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ProductionDate", product.ProductionDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FarmerId", product.FarmerId ?? (object)DBNull.Value);

                connection.Open();
                product.ProductId = (int)command.ExecuteScalar();
            }
        }

        //This method retrieves all products from the database that are linked to a specific farmer so they only see their products
        //Debugging and corrections done by DeepSeek AI
        public List<Product> GetProductsByFarmerId(int farmerId)
        {
            var products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                    SELECT ProductId, ProductName, ProductCategory, ProductionDate, FarmerId 
                    FROM Products 
                    WHERE FarmerId = @FarmerId
                    ORDER BY ProductionDate DESC"; // SQL query to select products by FarmerId

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FarmerId", farmerId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductName = reader["ProductName"]?.ToString(),
                                ProductCategory = reader["ProductCategory"]?.ToString(),
                                ProductionDate = reader["ProductionDate"] as DateTime?,
                                FarmerId = reader["FarmerId"] as int?
                            });
                        }
                    }
                }
            }

            return products;
        }
    }
}
//***********************************************End of file*****************************************//
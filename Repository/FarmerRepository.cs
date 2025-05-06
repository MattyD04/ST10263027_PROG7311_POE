using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ST10263027_PROG7311_POE.Models;

namespace ST10263027_PROG7311_POE.Repository
{
    public class FarmerRepository
    {
        private readonly string _connectionString;

        public FarmerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE - Add a new farmer to the database
        public void AddFarmer(Farmer farmer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    INSERT INTO Farmers (FarmerUserName, FarmerPassword, FarmerContactNum)
                    VALUES (@FarmerUserName, @FarmerPassword, @FarmerContactNum);
                    SELECT CAST(SCOPE_IDENTITY() AS int);
                ";

                command.Parameters.AddWithValue("@FarmerUserName", farmer.FarmerUserName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FarmerPassword", farmer.FarmerPassword ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FarmerContactNum", farmer.FarmerContactNum ?? (object)DBNull.Value);

                connection.Open();
                farmer.FarmerId = (int)command.ExecuteScalar();
            }
        }

        // READ - Get a farmer by ID
        public Farmer GetFarmerById(int farmerId)
        {
            Farmer farmer = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Farmers WHERE FarmerId = @FarmerId";

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

        // READ - Get a farmer by username
        public Farmer GetFarmerByUsername(string username)
        {
            Farmer farmer = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Farmers WHERE FarmerUserName = @FarmerUserName";

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

        // READ - Get a farmer by username and password (for authentication)
        public Farmer GetFarmerByUsernameAndPassword(string username, string password)
        {
            Farmer farmer = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Farmers WHERE FarmerUserName = @FarmerUserName AND FarmerPassword = @FarmerPassword";

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

        // READ - Check if a farmer exists by username
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

        // UPDATE - Update an existing farmer's information
        public void UpdateFarmer(Farmer farmer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                    UPDATE Farmers 
                    SET FarmerUserName = @FarmerUserName, 
                        FarmerPassword = @FarmerPassword, 
                        FarmerContactNum = @FarmerContactNum 
                    WHERE FarmerId = @FarmerId";

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

        // DELETE - Delete a farmer by ID
        public void DeleteFarmer(int farmerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Farmers WHERE FarmerId = @FarmerId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FarmerId", farmerId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        // READ - Get all farmers
        public List<Farmer> GetAllFarmers()
        {
            List<Farmer> farmers = new List<Farmer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Farmers";

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
    }


}
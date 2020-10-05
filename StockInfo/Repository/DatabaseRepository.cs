﻿using StockInfo.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using StockInfo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;

namespace StockInfo.Repository
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly MainDbContext mainDbContext;
        //private readonly IHubContext<CustomersHub> signalContext;
        string connectionString = "";


        public DatabaseRepository(MainDbContext mainDbContext, IConfiguration configuration)
        {
            this.mainDbContext = mainDbContext;
            connectionString = configuration.GetConnectionString("DefaultConnection");

        }


        public List<Stock> GetSearchResults(string name)
        {
            //This method will display all results within the SQL table that corelate with the given name variable/
            //If there is no name give, then by default all results within the Stocks table will be displayed.


            //SQL command text to selects all fields from Stocks table where the name paramater has a close
            //match to any of the column fields.


            string commandtext = "SELECT * FROM Stock WHERE ItemID LIKE @param OR Name LIKE @param OR Description LIKE @param";

            //Use %  on each side of variable query to work with SQL LIKE command.

            name = "%" + name + "%";



            //initialise new list instance of Stock model. 

            var results = new List<Stock>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                //Establish SQL connection and dependencies.

                conn.Open();

                SqlDependency.Start(connectionString);

                SqlCommand cmd = new SqlCommand(commandtext, conn);
                SqlDependency dependency = new SqlDependency(cmd);

                //Add the name variable to the commandText query.

                cmd.Parameters.AddWithValue("@param", name);

                //Executre reader
                var reader = cmd.ExecuteReader();

                //Read all rows until null
                while (reader.Read())
                {

                    var result = new Stock
                    {
                        //Assign class objects with the fields in search results table.

                        ItemId = (int)reader["ItemId"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString()
                    };

                    //Add class objects to the final search results list.

                    results.Add(result);
                }
            }
            //return search results

            return results;
        }

       

        public Stock GetStockInfo(int itemId)
        {
            string commandtext = string.Format("SELECT * FROM Stock where ItemId = {0} ", itemId);

            var result = new Stock();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //establish Sql Connection 
                conn.Open();

                SqlDependency.Start(connectionString);

                SqlCommand cmd = new SqlCommand(commandtext, conn);
                SqlDependency dependency = new SqlDependency(cmd);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result = new Stock
                    {
                        ItemId = (int)reader["ItemId"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString()
                    };

                    return result;
                }
            }

            return result;
        }

        public void DeleteStock(int id)
        {
            string commandText = "DELETE FROM Stock WHERE ItemId = @id";


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlDependency.Start(connectionString);
                SqlCommand cmd = new SqlCommand(commandText, conn);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteNonQuery();
            }

        }

        public void UpdateStockInfo(int id, string name, string description)
        {
            string commandText = "UPDATE Stock SET Name = @name ,  Description = @description WHERE ItemId = @itemId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlDependency.Start(connectionString);
                SqlCommand cmd = new SqlCommand(commandText, conn);
                cmd.Parameters.AddWithValue("@itemId", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description);

                var reader = cmd.ExecuteNonQuery();
            }
        }

        public void AddStock(string name, string description)
        {
            string commandText = "INSERT INTO Stock VALUES (@name, @description)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlDependency.Start(connectionString);
                SqlCommand cmd = new SqlCommand(commandText, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description);

                var reader = cmd.ExecuteNonQuery();
            }



          
        }

       


        public List<Stock> GetStockInfo()
        {
            string commandtext = "SELECT * FROM Stock";  // Can Please change the table name ok
            var results = new List<Stock>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //establish Sql Connection 
                conn.Open();

                SqlDependency.Start(connectionString);

                SqlCommand cmd = new SqlCommand(commandtext, conn);
                SqlDependency dependency = new SqlDependency(cmd);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var result = new Stock
                    {
                        ItemId = (int)reader["ItemId"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString()
                    };

                    results.Add(result);
                }
            }
            return results;
        }


    }
}
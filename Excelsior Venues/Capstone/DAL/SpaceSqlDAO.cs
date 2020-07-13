using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Capstone.DAL
{
    public class SpaceSqlDAO
    {
        private string connectionString;
        public SpaceSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Space> GetSpecificVenueSpace(int userInput)
        {
            List<Space> SpaceList = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql_inert = "SELECT * FROM space WHERE venue_id = @userInput";
                                        
                    SqlCommand cmd = new SqlCommand(sql_inert, conn);
                    cmd.Parameters.AddWithValue("@userInput", userInput);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = new Space();
                        space = SpaceAttributes(reader);
                        SpaceList.Add(space);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.Message);
            }
            return SpaceList;
        }



        private Space SpaceAttributes(SqlDataReader reader)
        {
            Space space = new Space();


            space.Id = Convert.ToInt32(reader["id"]);
            space.VenueId = Convert.ToInt32(reader["venue_id"]);
            space.Name = Convert.ToString(reader["name"]);
            if (DBNull.Value.Equals(reader["open_from"]))
            {
                space.OpenFrom = 1;
                space.OpenTo = 12;

            }
            else
            {
                space.OpenFrom = Convert.ToInt32(reader["open_from"]);
                space.OpenTo = Convert.ToInt32(reader["open_to"]);
            }
            space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
            space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);

            return space;
        }   
    }
}

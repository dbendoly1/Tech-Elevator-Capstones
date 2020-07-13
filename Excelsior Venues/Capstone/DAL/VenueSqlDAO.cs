using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Capstone.DAL
{
    public class VenueSqlDAO
    {
        private string connectionString;
        public VenueSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Venue> ListVenues()
        {

            List<Venue> venueList = new List<Venue>();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql_insert = "SELECT *, c.name AS 'Cname' FROM venue v" +
                                        "\nJOIN city c ON v.city_id = c.id" +
                                        "\nORDER BY v.name";
                                        
                    
                                      
                    SqlCommand cmd = new SqlCommand(sql_insert, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        
                        Venue venue = new Venue();
                        venue = VenueAttributes(reader);
                        
                        venueList.Add(venue);
                        
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.Message);
            }
            return venueList;
        }



        private Venue VenueAttributes(SqlDataReader reader)
        {
            Venue venue = new Venue();
            

            venue.Id = Convert.ToInt32(reader["id"]);
            venue.Name = Convert.ToString(reader["name"]);
            venue.CityId = Convert.ToInt32(reader["city_id"]);
            venue.Description = Convert.ToString(reader["description"]);
            venue.CityId2 = Convert.ToInt32(reader["id"]);
            venue.CityName = Convert.ToString(reader["Cname"]);
            venue.StateAbbreviation = Convert.ToString(reader["state_abbreviation"]);
          

            return venue;

        }
    }
}


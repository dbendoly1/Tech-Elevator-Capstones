using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO
    {
        
        private string connectionString;
        public ReservationSqlDAO(string connectionString)
        {
                this.connectionString = connectionString;
        }


        public List<Reservation> ListSpacesNotReserved(DateTime startDate, DateTime endDate, int venueSelection)
        {
                List<Reservation> reservationList = new List<Reservation>();

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                    string sql_inert = "SELECT * FROM space s" +
                                        "\nWHERE venue_id = @venueSelection" +
                                       "\nAND s.id NOT IN(SELECT s.id from reservation r" +
                                       "\nJOIN space s on r.space_id = s.id" +
                                       "\nWHERE s.venue_id = @venueSelection" +
                                       "\nAND r.end_date >= @startDate AND r.start_date <= @endDate)";
  
                    //"\nWHERE((Start_date BETWEEN @startDate AND @endDate) OR (end_date BETWEEN @startDate AND @endDate) OR (@startDate BETWEEN Start_date AND end_date) OR (@endDate BETWEEN Start_date AND end_date))";
                   
                    SqlCommand cmd = new SqlCommand(sql_inert, conn);
                        cmd.Parameters.AddWithValue("@startDate", startDate);
                        cmd.Parameters.AddWithValue("@endDate", endDate);
                        cmd.Parameters.AddWithValue("@venueSelection", venueSelection);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Reservation reservation = new Reservation();
                            reservation = ReservationAttributes(reader);
                            reservationList.Add(reservation);

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error");
                    Console.WriteLine(ex.Message);
                }
            
                return reservationList;
        }
    

        public Reservation InsertReservation(int spaceID, string attendees, DateTime startDate, DateTime endDate, string reservedFor)
        {
            Reservation NewRes = new Reservation();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    string sql_inert = "INSERT INTO reservation (space_id, number_of_attendees, start_date, end_date, reserved_for) VALUES(@spaceID, @attendees, @startDate, @endDate, @reservedFor)";
                    
                    SqlCommand cmd = new SqlCommand(sql_inert, conn);
                    cmd.Parameters.AddWithValue("@spaceID", spaceID);
                    cmd.Parameters.AddWithValue("@attendees", attendees);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    cmd.Parameters.AddWithValue("@reservedFor", reservedFor);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.Message);
            }
            return NewRes;
        }

        //****BONUS****
        public List<Reservation> ListSpacesNotReservedForAllVenues(DateTime startDate, DateTime endDate)
        {
            List<Reservation> reservationList = new List<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql_inert = "SELECT * FROM space s" +

                                       "\nWHERE s.id NOT IN(SELECT s.id from reservation r" +
                                       "\nJOIN space s on r.space_id = s.id" +
                                       "\nAND r.end_date >= @startDate AND r.start_date <= @endDate)";

                    

                    SqlCommand cmd = new SqlCommand(sql_inert, conn);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation();
                        reservation = ReservationAttributes(reader);
                        reservationList.Add(reservation);

                    }
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.Message);
            }

            return reservationList;
        }

        //Bonus
        public List<Reservation> ListResevations()
        {
            List<Reservation> reservationList = new List<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql_inert = "SELECT *, v.name AS 'Vname' FROM reservation r" +
                                       "\nJOIN space s ON r.space_id = s.id" +
                                       "\nJOIN venue v ON s.venue_id = v.id";

                                  

                    

                    SqlCommand cmd = new SqlCommand(sql_inert, conn);
                  

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation();
                        reservation = ReservationAttributesBonus(reader);
                        reservationList.Add(reservation);

                    }
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.Message);
            }

            return reservationList;
        }
        //Bonus
        private Reservation ReservationAttributes(SqlDataReader reader)
        {
                    Reservation reservation = new Reservation();

                    reservation.SpaceId = Convert.ToInt32(reader["id"]);
                    reservation.Name = Convert.ToString(reader["name"]);
                    reservation.IsAccessible = Convert.ToInt32(reader["is_accessible"]);
                        if (DBNull.Value.Equals(reader["open_from"]))
                        {
                            reservation.OpenFrom = 1;
                            reservation.OpenTo = 12;

                        }
                        else
                        {
                            reservation.OpenFrom = Convert.ToInt32(reader["open_from"]);
                            reservation.OpenTo = Convert.ToInt32(reader["open_to"]);
                        }
                    reservation.DailyRate = Convert.ToInt32(reader["daily_rate"]);
                    reservation.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            
                    return reservation;
        }

        //Bonus
        private Reservation ReservationAttributesBonus(SqlDataReader reader)
        {
            Reservation reservation = new Reservation();

            reservation.StartDate = Convert.ToDateTime(reader["start_date"]);
            reservation.EndDate = Convert.ToDateTime(reader["end_date"]);
            reservation.ReservedFor = Convert.ToString(reader["reserved_for"]);
            reservation.SpaceName = Convert.ToString(reader["name"]);
            reservation.VenueName = Convert.ToString(reader["Vname"]);


            return reservation;
        }
    }
}

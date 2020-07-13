using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Capstone
{
    public class UserInterface
    {
      
        private string connectionString;
        bool done = false;
        public int venueSelection;
        public DateTime startDate;
        public DateTime endDate;
        public int inputDays;
        public int spaceInt;
        public int inputAttendees;
        public string reservationName;

        

        private VenueSqlDAO venueDAO;
        private SpaceSqlDAO spaceDAO;
        private CategorySqlDAO categoryDAO;
        private ReservationSqlDAO reservationDAO;

              
        string reservedFor;
        string attendees;
        decimal dailyRate;
        int confirmation;
        int days;

        


        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueSqlDAO(connectionString);
            spaceDAO = new SpaceSqlDAO(connectionString);
            categoryDAO = new CategorySqlDAO(connectionString);
            reservationDAO = new ReservationSqlDAO(connectionString);
        }

        public void Run()
        {
            Console.WriteLine("~ Excelsior Venues ~");
            while (!done)
            {
                MainMenu();
            }
        }

        public void MainMenu()
        {
            Console.WriteLine("\nMain Menu" +
                              "\nWhat would you like to do?" +
                              "\n(1) List Venues" +
                              "\n(S) Search for a Space" +
                              "\n(D) Display Reservations" +
                              "\n(Q) Quit");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int result) == false && (input.ToLower() == "s" || input.ToLower() == "d" || input.ToLower() == "q"))
            {
                switch (input)
                {
                    case "s":
                        SearchForASpace();
                        break;
                    case "d":
                        DisplayReservations();
                        break;
                    case "q":
                        done = true;
                        break;
                }
            }
            else if (int.Parse(input) == 1)
            {
                VenueSelector();
            }
            else
            {
            Console.WriteLine("\n***INVALID ENTRY. TRY AGAIN***\n");
                MainMenu();
            }
        }

        public void VenueSelector()
        {
            List<Venue> venues = venueDAO.ListVenues();

            DisplayVenues();

            Console.WriteLine( "R) Return to Previous Screen\n" +
                                "\nWhich venue would you like to view?");

            string selection = Console.ReadLine();
           
            if (selection.ToLower() == "r")
            {
                MainMenu();
            }
            else if(int.TryParse(selection, out int result) == true && int.Parse(selection) <= venues.Count() && int.Parse(selection) > 0)
            {
                venueSelection = int.Parse(selection);
                
            }
            else
            {
                Console.WriteLine("\n***INVALID ENTRY. TRY AGAIN***\n");
                VenueSelector();
            }
            VenueMenu();
        }

        public void VenueMenu()
        {
            DisplayVenueDetails(venueSelection);

            Console.WriteLine("\n(1) View Spaces" +                             
                              "\n(2) Search for Reservations" +
                              "\n(R) Return to Previous Screen");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int intValue) == false && input.ToLower() == "r")
            {
                VenueSelector();
            }
            else if (int.TryParse(input, out int Value) == true && (intValue == 1 || intValue == 2))
            {
                switch (intValue)
                {
                    case 1:
                        SpaceMenu();
                        break;
                    case 2:
                        ReserveSpace();
                        break;
                }
            }
            else
            {
                Console.WriteLine("\n***INVALID ENTRY. TRY AGAIN***\n");
                VenueMenu();
            }
        }

        public void SpaceMenu()
        {
            Console.WriteLine(venueSelection + "\n");

            DisplaySpaces(venueSelection);

            Console.WriteLine("\nWhat would you like to do next?" +
                              "\n(1) Reserve a Space" +
                              "\n(R) Return to previous section");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int IntValue) == false && input.ToLower() == "r")
            {
                VenueMenu();
            }
            else if (int.TryParse(input, out int Value) == true && int.Parse(input) == 1)
            {
                ReserveSpace();
            }
            else
            {
                Console.WriteLine("\n***INVALID ENTRY. TRY AGAIN***\n");
                SpaceMenu();
            }

        }

        public void ReserveSpace()
        {
            Console.Write("When do you need the space? (MM/DD/YYYY)  ");
              string  istartDate = Console.ReadLine();
            if (DateTime.TryParse(istartDate, out DateTime RstartDate) == true)
            {
                startDate = Convert.ToDateTime(istartDate);
            }
            else
            {
                Console.WriteLine("Error, Try again");
                ReserveSpace();
            }

            Console.Write("How many days will you need the space?  ");
              inputDays = int.Parse(Console.ReadLine());
              days = inputDays;
              endDate = startDate.AddDays(inputDays-1);

            Console.Write("How many people will be in attendance?  ");
              inputAttendees = int.Parse(Console.ReadLine());
              attendees = Convert.ToString(inputAttendees);

            Console.WriteLine("\nThe following spaces are available based on your needs:\n\n");

            DisplayNotReservedSpaces();

            Console.Write("\nWhich space would you like to reserve? (enter 0 to cancel)  ");
             spaceInt = int.Parse(Console.ReadLine());
            
                   if (spaceInt == 0)
                   {
                   MainMenu();  
                   }

            Console.WriteLine("Who is this reservation for?  ");
              reservationName = Console.ReadLine();
              reservedFor = reservationName;

            reservationDAO.InsertReservation(spaceInt, attendees, startDate, endDate, reservedFor);

            Console.WriteLine("\n\nThanks for submitting your reservation! the details for your event are listed below:\n\n");

            DisplayReservationDetails();
        }
        

        private void DisplayVenues()
        {
            List<Venue> venues = venueDAO.ListVenues();
            int i = 1;
                foreach(Venue temp in venues)
                {
                    Console.WriteLine(i + ")\t" + temp.Name);
                    i++;
                }  
        }

        private void DisplayVenueDetails(int venueSelection)
        {
            List<Venue> venues = venueDAO.ListVenues();
            List<Category> categories = categoryDAO.ListCategories(venues[venueSelection - 1].Id);

            foreach (Venue temp in venues)
            {
                if (temp.Id == venues[venueSelection - 1].Id)
                {
                    Console.Write("\n" + temp.Name + "\nLocation: " + temp.CityName + ", " + temp.StateAbbreviation + "\nCategories: ");

                    foreach (Category category in categories)
                    {
                        Console.Write(category.CatName +"\n\t    ");

                    }
                    Console.WriteLine("\n" + temp.Description);
                }
            }
        }
        



        private void DisplaySpaces(int venueSelection)
        {
            List<Venue> venues = venueDAO.ListVenues();
            List<Space> spaces = spaceDAO.GetSpecificVenueSpace(venues[venueSelection-1].Id);

            Console.WriteLine("\t" + venues[venueSelection-1].Name + "\n\n" +
                             "\tName\t\t\t\t\tOpen\t\tClose\t\tDaily Rate\tMax.Occupancy\n");

            foreach(Space tempSpace in spaces)
            {
                
                Console.WriteLine("#" + tempSpace.Id + "\t" + tempSpace.Name.PadRight(40) +
                CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(tempSpace.OpenFrom) + "\t\t" +
                CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(tempSpace.OpenTo) + "\t\t$" +
                Math.Round(tempSpace.DailyRate, 2) + " \t\t" + tempSpace.MaxOccupancy);

            }
        }

        
        private void DisplayNotReservedSpaces()
        {
            List<Venue> venues = venueDAO.ListVenues();
            List<Reservation> reservations = reservationDAO.ListSpacesNotReserved(startDate, endDate, venues[venueSelection - 1].Id);

            Console.WriteLine("Space #\tName\t\t\t\t\tDaily Rate\tMax Occup.\tAccessible?\tTotal Cost");
            
            foreach (Reservation res in reservations)
            {
                if (res.MaxOccupancy >= inputAttendees && startDate.Month >= res.OpenFrom && endDate.Month <= res.OpenTo)
                {
                Console.Write(res.SpaceId + "\t" + res.Name.PadRight(40) + "$" + res.DailyRate + "\t\t" + res.MaxOccupancy + "\t\t");
                                   
                      if (res.IsAccessible == 1)
                      {
                      Console.Write("Yes");
                      }
                      else
                      {
                      Console.Write("No");
                      }
                Console.Write("\t\t$" + (res.DailyRate * inputDays) + "\n");
                }
            }
        }

       

        private void DisplayReservationDetails()
        {
            
            Random rand = new Random();
            List<Venue> venues = venueDAO.ListVenues();
            List<Space> spaceInfo = spaceDAO.GetSpecificVenueSpace(venues[venueSelection - 1].Id);
            confirmation = rand.Next(10000000, 99999999);

            Console.WriteLine("Confirmation #: " + confirmation);
          
   
            Console.WriteLine("Venue: " + venues[venueSelection - 1].Name);       
              foreach (Space s in spaceInfo)
                        {
                            if(s.Id == spaceInt)
                            {
                                Console.WriteLine("Space: " + s.Name);
                                dailyRate = s.DailyRate;
                            }
                        }
            Console.WriteLine("Reserved For: " + reservedFor);
            Console.WriteLine("Attendees: " + attendees);
            Console.WriteLine("Arrival Date: "+ startDate);
            Console.WriteLine("Depart Date: " + endDate);
            Console.WriteLine("Total Cost: " + dailyRate * days);
        }


        //****BONUS*****

        private void SearchForASpace()
        {
                Console.Write("When do you need the space? (MM/DD/YYYY)  ");
                string istartDate = Console.ReadLine();
                if (DateTime.TryParse(istartDate, out DateTime RstartDate) == true)
                {
                    startDate = Convert.ToDateTime(istartDate);
                }
                else
                {
                    Console.WriteLine("Error, Try again");
                    SearchForASpace();
                }

            Console.Write("How many days will you need the space?  ");
                inputDays = int.Parse(Console.ReadLine());
                days = inputDays;
                endDate = startDate.AddDays(inputDays - 1);
            
                Console.Write("How many people will be in attendance?  ");
                inputAttendees = int.Parse(Console.ReadLine());
                attendees = Convert.ToString(inputAttendees);

                Console.WriteLine("\nThe following spaces are available based on your needs:\n\n");

            DisplayNotReservedSpacesForAllVenues();

            Console.Write("\nWhich space would you like to reserve? (enter 0 to cancel)  ");
            spaceInt = int.Parse(Console.ReadLine());

            if (spaceInt == 0)
            {
                MainMenu();
            }

            Console.WriteLine("Who is this reservation for?  ");
            reservationName = Console.ReadLine();
            reservedFor = reservationName;

            reservationDAO.InsertReservation(spaceInt, attendees, startDate, endDate, reservedFor);

            Console.WriteLine("\n\nThanks for submitting your reservation! the details for your event are listed below:\n\n");

            DisplayReservationDetailsBonus();


        }
        private void DisplayNotReservedSpacesForAllVenues()
        {

            List<Reservation> reservations = reservationDAO.ListSpacesNotReservedForAllVenues(startDate, endDate);

            Console.WriteLine("Space #\tName\t\t\t\t\tDaily Rate\tMax Occup.\tAccessible?\tTotal Cost");

            foreach (Reservation res in reservations)
            {
                if (res.MaxOccupancy >= inputAttendees)
                {
                    Console.Write(res.SpaceId + "\t" + res.Name.PadRight(40) + "$" + res.DailyRate + "\t\t" + res.MaxOccupancy + "\t\t");

                    if (res.IsAccessible == 1)
                    {
                        Console.Write("Yes");
                    }
                    else
                    {
                        Console.Write("No");
                    }
                    Console.Write("\t\t$" + (res.DailyRate * inputDays) + "\n");
                }
            }
        }
        private void DisplayReservations()
        {
            ReservationSqlDAO resDAO = new ReservationSqlDAO(connectionString);
            List<Reservation> resList = resDAO.ListResevations();

            Console.WriteLine("The following reservations are coming up in the next 30 days:");

            Console.WriteLine("Venue" + "\t\t\t" + "Space" + "\t\t\t\t" + "Reserved For" + "\t\t\t\t\t" + "From" + "\t\t\t\t" + "To");
            Console.WriteLine();
            foreach (Reservation res in resList)
            {
                if(Convert.ToDateTime(res.EndDate) < DateTime.Now.AddDays(30))
                {
                    
                    Console.WriteLine(res.VenueName.PadRight(30) + res.SpaceName.PadRight(32) + res.ReservedFor.PadRight(30) + res.StartDate.Date + "\t" + res.EndDate.Date);
                }
            }
        }

       
        private void DisplayReservationDetailsBonus()
        {

            Random rand = new Random();
            List<Space> spaceInfo = spaceDAO.GetSpecificVenueSpace(venueSelection);
            List<Venue> venues = venueDAO.ListVenues();
            confirmation = rand.Next(10000000, 99999999);

            Console.WriteLine("Confirmation #: " + confirmation);


            Console.WriteLine("Venue: " + venues[venueSelection - 1].Name);
            foreach (Space s in spaceInfo)
            {
                if (s.VenueId == venues[venueSelection-1].Id)
                {
                    Console.WriteLine("Space: " + s.Name);
                    dailyRate = s.DailyRate;
                }
            }
            Console.WriteLine("Reserved For: " + reservedFor);
            Console.WriteLine("Attendees: " + attendees);
            Console.WriteLine("Arrival Date: " + startDate);
            Console.WriteLine("Depart Date: " + endDate);
            Console.WriteLine("Total Cost: " + dailyRate * days);
        }
    }
}

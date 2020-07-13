using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {

        public Reservation()
        {

        }

        public int VenueId { get; set; }
        public int SpaceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public int IsAccessible { get; set; }
        public int DailyRate { get; set; }
        public int MaxOccupancy { get; set; }
        public int OpenFrom { get; set; }
        public int OpenTo { get; set; }

        //Bonus
        public string ReservedFor { get; set; }
        public string SpaceName { get; set; }
        public string VenueName { get; set; }

    }
}

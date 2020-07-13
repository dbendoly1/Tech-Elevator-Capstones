using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Venue
    {

        
        public Venue()
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string Description { get; set; }

        //city properties
        public int CityId2 { get; set; }
        public string CityName { get; set; }
        public string StateAbbreviation { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Category
    {
        public Category()
        {

        }

        //category properties
        public int CatId { get; set; }
        public string CatName { get; set; }

        //category_venue properties
        public int VenueId { get; set; }
        public int CategoryId { get; set; }



    }
}

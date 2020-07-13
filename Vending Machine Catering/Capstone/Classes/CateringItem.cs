using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class CateringItem
    {
        // This class should contain the definition for one catering item
        // Constructor
        public CateringItem (string name, decimal price, string itemId)
        {
            ItemId = itemId;
            Name = name;
            Price = price;
            Inventory = 50;
        }

        public string ItemId
        {
            get;
        }
        public string Name
        {
            get;
        }
        public decimal Price
        {
            get;
        }
        public int Inventory
        {
            get; set;
        }
    }
}

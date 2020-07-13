using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class TotalSalesData
    {
        public TotalSalesData (string name, decimal totalRevenue, int totalQuantity)
        {
            Name = name;
            TotalPurchasedRevenue = totalRevenue;
            TotalSoldQuantity = totalQuantity;
        }

        public string Name
        {
            get; set;
        }
        public decimal TotalPurchasedRevenue
        {
            get; set;
        }
        public int TotalSoldQuantity
        {
            get; set;
        }
    }
}

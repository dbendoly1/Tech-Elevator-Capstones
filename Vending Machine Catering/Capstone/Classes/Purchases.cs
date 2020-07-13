using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Purchases
    {
        public Purchases(string itemCode, string name, decimal price, int quantityOrdered)
        {
            switch (itemCode.Substring(0,1))
            {
                case "B":
                    ItemType = "Beverage";
                    break;
                case "E":
                    ItemType = "Entree";
                    break;
                case "A":
                    ItemType = "Appetizer";
                    break;
                case "D":
                    ItemType = "Dessert";
                    break;
            }
            QuantityOrdered = quantityOrdered;
            PricePerUnit = price;
            TotalPurchaseCost = QuantityOrdered * PricePerUnit;
            ItemName = name;
        }
        public string ItemType
        {
            get;
        }
        public int QuantityOrdered
        {
            get;
        }
        public decimal PricePerUnit
        {
            get;
        }
        public decimal TotalPurchaseCost
        {
            get;
        }
        public string ItemName
        {
            get;
        }

    }
}

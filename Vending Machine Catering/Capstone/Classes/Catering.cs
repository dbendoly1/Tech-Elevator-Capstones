using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class Catering
    {
        // This class should contain all the "work" for catering

        //Property for CurrentAccountBalance
        public decimal CurrentAccountBalance
        {
            get; set;
        }

        //Create List of Inventory Items
        FileAccess inAndOut = new FileAccess();
        public List<CateringItem> ourStock;
        public List<Purchases> purchaseHistory = new List<Purchases>();

        public void InitializeCateringItems()
        {
            ourStock = inAndOut.SetUpCateringItems();
        }

        public string AddMoneyToAcct(int moneyToAdd)
        {
            
            string result;
            if (CurrentAccountBalance + moneyToAdd <= 5000.00M)
            {
                CurrentAccountBalance += moneyToAdd;
                result = "Amount was successfully added.";
                inAndOut.PrintToLog("ADD MONEY:", "$" + moneyToAdd, "$" + CurrentAccountBalance);
            }
            else
            {
                result = "Error: Maximum Account Balance is $5000.";
            }
            return result;
        }

        public string MakePurchase(string itemCode, int quantity)
        {
            string result = "";
            string name = "";
            decimal price = -1M;
            int itemInventory = 0;
            bool enoughInventory = false;
            foreach (CateringItem item in ourStock)
            {
                if (item.ItemId == itemCode)
                {
                    itemInventory = item.Inventory;
                    price = item.Price;
                    name = item.Name;
                    if (quantity <= item.Inventory && price * quantity <= CurrentAccountBalance)
                    {
                        item.Inventory -= quantity;
                        enoughInventory = true;
                    }
                }

            }
            decimal amountOfSale = quantity * price;
            if (price == -1M)
            {
                result = "Invalid Item ID";
            }
            else if (CurrentAccountBalance < amountOfSale)
            {
                result = "Not enough money in account to make purchase";
            }
            else if (!enoughInventory)
            {
                if (itemInventory == 0)
                {
                    result = "Ordered product is sold out";
                }
                else
                {
                    result = "Not enough of ordered product in inventory";
                }
            }
           
            else
            {
                Purchases thisPurchase = new Purchases(itemCode, name, price, quantity);
                purchaseHistory.Add(thisPurchase);
                result = "Purchase was made successfully";
                CurrentAccountBalance -= amountOfSale;
                inAndOut.PrintToLog(quantity + " " + name + " " + itemCode, "$" + amountOfSale, "$" + CurrentAccountBalance);

            }
            return result;
        }


        public void RunUpdateSalesReport()
        {
            inAndOut.UpdateTotalSalesReport(purchaseHistory);
        }


        public string DispenseChange(decimal amountOfChange)
        {
            string changeAmount = "$" + amountOfChange;
            string result = "Change is: ";
            result += ConvertToBills(ref amountOfChange, 100.00M, "Hundred Dollar Bills");
            result += ConvertToBills(ref amountOfChange, 20.00M, "Twenty Dollar Bills");
            result += ConvertToBills(ref amountOfChange, 10.00M, "Ten Dollar Bills");
            result += ConvertToBills(ref amountOfChange, 5.00M, "Five Dollar Bills");
            result += ConvertToBills(ref amountOfChange, 1.00M, "One Dollar Bills");
            result += ConvertToBills(ref amountOfChange, 0.25M, "Quarters");
            result += ConvertToBills(ref amountOfChange, 0.10M, "Dimes");
            result += ConvertToBills(ref amountOfChange, 0.05M, "Nickels");
            result = result.Substring(0, result.Length - 2);
            result += ".";
            inAndOut.PrintToLog("GIVE CHANGE:", changeAmount, "$0.00");
            return result;
        }

        public string ConvertToBills(ref decimal amountOfChange, decimal targetAmount, string dollarType)
        {
            int currencyCount = 0;
            while (amountOfChange >= targetAmount)
            {
                amountOfChange -= targetAmount;
                currencyCount++;
            }
            if (currencyCount == 0)
            {
                return "";
            }
            else if (currencyCount == 1)
            {
                dollarType = dollarType.Substring(0, dollarType.Length - 1);
            }
            return currencyCount + " " + dollarType + ", ";
        }










    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class UserInterface
    {
        // This class provides all user communications, but not much else.
        // All the "work" of the application should be done elsewhere
        // ALL instances of Console.ReadLine and Console.WriteLine should 
        // be in this class.

        private Catering catering = new Catering();

        public void DisplayInventory()
        {
            Console.WriteLine("Available Items: ");
            foreach (CateringItem item in catering.ourStock)
            {
                Console.Write("\n" + item.ItemId + " " + item.Name + " $" + item.Price);
                if (item.Inventory > 0 )
                {
                    Console.Write(" | " + item.Inventory + " Available");
                }
                else
                {
                    Console.Write(" | SOLD OUT");
                }
            }
            Console.WriteLine();
        }



        public void RunInterface()
        {
            catering.InitializeCateringItems();
            bool done = false;
            Console.WriteLine("Welcome to Cater-Mart!");
            while (!done)
            {
                Console.WriteLine("\nMAIN MENU " +
                    "\n (1) Display Catering Items" +
                    "\n (2) Order" +
                    "\n (3) Quit");
                int input = int.Parse(Console.ReadLine());

                switch(input)
                {
                    case 1:
                        DisplayInventory();   
                        break;

                    case 2:
                        OrderMenu();
                        break;

                    case 3:
                        done = true;
                        break;

                    default:
                        break;
                }
            }
        }
        
        public void OrderMenu()
        {
            bool done = false;
            while(!done)
            { 
            Console.WriteLine("\nORDER MENU " +
                "\n (1) Add Money" +
                "\n (2) Select Products" +
                "\n (3) Complete Transaction\n" +
                "\n Current Account Balance: $" + catering.CurrentAccountBalance);
            int input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        string addMoneyResult;
                        Console.WriteLine("How much money would you like to enter?");
                        try
                        {
                            addMoneyResult = catering.AddMoneyToAcct(int.Parse(Console.ReadLine()));
                        }
                        catch (Exception e)
                        {
                            addMoneyResult = "Invalid; please enter a whole-dollar amount";
                        }
                        
                        Console.WriteLine(addMoneyResult);
                        break;

                    case 2:

                        PurchaseMenuOption();                        
                        break;

                    case 3:                      
                        EndTransactionMenuOption();
                        done = true;
                        break;

                    default:
                        break;

                }
            }   
        }

        public string ItemCodeNotCaseSensitive(string itemId)
        {
            itemId = itemId.Substring(0, 1).ToUpper() + itemId.Substring(1);
            return itemId;
        }
        
        public void PurchaseMenuOption()
        {
            DisplayInventory();
            Console.WriteLine("\nEnter item code");
            string itemForPurchase = ItemCodeNotCaseSensitive(Console.ReadLine());
            Console.WriteLine("\nEnter Quantity");
            int itemQuantity = int.Parse(Console.ReadLine());
            string result = catering.MakePurchase(itemForPurchase, itemQuantity);
            Console.WriteLine(result);
        }
        //EndTransactionInterface
        public void EndTransactionMenuOption()
        {
            catering.RunUpdateSalesReport();
            decimal grandTotal = 0M;
            Console.WriteLine("\nRECEIPT:");
            foreach (Purchases item in catering.purchaseHistory)
            {
                grandTotal += item.TotalPurchaseCost;
                Console.WriteLine("\n " + item.QuantityOrdered + "\t" + item.ItemType + "\t" + item.ItemName + "\t" + "$" +
                    item.PricePerUnit + "\t" + "$" + item.TotalPurchaseCost);
            }
            Console.WriteLine("\nTotal: $" + grandTotal + "\n");
            string change = catering.DispenseChange(catering.CurrentAccountBalance);  //Add in purchase report
            Console.WriteLine(change + "\n");

        }
    }
}

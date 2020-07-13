using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    public class FileAccess
    {
        // This class should contain any and all details of access to files

        //SetUpCateringItems Method, returns a list of items from the input file

        //PrintToLog takes three strings, action, price/amountadded, currentacount balance

        private string filePath = @"C:\Catering"; //filepath should be moved to file access class
        private string fileName = "cateringsystem.csv";
        private string logName = "Log.txt";
        private string reportName = "TotalSales.rpt";

        public List<CateringItem> SetUpCateringItems()
        {
            string fullPath = Path.Combine(filePath, fileName);

            List<CateringItem> cateringItems = new List<CateringItem>();

            try 
            {
                using (StreamReader sr = new StreamReader(fullPath))
                {
                    while (!sr.EndOfStream)
                    {
                        string unsplit = sr.ReadLine();
                        string[] split = unsplit.Split('|');

                        CateringItem temp = new CateringItem(split[1], decimal.Parse(split[2]), split[0]);
                        cateringItems.Add(temp);
                        
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("No inventory file present, Contact Technical Support.");
                Console.WriteLine("Press enter to close.");
                Console.ReadLine();
            }
            return cateringItems;
        }

        public void PrintToLog(string action, string amountAdded, string currentAccountBalance)
        {
            string fullPath = Path.Combine(filePath, logName);
            try
            {
                using (StreamWriter sw = new StreamWriter(fullPath, true))
                {
                    sw.WriteLine(DateTime.UtcNow + " " + action + " " + amountAdded + " " + currentAccountBalance);
                }
            }
            catch (IOException e)
            {

            }
        }
        public List<TotalSalesData> ReadInTotalSystemSalesReport()
        {
 
            string fullPath = Path.Combine(filePath, reportName);
            List<string> fileLines = new List<string>();
            List<TotalSalesData> outputList = new List<TotalSalesData>();
            if (File.Exists(fullPath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(fullPath))
                    {
                        while(!sr.EndOfStream)
                        {
                            fileLines.Add(sr.ReadLine());
                        }
                        for (int i = 0; i < fileLines.Count - 2; i++)
                        {
                            string[] data = fileLines[i].Split("|");
                            TotalSalesData temp = new TotalSalesData(data[0], decimal.Parse(data[2]), int.Parse(data[1]));
                            outputList.Add(temp);
                        }
                    }
                }
                catch (IOException e)
                {

                }
            }
            return outputList;
        }

        public void UpdateTotalSalesReport(List<Purchases> purchaseList)
        {
            string fullPath = Path.Combine(filePath, reportName);
            decimal totalSales = 0M;
            List<TotalSalesData> oldDataList = ReadInTotalSystemSalesReport();
            foreach (Purchases thisPurchase in purchaseList)
            {
                bool existsInOldData = false;
                foreach (TotalSalesData thisData in oldDataList)
                {
                    if(thisPurchase.ItemName == thisData.Name)
                    {
                        existsInOldData = true;
                        thisData.TotalSoldQuantity += thisPurchase.QuantityOrdered;
                        thisData.TotalPurchasedRevenue += thisPurchase.TotalPurchaseCost;
                    }
                }
                if(!existsInOldData)
                {
                    TotalSalesData temp = new TotalSalesData(thisPurchase.ItemName, thisPurchase.TotalPurchaseCost, thisPurchase.QuantityOrdered);
                    oldDataList.Add(temp);
                }
            }
            foreach(TotalSalesData data in oldDataList)
            {
                totalSales += data.TotalPurchasedRevenue;
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(fullPath, false))
                {
                    foreach (TotalSalesData data in oldDataList)
                    {
                        sw.WriteLine(data.Name + "|" + data.TotalSoldQuantity + "|" + data.TotalPurchasedRevenue);
                    }
                    sw.WriteLine("\n**TOTAL SALES** $" + totalSales);
                }
            }
            catch (IOException e)
            {

            }
        }
    }
}

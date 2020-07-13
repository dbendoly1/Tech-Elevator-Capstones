using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Capstone.Classes;

namespace Capstone.Tests
{
    [TestClass]
    public class CateringTests
    {
        [TestMethod]
        public void AddMoneyToAccountFailTest()
        {
            //Arrange
            Catering testObj = new Catering();
            testObj.CurrentAccountBalance = 5000;
            //Act
            string result = testObj.AddMoneyToAcct(1);

            //Assert
            Assert.AreEqual("Error: Maximum Account Balance is $5000.", result);

        }

        [TestMethod]
        public void AddMoneyToAccountPassTest()
        {
            //Arrange
            Catering testObj = new Catering();
            testObj.CurrentAccountBalance = 4500;
            //Act
            string result = testObj.AddMoneyToAcct(500);

            //Assert
            Assert.AreEqual("Amount was successfully added.", result);

        }

        [TestMethod]
        public void MakePurchaseInvalidIdTest()
        {
            //Arrange
            Catering testObj = new Catering();
            testObj.CurrentAccountBalance = 4500;
            testObj.InitializeCateringItems();

            //Act
            string result = testObj.MakePurchase("C1", 10);

            //Assert
            Assert.AreEqual("Invalid Item ID", result);

        }
        [TestMethod]
        public void MakePurchaseNotEnoughInventoryTest()
        {
            //Arrange
            Catering testObj = new Catering();
            testObj.CurrentAccountBalance = 4500;
            testObj.InitializeCateringItems();

            //Act
            string result = testObj.MakePurchase("B1", 60);

            //Assert
            Assert.AreEqual("Not enough of ordered product in inventory", result);

        }

        [TestMethod]
        public void MakePurchaseNotEnoughMoneyTest()
        {
            //Arrange
            Catering testObj = new Catering();
            testObj.CurrentAccountBalance = 1;
            testObj.InitializeCateringItems();

            //Act
            string result = testObj.MakePurchase("B1", 1);

            //Assert
            Assert.AreEqual("Not enough money in account to make purchase", result);

        }

        [TestMethod]
        public void MakePurchaseSuccesfulPurchaseTest()
        {
            //Arrange
            Catering testObj = new Catering();
            testObj.CurrentAccountBalance = 75;
            testObj.InitializeCateringItems();

            //Act
            string result = testObj.MakePurchase("B1", 50);

            //Assert
            Assert.AreEqual("Purchase was made successfully", result);

        }

//MAKEPURCHASE SOLD OUT TEST HERE

        [TestMethod]
        //[DataRow("Change is: 1 Hundred Dollar Bills, 1 Twenty Dollar Bills, 1 Ten Dollar Bills," +
        //    "1 Five Dollar Bills, 1 One Dolar Bills, 1 Quarters, 1 Dimes, 1 Nickels, ", new decimal 136.40M)]
        public void DispenseChangeAllTypesTest()
        {
            //Arrange
            Catering testObj = new Catering();
            testObj.InitializeCateringItems();

            //Act
            string result = testObj.DispenseChange(136.40M);

            //Assert
            Assert.AreEqual("Change is: 1 Hundred Dollar Bill, 1 Twenty Dollar Bill, 1 Ten Dollar Bill," +
            " 1 Five Dollar Bill, 1 One Dollar Bill, 1 Quarter, 1 Dime, 1 Nickel.", result);

        }

        [TestMethod]
        public void DispenseChange4570Test()
        {
            //Arrange
            Catering testObj = new Catering();
            testObj.InitializeCateringItems();

            //Act
            string result = testObj.DispenseChange(45.70M);

            //Assert
            Assert.AreEqual("Change is: 2 Twenty Dollar Bills, 1 Five Dollar Bill, 2 Quarters, 2 Dimes.", result);

        }
        //ADD DATATEST METHODS FOR EACH CURRENCY TYPE



        [TestMethod]
        public void ItemCodeNotCaseSensitiveTest()
        {
            UserInterface testObj = new UserInterface();

            string result = testObj.ItemCodeNotCaseSensitive("b3");

            Assert.AreEqual("B3", result);
        }
        [TestMethod]
        public void ItemCodeNotCaseSensitiveTest2()
        {
            UserInterface testObj = new UserInterface();

            string result = testObj.ItemCodeNotCaseSensitive("b333333");

            Assert.AreEqual("B333333", result);
        }

        [TestMethod]
        public void SetUpCateringItemsTest()
        {
            FileAccess testObj = new FileAccess();

            List<CateringItem> result = testObj.SetUpCateringItems();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateSalesReportTest()
        {
            FileAccess testObj = new FileAccess();

            List<TotalSalesData> result = testObj.ReadInTotalSystemSalesReport();

            Assert.IsNotNull(result);
        }
    }
}

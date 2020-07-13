using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Capstone.Tests
{
    [TestClass]
    public class VenuSqlTest : ParentTest
    {
        [TestMethod]
        public void ListVenuTest()
        {

            VenueSqlDAO venueDAO = new VenueSqlDAO(connectionString);
            List<Venue> venueList = venueDAO.ListVenues();

            bool result = false;

            foreach(Venue ven in venueList)
            {
                if (ven.Name == "My new Venue!")
                {
                    result = true;
                }
            }
            Assert.IsTrue(result);
        }



    }
}

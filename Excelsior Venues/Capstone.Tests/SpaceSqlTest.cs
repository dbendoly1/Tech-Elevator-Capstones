using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Tests
{
    [TestClass]
    public class SpaceSqlTest : ParentTest
    {
        [TestMethod]
        public void GetSpecificVenueSpaceTest()
        {
            SpaceSqlDAO spaceSqlDAO = new SpaceSqlDAO(connectionString);
            List<Space> TestSpaceList = spaceSqlDAO.GetSpecificVenueSpace(7);
           

            bool result = false;
            foreach (Space res in TestSpaceList)
            {
                if (res.Name == "The Royal Room")
                {
                    result = true;
                }
            }

            Assert.IsTrue(result);
        }


    }
}

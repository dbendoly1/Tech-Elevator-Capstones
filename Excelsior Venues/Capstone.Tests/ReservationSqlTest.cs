using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;


namespace Capstone.Tests
{
    [TestClass]
    public class ReservationSqlTest : ParentTest
    {

        [TestMethod]
        public void ListSpacesNotReserved()
        {
            ReservationSqlDAO reservationDAO = new ReservationSqlDAO(connectionString);
            List<Reservation> spaceList = reservationDAO.ListSpacesNotReserved(Convert.ToDateTime("06/25/2020"), Convert.ToDateTime("06/29/2020"), 7);

            bool result = false;
            foreach(Reservation res in spaceList)
            {
                if (res.Name == "The Royal Room")
                {
                    result = true;
                }
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void InsertReservationTest()
        {
            ReservationSqlDAO reservationDAO = new ReservationSqlDAO(connectionString);
            Reservation BenderRodriguez = reservationDAO.InsertReservation(28, "25", Convert.ToDateTime("07/20/2020"), Convert.ToDateTime("07/23/2020"), "Bender B. Rodriguez");



            bool result = false;

            if(BenderRodriguez != null)
            {
                result = true;
            }


            Assert.IsTrue(result);
        }

        ////****BONUS****
        [TestMethod]
        public void ListSpacesNotReservedForAllVenuesTest()
        {
            ReservationSqlDAO reservationDAO = new ReservationSqlDAO(connectionString);
            List<Reservation> spaceList = reservationDAO.ListSpacesNotReservedForAllVenues(Convert.ToDateTime("06/25/2020"), Convert.ToDateTime("06/30/2020"));

            bool result = false;
            foreach (Reservation res in spaceList)
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

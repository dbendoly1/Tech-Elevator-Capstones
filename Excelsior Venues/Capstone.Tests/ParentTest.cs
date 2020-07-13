using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class ParentTest
    {
        private TransactionScope trans;

        protected string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=excelsior_venues;Integrated Security=True";

        [TestInitialize]
        public void Setup()
        {
            trans = new TransactionScope();

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql_insert = "INSERT INTO venue VALUES('My new Venue!', '1', 'You guessed its a new venue!')";
                SqlCommand cmd = new SqlCommand(sql_insert, conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Reset()
        {
            trans.Dispose();
        }

    }
}

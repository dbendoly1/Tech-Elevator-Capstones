using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDao
    {
        private readonly string connectionString;

        const string sql_GetAccountBalance = "SELECT balance FROM accounts WHERE user_id = @userId";
        const string sql_MoveFundsTo = "UPDATE accounts \nSET balance = @newamount \nWHERE user_id = @userID";
        const string sql_MoveFundsFrom = "UPDATE accounts \nSET balance = @newamount \nWHERE user_id = @userID";
        const string sql_GetUserId = "SELECT user_id FROM accounts WHERE account_id = @accountId ";
        const string sql_GetAccountId = "SELECT account_id FROM accounts WHERE user_id = @userId ";

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public decimal GetAccountBalance(int user_id)
        {
            decimal balance = 0.00M;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetAccountBalance, conn);
                    cmd.Parameters.AddWithValue("@userId", user_id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        balance = Convert.ToDecimal(reader["balance"]);
                    }

                    //cmd.Parameters.AddWithValue("@balance", balance);
                    return balance;
                }
            }
            catch (SqlException ex)
            {
                return balance;
            }

        }
        public bool MoveFundsTo(int user_id, decimal amount)
        {
            bool successful = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    decimal newAmount = GetAccountBalance(user_id) + amount;

                    SqlCommand cmd = new SqlCommand(sql_MoveFundsTo, conn);
                    cmd.Parameters.AddWithValue("@userId", user_id);
                    cmd.Parameters.AddWithValue("@newamount", newAmount);

                    cmd.ExecuteNonQuery();

                    successful = true;
                    return successful;
                }
            }
            catch (Exception ex)
            {
                return successful;
            }
        }
        public bool MoveFundsFrom(int user_id, decimal amount)
        {
            bool successful = false;
            //AccountDao account = new IAccountDao();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    decimal newAmount = GetAccountBalance(user_id) - amount;

                    SqlCommand cmd = new SqlCommand(sql_MoveFundsFrom, conn);
                    cmd.Parameters.AddWithValue("@userId", user_id);
                    cmd.Parameters.AddWithValue("@newamount", newAmount);

                    cmd.ExecuteNonQuery();

                    successful = true;
                    return successful;
                }
            }
            catch (Exception ex)
            {
                return successful;
            }
        }

        public int GetUserId(int account_id)
        {
            int result = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetUserId, conn);
                    cmd.Parameters.AddWithValue("@accountId", account_id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = Convert.ToInt32(reader["user_id"]);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return result;
            }
        }
        public int GetAccountId(int user_id)
        {
            int result = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetAccountId, conn);
                    cmd.Parameters.AddWithValue("@userId", user_id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = Convert.ToInt32(reader["account_id"]);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return result;
            }
        }

    }
            
}

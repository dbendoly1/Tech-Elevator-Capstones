using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        const string sql_AddTransfer = "INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount)\n" +
                                       "VALUES (@type, @status, @transferFrom, @transferTo, @amount)";
        const string sql_GetFullTransList = "SELECT * FROM transfers";
        const string sql_ApproveRequest = "UPDATE transfers \n" +
                                          "SET transfer_status_id = 2\n" +
                                          "WHERE transfer_id = @transfer_id";
        const string sql_DenyRequest = "UPDATE transfers \n" +
                                         "SET transfer_status_id = 3\n" +
                                         "WHERE transfer_id = @transfer_id";

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public bool AddSendTransfer(int transferTo, int transferFrom, decimal amount)
        {
            bool successful = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    //@type, @status, @transferFrom, @transferTo, @amount

                    SqlCommand cmd = new SqlCommand(sql_AddTransfer, conn);
                    cmd.Parameters.AddWithValue("@type", 2);
                    cmd.Parameters.AddWithValue("@status", 2);
                    cmd.Parameters.AddWithValue("@transferFrom", transferFrom);
                    cmd.Parameters.AddWithValue("@transferTo", transferTo);
                    cmd.Parameters.AddWithValue("@amount", amount);

                    cmd.ExecuteNonQuery();

                    successful = true;
                    return successful;

                }
            }
            catch (SqlException ex)
            {
                return successful;
            }
        }

        public bool AddRequestTransfer(int transferTo, int transferFrom, decimal amount)
        {
            bool successful = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    //@type, @status, @transferFrom, @transferTo, @amount

                    SqlCommand cmd = new SqlCommand(sql_AddTransfer, conn);
                    cmd.Parameters.AddWithValue("@type", 1);
                    cmd.Parameters.AddWithValue("@status", 1);
                    cmd.Parameters.AddWithValue("@transferFrom", transferFrom);
                    cmd.Parameters.AddWithValue("@transferTo", transferTo);
                    cmd.Parameters.AddWithValue("@amount", amount);

                    cmd.ExecuteNonQuery();

                    successful = true;
                    return successful;

                }
            }
            catch (SqlException ex)
            {
                return successful;
            }
        }

        
        public List<Transfer> GetFullTransList()
        {
            List<Transfer> transList = new List<Transfer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetFullTransList, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Transfer t = GetTransferFromReader(reader);
                            transList.Add(t);
                        }
                    }
                    return transList;
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public Transfer GetSpecificTransfer(int transferId)
        {
            List<Transfer> fullList = GetFullTransList();
            Transfer selected = new Transfer();
            foreach(Transfer t in fullList)
            {
                if(t.TransferId == transferId)
                {
                    selected = t;
                }
            }
            return selected;
        }

        public bool DenyRequest(int transferId)
        {
            bool approved = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql_DenyRequest, conn);
                    cmd.Parameters.AddWithValue("@transfer_id", transferId);

                    cmd.ExecuteNonQuery();
                    approved = true;
                    return approved;
                }
            }
            catch (Exception ex)
            {
                return approved;
                throw;
            }

        }
        
        public bool ApproveRequest(int transferId)
        {

            bool approved = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql_ApproveRequest, conn);
                    cmd.Parameters.AddWithValue("@transfer_id", transferId);

                    cmd.ExecuteNonQuery();
                    approved = true;
                    return approved;
                }
            }
            catch(Exception ex)
            {
                return approved;
                throw;
            }
        }
    
        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer t = new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                TypeId = Convert.ToInt32(reader["transfer_type_id"]),
                StatusId = Convert.ToInt32(reader["transfer_status_id"]),
                AccountFrom = Convert.ToInt32(reader["account_from"]),
                TransferTo = Convert.ToInt32(reader["account_to"]),
                Amount = Convert.ToDecimal(reader["amount"])
            };
            return t;
        }
        Transfer ITransferDao.GetTransferFromReader(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

       
    }
}



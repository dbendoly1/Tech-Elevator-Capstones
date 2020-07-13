using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        bool AddSendTransfer(int transferTo, int transferFrom, decimal amount);
        List<Transfer> GetFullTransList();
        Transfer GetTransferFromReader(SqlDataReader reader);
        bool AddRequestTransfer(int transferTo, int transferFrom, decimal amount);
        Transfer GetSpecificTransfer(int transferId);
        bool ApproveRequest(int transferId);
        bool DenyRequest(int transferId);
    }
}

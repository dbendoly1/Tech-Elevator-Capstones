using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        decimal GetAccountBalance(int user_id);
        bool MoveFundsFrom(int user_id, decimal amount);
        bool MoveFundsTo(int user_id, decimal amount);
        int GetUserId(int account_id);
        int GetAccountId(int user_id);
    }
}

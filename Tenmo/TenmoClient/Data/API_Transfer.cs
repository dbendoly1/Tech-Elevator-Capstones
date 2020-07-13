using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class API_Transfer
    {
        public int TransferTo { get; set; }
        public decimal Amount { get; set; }
    }

    public class API_TransferDetails
    {
        public int TransferId { get; set; }
    }
    

}

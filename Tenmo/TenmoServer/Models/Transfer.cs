using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int AccountFrom { get; set; }
        public int TransferTo { get; set; }
        public decimal Amount { get; set; }
    }
}

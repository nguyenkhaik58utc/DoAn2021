using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ServiceCommandContractItem
    {
        public int ID { get; set; }
        public int ContractID { get; set; }
        public string ContractName { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int CommandID { get; set; }
        public string CommandName { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

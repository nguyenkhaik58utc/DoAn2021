using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductionPerformHistoryItem
    {
        public int ID { get; set; }
        public int ProductionPerformID { get; set; }
        public int HumanEmployeeID { get; set; }
        public System.DateTime Time { get; set; }
        public string Code { get; set; }
        public int StockProductID { get; set; }
        public string Content { get; set; }
        public string Error { get; set; }
        public string Cause { get; set; }
        public string Solution { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

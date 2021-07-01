using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductionPerformResultItem
    {
        public int ID { get; set; }
        public HumanEmployeeViewItem HumanEmployee { get; set; }
        public int ProductionPeformID { get; set; }
        public int StockProductID { get; set; }
        public Nullable<float> MaterialNumber { get; set; }
        public Nullable<float> MaterialExitsNumber { get; set; }
        public Nullable<int> ProductionPerformProductResultID { get; set; }
        public string Cause { get; set; }
        public string Solution { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }

        public int? Quantity { get; set; }

        public bool IsAbsent { get; set; }
    }
}

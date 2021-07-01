using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class AdjustmentDetailItem
    {
        public int ID { get; set; }
        public int StockAdjustmentID { get; set; }
        public int StockProductID { get; set; }
        public string Product_Code { get; set; }
        public string Unit { get; set; }
        public string Product_Name { get; set; }
        public string Group_Name { get; set; }
        public Nullable<int> StockID { get; set; }
        public Nullable<int> Unit_ID { get; set; }
        public Nullable<decimal> UnitConvert { get; set; }
        public Nullable<decimal> Width { get; set; }
        public Nullable<decimal> Height { get; set; }
        public string Orgin { get; set; }
        public Nullable<decimal> CurrentQty { get; set; }
        public Nullable<decimal> NewQty { get; set; }
        public Nullable<decimal> QtyDiff { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> QtyConvert { get; set; }
        public Nullable<long> StoreID { get; set; }
        public string Batch { get; set; }
        public string Serial { get; set; }
        public string Description { get; set; }
        public Nullable<long> Sorted { get; set; }
        public System.DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

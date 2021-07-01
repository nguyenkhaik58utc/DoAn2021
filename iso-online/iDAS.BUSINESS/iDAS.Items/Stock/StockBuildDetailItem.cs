using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public  class StockBuildDetailItem
    {
        public int ID { get; set; }
        public int Build_ID { get; set; }
        public string Build_Code { get; set; }
        public int StockProductID { get; set; }
        public string Product_Code { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> RefType { get; set; }
        public Nullable<int> RefTypeSub { get; set; }
        public string RefTypeSubName { get; set; }
        public Nullable<int> StockID { get; set; }
        public string Group_Name { get; set; }
        public string Stock_Name { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> UnitConvert { get; set; }
        public Nullable<int> Vat { get; set; }
        public Nullable<decimal> CurrentQty { get; set; }
        public Nullable<decimal> QuantityDefault { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> QtyConvert { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> Charge { get; set; }
        public Nullable<System.DateTime> Limit { get; set; }
        public string Serial { get; set; }
        public string Description { get; set; }
        public long Sorted { get; set; }
        public System.DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

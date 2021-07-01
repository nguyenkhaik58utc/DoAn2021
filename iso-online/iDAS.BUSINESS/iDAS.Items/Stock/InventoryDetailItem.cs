using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class InventoryDetailItem
    {
        public int ID { get; set; }
        public string RefNo { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
        public Nullable<System.Guid> RefDetailNo { get; set; }
        public Nullable<int> RefType { get; set; }
        public Nullable<int> RefStatus { get; set; }
        public Nullable<int> StoreID { get; set; }
        public Nullable<int> StockID { get; set; }
        public Nullable<int> StockProductID { get; set; }
        public string Stock_Name { get; set; }
        public string RefType_Name { get; set; }
        public string RefStatus_Name { get; set; }
        public string Product_Code { get; set; }
        public string Product_Name { get; set; }
        public Nullable<int> Customer_ID { get; set; }    
        public Nullable<int> Employee_ID { get; set; }
        public string Batch { get; set; }
        public string Serial { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> E_Qty { get; set; }
        public Nullable<decimal> E_Amt { get; set; }
        public string Description { get; set; }
        public Nullable<long> Sorted { get; set; }
        public Nullable<int> Book_ID { get; set; }
        public System.DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
    
}

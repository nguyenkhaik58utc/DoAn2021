using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class InventoryItem
    {
        public int ID { get; set; }
        public string RefID { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
        public Nullable<int> RefType { get; set; }
        public Nullable<int> StockID { get; set; }
        public string Stock_Name { get; set; }
        public string Group_Name { get; set; }
        public Nullable<int> StockProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public Nullable<int> Customer_ID { get; set; }
        public string Customer_Name { get; set; }
        public Nullable<int> Currency_ID { get; set; }
        public Nullable<System.DateTime> Limit { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string Batch { get; set; }
        public string Serial { get; set; }
        public string Unit { get; set; }
        public string ChassyNo { get; set; }
        public string Color { get; set; }
        public string Location { get; set; }
        public Nullable<decimal> Width { get; set; }
        public Nullable<decimal> Height { get; set; }
        public string Orgin { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> NumberBegin { get; set; }
        public Nullable<decimal> NumberInward { get; set; }
        public Nullable<decimal> NumberOutward { get; set; }
        public System.DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class StockInwardItem
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
        public string Ref_OrgNo { get; set; }
        public Nullable<int> RefType { get; set; }
        public string RefType_Name { get; set; }
        public string Barcode { get; set; }
        public Nullable<int> Department_ID { get; set; }
        public Nullable<int> Employee_ID { get; set; }
        public Nullable<int> StockID { get; set; }
        public Nullable<int> Branch_ID { get; set; }
        public Nullable<int> Contract_ID { get; set; }
        public Nullable<int> Customer_ID { get; set; }
        public bool IsInside { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string Contact { get; set; }
        public string Reason { get; set; }
        public Nullable<short> Payment { get; set; }
        public Nullable<int> Currency_ID { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<int> Vat { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> FAmount { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> Charge { get; set; }
        public Nullable<bool> IsClose { get; set; }
        public string Description { get; set; }
        public Nullable<long> Sorted { get; set; }
        public bool Active { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
   
}

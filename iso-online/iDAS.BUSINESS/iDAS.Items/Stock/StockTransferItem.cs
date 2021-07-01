using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class StockTransferItem
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
        public string Ref_OrgNo { get; set; }
        public Nullable<int> RefType { get; set; }
        public Nullable<int> Department_ID { get; set; }
        public Nullable<int> Employee_ID { get; set; }
        public string EmployeeName { get; set; }
        public string FromStockID { get; set; }
        public Nullable<int> Sender_ID { get; set; }
        public string ToStockID { get; set; }
        public Nullable<int> Receiver_ID { get; set; }
        public Nullable<int> Branch_ID { get; set; }
        public Nullable<int> Contract_ID { get; set; }
        public Nullable<int> Currency_ID { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }        
        public string Barcode { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public bool IsReview { get; set; }
        public bool IsClose { get; set; }
        public Nullable<long> Sorted { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public System.DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class TransRefItem
    {
        public int ID { get; set; }
        public Nullable<int> RefID { get; set; }     
        public string RefOrgNo { get; set; }
        public Nullable<int> RefType { get; set; }      
        public Nullable<System.DateTime> RefDate { get; set; }
        public string TransFrom { get; set; }
        public string TransTo { get; set; }
        public string RefType_Name { get; set; }
        public string Employee_Name { get; set; }
        public Nullable<int> Department_ID { get; set; }
        public Nullable<int> Employee_ID { get; set; }
        public Nullable<int> StockID { get; set; }
        public Nullable<int> Branch_ID { get; set; }
        public Nullable<int> Contract_ID { get; set; }
        public string Contract { get; set; }
        public string Reason { get; set; }
        public Nullable<int> Currency_ID { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }         
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> FAmount { get; set; }
        public Nullable<decimal> FDiscount { get; set; }
        public Nullable<bool> IsClose { get; set; }
        public Nullable<long> Sorted { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Active { get; set; }
        public System.DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }          
    }
}

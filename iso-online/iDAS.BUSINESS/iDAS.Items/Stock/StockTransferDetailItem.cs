using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class StockTransferDetailItem
    {
        public int ID { get; set; }
        public int Transfer_ID { get; set; }
        public int NumberOrder { get; set; }
        public Nullable<int> RefType { get; set; }
        public int StockProductID { get; set; }
        public string Product_Code { get; set; }
        public string Transfer_Code { get; set; }
        public string Product_Name { get; set; }
        public string Group_Name { get; set; }
        public string InStock_Name { get; set; }
        public string OutStock_Name { get; set; }
        public Nullable<int> OutStock { get; set; }
        public Nullable<int> InStock { get; set; }
        public Nullable<decimal> Lev1 { get; set; }
        public Nullable<decimal> Lev2 { get; set; }
        public Nullable<decimal> Lev3 { get; set; }
        public Nullable<decimal> Lev4 { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> UnitConvert { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> QtyConvert { get; set; }
        public Nullable<long> StoreID { get; set; }
        public string Batch { get; set; }
        public string Serial { get; set; }
        public string Description { get; set; }
        public long Sorted { get; set; }
        public System.DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
   public class TransferInfo
   {
       public int ID { get; set; }
       public string RefDate { get; set; }
       public string Ref_OrgNo { get; set; }
       public string EmployeeName { get; set; }
       public string ContractCode { get; set; }
       public string Barcode { get; set; }
       public Nullable<decimal> Amount { get; set; }
       public Nullable<decimal> FAmount { get; set; }
       public string Description { get; set; }
       public string DateReport { get; set; }
       public string CustomerName { get; set; }
       public byte[] CustomerLogo { get; set; }
       public List<StockTransferDetailItem> Items { get; set; }
   }  
}

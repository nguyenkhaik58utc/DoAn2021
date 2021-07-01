using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class StockInwardDetailItem
    {
        public int ID { get; set; }
        public int Inward_ID { get; set; }
        public int NumberOrder { get; set; }
        public string Inward_Code { get; set; }
        public int StockProductID { get; set; }
        public string Group_Name { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> RefType { get; set; }
        public Nullable<int> StockID { get; set; }
        public string Stock_Name { get; set; }
        public Nullable<decimal> Lev1 { get; set; }
        public Nullable<decimal> Lev2 { get; set; }
        public Nullable<decimal> Lev3 { get; set; }
        public Nullable<decimal> Lev4 { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> UnitConvert { get; set; }
        public Nullable<int> Vat { get; set; }
        public Nullable<decimal> CurrentQty { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> QtyConvert { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> Charge { get; set; }
        public Nullable<System.DateTime> Limit { get; set; }
        public Nullable<decimal> Width { get; set; }
        public Nullable<decimal> Height { get; set; }
        public string Orgin { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Batch { get; set; }
        public string Serial { get; set; }
        public string ChassyNo { get; set; }
        public string IME { get; set; }
        public Nullable<long> StoreID { get; set; }
        public string Description { get; set; }
        public long Sorted { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }


    }
    public class InwardInfo
    {
        public int ID { get; set; }
        public string RefDate { get; set; }
        public string Ref_OrgNo { get; set; }
        public Nullable<int> RefType { get; set; }
        public string RefType_Name { get; set; }
        public string Barcode { get; set; }
        public Nullable<int> Department_ID { get; set; }
        public Nullable<int> Employee_ID { get; set; }
        public Nullable<int> StockID { get; set; }
        public Nullable<int> Branch_ID { get; set; }
        public string ContractCode { get; set; }
        public Nullable<int> Customer_ID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string Contact { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
        public Nullable<short> Payment { get; set; }
        public Nullable<int> Currency_ID { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<int> Vat { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> FAmount { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public string DateReport { get; set; }
        public string CustomerName { get; set; }
        public byte[] CustomerLogo { get; set; }
        public string CreateName { get; set; }
        public List<StockInwardDetailItem> Items { get; set; }
    }
}

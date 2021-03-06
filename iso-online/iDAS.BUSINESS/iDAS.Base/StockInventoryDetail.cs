//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iDAS.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class StockInventoryDetail
    {
        public int ID { get; set; }
        public Nullable<int> StockInventoryID { get; set; }
        public string RefNo { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
        public Nullable<System.Guid> RefDetailNo { get; set; }
        public Nullable<int> RefType { get; set; }
        public Nullable<int> RefStatus { get; set; }
        public Nullable<int> StoreID { get; set; }
        public Nullable<int> StockID { get; set; }
        public Nullable<int> StockProductID { get; set; }
        public string Product_Name { get; set; }
        public Nullable<int> Supplier_ID { get; set; }
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
    
        public virtual StockInventory StockInventory { get; set; }
        public virtual StockProduct StockProduct { get; set; }
        public virtual Stock Stock { get; set; }
    }
}

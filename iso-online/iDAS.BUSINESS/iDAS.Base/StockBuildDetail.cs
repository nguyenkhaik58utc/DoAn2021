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
    
    public partial class StockBuildDetail
    {
        public int ID { get; set; }
        public int StockBuildID { get; set; }
        public int StockProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> RefType { get; set; }
        public Nullable<int> RefTypeSub { get; set; }
        public Nullable<int> Stock_ID { get; set; }
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
    
        public virtual StockBuild StockBuild { get; set; }
    }
}

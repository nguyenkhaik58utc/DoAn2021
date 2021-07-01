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
    
    public partial class StockTransfer
    {
        public StockTransfer()
        {
            this.StockTransferDetails = new HashSet<StockTransferDetail>();
        }
    
        public int ID { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
        public string Ref_OrgNo { get; set; }
        public Nullable<int> RefType { get; set; }
        public Nullable<int> Department_ID { get; set; }
        public Nullable<int> Employee_ID { get; set; }
        public string FromStock_ID { get; set; }
        public Nullable<int> Sender_ID { get; set; }
        public string ToStock_ID { get; set; }
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
    
        public virtual ICollection<StockTransferDetail> StockTransferDetails { get; set; }
    }
}
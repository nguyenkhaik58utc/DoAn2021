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
    
    public partial class CustomerContract
    {
        public CustomerContract()
        {
            this.AccountingPayments = new HashSet<AccountingPayment>();
            this.CustomerContractAttachmentFiles = new HashSet<CustomerContractAttachmentFile>();
            this.CustomerOrders = new HashSet<CustomerOrder>();
            this.ServiceCommandContracts = new HashSet<ServiceCommandContract>();
            this.RiskContracts = new HashSet<RiskContract>();
        }
    
        public int ID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<System.DateTime> StatTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public bool IsPause { get; set; }
        public bool IsFinish { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsSignCustomer { get; set; }
        public Nullable<System.DateTime> SignCustomerAt { get; set; }
        public bool IsSignReview { get; set; }
        public bool IsSignCompany { get; set; }
        public Nullable<System.DateTime> SignCompanyAt { get; set; }
        public bool IsSend { get; set; }
        public bool IsEdit { get; set; }
        public bool IsApproval { get; set; }
        public bool IsAccept { get; set; }
        public string ApprovalNote { get; set; }
        public Nullable<System.DateTime> ApprovalAt { get; set; }
        public Nullable<int> ApprovalBy { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public System.DateTime FinishDate { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public bool IsCancel { get; set; }
        public string CancelNote { get; set; }
        public bool IsStart { get; set; }
        public Nullable<System.DateTime> StartRealTime { get; set; }
    
        public virtual ICollection<AccountingPayment> AccountingPayments { get; set; }
        public virtual ICollection<CustomerContractAttachmentFile> CustomerContractAttachmentFiles { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; }
        public virtual ICollection<ServiceCommandContract> ServiceCommandContracts { get; set; }
        public virtual ICollection<RiskContract> RiskContracts { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerAssessResultItem 
    {
        public int ID { get; set; }
        public int AuditID { get; set; }
        public int CustomerID { get; set; }
        public int CriteriaID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public int Point { get; set; }
        public int MinPoint { get; set; }
        public int MaxPoint { get; set; }
        public bool IsCategory { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    }
    public class CustomerAuditResultItems
    {
        public int AuditID { get; set; }
        public int CustomerID { get; set; }
        public int CriteriaCategoryID { get; set; }
    }
    public class SumCustomerAuditItem
    {
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public string Name { get; set; }
        public bool IsCategory { get; set; }
        public int SumPoint { get; set; }
        public int SumCustomerAudit { get; set; }
    }
}
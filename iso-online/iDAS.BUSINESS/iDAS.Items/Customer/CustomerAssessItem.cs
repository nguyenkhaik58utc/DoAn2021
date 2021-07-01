using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerAssessItem
    {
        public int ID { get; set; }
        public int? CriteriaCategoryID { get; set; }
        public string Name { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
        public string Method { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }

    }
    #region Đối tượng để gửi mail
    public class AuditMailObject
    {
        public List<CustomerDetail> Customers { get; set; }
        public List<AuditMailQuestion> MailContent { get; set; }
        public int AuditId { get; set; }
    }
    public class AuditMailQuestion
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Point { get; set; }
        public int MaxPoint { get; set; }
        public int MinPoint { get; set; }
    }
    #endregion
}
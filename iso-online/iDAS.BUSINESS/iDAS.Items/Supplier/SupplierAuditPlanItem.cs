using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class SupplierAuditPlanItem : QualityPlanItem
    {
        public SupplierAuditPlanItem()
        {
            this.SupplierAudits = new HashSet<SupplierAuditItem>();
        }
    
        public int ID { get; set; }
        public int QualityPlanID { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public int SupplierAuditCount { get; set; }
        public int SupplierAuditPassCount { get; set; }
        public int SupplierAuditNotPassCount { get; set; }
        public int SupplierAuditChecked { get; set; }
        public int SupplierAuditNotCheck { get; set; }
        public virtual QualityPlanItem QualityPlan { get; set; }
        public virtual ICollection<SupplierAuditItem> SupplierAudits { get; set; }
    }
}

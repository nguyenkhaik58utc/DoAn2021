using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class SupplierAuditResultItem
    {
        public int ID { get; set; }
        public int SupplierAuditID { get; set; }
        public int QualityCriteria { get; set; }
        public Nullable<decimal> Point { get; set; }
        public Nullable<decimal> Factory { get; set; }
        public Nullable<decimal> TotalPoint { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<decimal> MinPoint { get; set; }
        public Nullable<decimal> MaxPoint { get; set; }

        public virtual QualityCriteriaItem QualityCriteria1 { get; set; }
        public virtual SupplierAuditItem SupplierAudit { get; set; }
    }
}

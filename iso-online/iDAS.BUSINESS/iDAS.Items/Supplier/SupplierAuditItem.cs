using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class SupplierAuditItem
    {
        public SupplierAuditItem()
        {
            this.SupplierAuditResults = new HashSet<SupplierAuditResultItem>();
        }
    
        public int ID { get; set; }
        public int SupplierID { get; set; }
        public int SupplierAuditPlanID { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<bool> IsPass { get; set; }
        public int countSupplierOrder
        {
            get {
                if (Supplier.SuppliersOrders != null)
                    return Supplier.SuppliersOrders.Count;
                else
                    return 0;
            }
            set { }
        }
        public string totalAudit
        {
            get {
                if (SupplierAuditResults != null)
                {
                    decimal total=0;
                    foreach(SupplierAuditResultItem rs in SupplierAuditResults)
                    {
                        if(rs.TotalPoint.HasValue) total += rs.TotalPoint.Value;
                    }
                    string s = total + "/" + SupplierAuditResults.Count;
                    return s;
                }
                else return "";
            }
            set { }
        }
        public virtual SupplierAuditPlanItem SupplierAuditPlan { get; set; }
        public virtual ICollection<SupplierAuditResultItem> SupplierAuditResults { get; set; }
        public virtual SupplierItem Supplier { get; set; }
    }
}

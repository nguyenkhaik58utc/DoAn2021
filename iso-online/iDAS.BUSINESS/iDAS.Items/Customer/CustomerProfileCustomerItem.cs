using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerProfileCustomerItem
    {
        public int ID { get; set; }
        public int InfoContactCount { get; set; }
        public bool IsHasInfoContact { get { if (InfoContactCount > 0) return true; else return false; } }
        public int HistoryContactCount { get; set; }
        public bool IsHasHistoryContact { get { if (HistoryContactCount > 0) return true; else return false; } }
        public int OrderCount { get; set; }
        public bool IsHasOrder { get { if (OrderCount > 0) return true; else return false; } }
        public int ContractCount { get; set; }
        public bool IsHasContract { get { if (ContractCount > 0) return true; else return false; } }
        public int AuditCount { get; set; }
        public bool IsHasAudit { get { if (AuditCount > 0) return true; else return false; } }
    }
}
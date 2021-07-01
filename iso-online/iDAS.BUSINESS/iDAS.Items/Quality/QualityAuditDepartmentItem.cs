using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class QualityAuditDepartmentItem : HumanDepartmentItem
    {   
        public bool IsSelected { get; set; }
        public int AuditProgramID { get; set; }
    }
}

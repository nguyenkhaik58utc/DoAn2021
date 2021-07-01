using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class HumanAuditDepartmentItem
    {
        public int ID { get; set; }
        public int HumanAuditGradationID { get; set; }
        public int HumanDepartmentID { get; set; }
        public string HumanDepartmentName { get; set; }
        public int EmployeeAuditManagementID { get; set; }
        public string EmployeeAuditManagementName { get; set; }
        public int EmployeeAuditLeaderID { get; set; }
        public string EmployeeAuditLeaderName { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

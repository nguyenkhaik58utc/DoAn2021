using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class HumanAuditGradationRoleItem
    {
        public int ID { get; set; }
        public int HumanAuditDepartmentID { get; set; }
        public int HumanRoleID { get; set; }
        public int Factory { get; set; }
        public bool IsAudit { get; set; }
        public bool Status { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string RoleName { get; set; }
        public string DepartmentName { get; set; }

        public int GradationID { get; set; }

        public string GradationName { get; set; }

        public string GradationContent { get; set; }

        public DateTime GradationEndTime { get; set; }

        public bool GradationIsPerform { get; set; }

        public DateTime GradationStartTime { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
       
    }
   public class HumanAuditGradationRoleStatisticalItem
   {
       public int HumanAuditGradationRoleID { get; set; }
       public string DepartmentRoleName { get; set; }
       public string AuditLevel { get; set; }
       public int AuditLevelID { get; set; }
       public int count { get; set; }
       public string DepartmentName { get; set; }
       public string RoleName { get; set; }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanAuditTickResultItem
    {
        public int ID { get; set; }
        public int HumanAuditTickID { get; set; }
        public string HumanAuditCriteriaName { get; set; }
        public Nullable<int> AuditEmployeeResult { get; set; }
        public string AuditEmployeeResultName { get; set; }
        public Nullable<int> AuditManagementResult { get; set; }
        public string AuditManagementResultName { get; set; }
        public Nullable<int> AuditLeaderResult { get; set; }
        public string AuditLeaderResultName { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }        
    }
}

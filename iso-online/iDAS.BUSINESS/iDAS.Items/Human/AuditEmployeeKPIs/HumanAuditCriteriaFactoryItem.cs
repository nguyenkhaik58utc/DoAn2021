using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanAuditCriteriaFactoryItem
    {
        public int ID { get; set; }
        public int HumanRoleID { get; set; }
        public string HumanRoleName { get; set; }
        public int HumanAuditCriteriaID { get; set; }
        public string HumanAuditCriteriaName { get; set; }
        public string HumanAuditCriteriaGroupName { get; set; }
        public Nullable<int> Factory { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class HumanAuditGradationCriteriaItem
    {
        public int ID { get; set; }
        public int HumanAuditGradationRoleID { get; set; }
        public string HumanAuditCriteriaCategoryName { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int Factory { get; set; }
        public bool InsertToCate { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public List<HumanAuditGradationCriteriaPointItem> Answer { get; set; }
    }
}

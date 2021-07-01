using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanAuditTickResultBonusScoreItem
    {
        public int ID { get; set; }
        public int AuditTickResultID { get; set; }
        public int Point { get; set; }
        public string CT
        {
            get;
            set;
        }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public virtual HumanAuditTickItem HumanAuditTickResult { get; set; }
    }
}

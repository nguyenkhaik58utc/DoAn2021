using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
  public class RiskAuditItem
    {
        public int ID { get; set; }
        public int RiskControlID { get; set; }
        public bool IsAccept { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public Nullable<int> QualityNCID { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ActionForm { get; set; }
    }
}

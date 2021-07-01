using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
  public  class CenterAuditAnswerItem
    {
        public int ID { get; set; }
        public Nullable<int> CenterAuditQuestionID { get; set; }
        public string Answer { get; set; }
        public string CreateAnswer { get; set; }
        public string UpdateAnswer { get; set; }
        public bool IsTrue { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

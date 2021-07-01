using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
  public  class HumanResultQuestionItem
    {
        public int ID { get; set; }
        public int HumanEmployeeAuditID { get; set; }
        public int HumanQuestionID { get; set; }
        public string Name { get; set; }
        public string ResultName { get; set; }
        public bool IsResult { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

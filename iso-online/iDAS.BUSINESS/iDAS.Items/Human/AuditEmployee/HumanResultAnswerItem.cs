using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
  public  class HumanResultAnswerItem
    {
        public int ID { get; set; }
        public int HumanResultQuestionID { get; set; }
        public int HumanAnswerID { get; set; }
        public string Answer { get; set; }
        public bool IsSelected { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

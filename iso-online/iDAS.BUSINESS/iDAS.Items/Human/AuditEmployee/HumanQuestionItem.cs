using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanQuestionItem
    {
        public int ID { get; set; }
        public Nullable<int> HumanCategoryQuestionID { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Note { get; set; }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool IsMulti { get; set; }
        public List<HumanAnswerItem> Answer { get; set; }
    }
}

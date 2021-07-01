using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class TopicItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url
        {
            get
            {
                return "/Topic/Index/" + ID.ToString();
            }
        }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
    }
}

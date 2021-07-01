using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerFeedbackItem
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }

        public string ActionForm { get; set; }
    }
}
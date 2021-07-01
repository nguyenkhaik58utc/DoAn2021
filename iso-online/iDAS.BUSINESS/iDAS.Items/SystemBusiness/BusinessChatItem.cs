using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class BusinessChatItem
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public BusinessChatEmployeeItem Sender { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public Nullable<System.DateTime> ReadTime { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

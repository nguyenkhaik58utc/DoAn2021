using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class BusinessChatEmployeeItem : HumanEmployeeViewItem
    {
        public string Display { get; set; }
        public string Content { get; set; }
        public int NumberNotRead { get; set; }
        public string TotalNotRead
        {
            get { 
                return NumberNotRead > 0 ? "(" + NumberNotRead.ToString() + ")" : string.Empty;
            } 
        }
        public string Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string Time
        {
            get
            {
                var value = CreateAt ?? DateTime.Now;
                return value.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }
    }
}

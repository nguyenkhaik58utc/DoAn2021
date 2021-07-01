using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace iDAS.Items
{
    public class CustomerSystemModuleItem
    {
        public int ID { get; set; }
        public int CustomerSystemID { get; set; }
        public int CenterModuleID { get; set; }
        public string CenterModuleName { get; set; }
        public bool IsSelect { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    }
}

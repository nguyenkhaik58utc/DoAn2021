using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class PermissionItem
    {
        public int ID { get; set; }
        public int ActionID { get; set; }
        public int RoleID { get; set; }
        public bool IsEnable { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}

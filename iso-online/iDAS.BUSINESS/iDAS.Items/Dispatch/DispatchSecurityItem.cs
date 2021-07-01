using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class DispatchSecurityItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Color { get; set; }
        public string Note { get; set; }

        public int? CreateBy { get; set; }//Người đính kèm
        public string CreateName { get; set; }//
        public DateTime? CreateAt { get; set; }//Thời gian đính kèm

        public string UpdateName { get; set; }
        public int? UpdateBy { get; set; }//Thời gian xóa
        public DateTime? UpdateAt { get; set; }//Thời gian xóa

        public bool IsActive { get; set; }
    }
}

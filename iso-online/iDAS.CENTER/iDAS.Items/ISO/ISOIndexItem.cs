using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace iDAS.Items
{
    public class ISOClauseItem
    {
        public int ID { get; set; }
        public int IsoID { get; set; }
        public string IsoCode { get; set; }//Ma tieu chuan Iso
        public string IsoName { get; set; }//Ten tieu chuan iso
        public int ModuleID { get; set; }
        public string Module { get; set; }
        public string Index { get; set; }//ten module
        public string Name { get; set; }//ten module
        
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string Note { get; set; }//ten module
        public DateTime? UpdateAt { get; set; }
        public DateTime? CreateAt { get; set; }
        public string UpdateName { get; set; }
        public int UpdateBy { get; set; }
        public int? CreateBy { get; set; }

        public int ParentID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace iDAS.Items
{
    public class ISOStandardModuleItem
    {
        public int ID { get; set; }
        public int IsoNaceID { get; set; }
        public int ModuleID { get; set; }
        public string Code { get; set; }
        public string Module { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public bool IsShow { get; set; }
        public Decimal? Sum { get; set; }
        public bool IsChoose { get; set; }
        public DateTime UpdateAt { get; set; }
        public string UpdateName { get; set; }
        public int UpdateBy { get; set; }
    }
}

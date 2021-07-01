using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CenterISOClauseItem
    {
        public int ID { get; set; }       
        public int ISOStandardID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ListModuleName { get; set; }
        public string Clause { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsSelected { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

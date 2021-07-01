using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class QualityCriteriaCategoryItem
    {
        public int ID { get; set; }
        public int ISOStandartID { get; set; }
        public string Name { get; set; }
        public bool IsUse { get; set; }
        public bool IsTerm { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public HumanEmployeeViewItem Audit { get; set; }

        public bool IsProduction { get; set; }
    }
    public class TreeCriteriaCategoryItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Note { get; set; }
        public bool Leaf { get; set; }
        public bool IsSelected { get; set; }
    }
}

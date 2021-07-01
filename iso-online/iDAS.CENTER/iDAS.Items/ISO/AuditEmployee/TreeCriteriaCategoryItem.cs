using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
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

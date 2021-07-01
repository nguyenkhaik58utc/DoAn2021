using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class LinkCategoryItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsHorizon { get; set; }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public int Position { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    }
}

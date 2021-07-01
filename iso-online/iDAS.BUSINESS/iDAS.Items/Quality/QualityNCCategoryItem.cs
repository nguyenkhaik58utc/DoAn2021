using System;

namespace iDAS.Items
{
    public class QualityNCCategoryItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public HumanEmployeeViewItem CheckBy { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    }
}

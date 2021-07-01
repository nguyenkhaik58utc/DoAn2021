using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class QualityCriteriaItem
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public int MinPoint { get; set; }
        public int MaxPoint { get; set; }
        public decimal Factory { get; set; }
        public decimal AvgPoint { 
            get {
                return (MinPoint + MaxPoint) * Factory / 2;
            }
            set { }
        }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

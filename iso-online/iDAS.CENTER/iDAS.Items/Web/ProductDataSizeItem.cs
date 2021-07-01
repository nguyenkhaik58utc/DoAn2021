using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductDataSizeItem
    {
        public int ID { get; set; }
        public string Name { 
            get {
                return Size +" "+ Unit;
            } 
        }
        public Nullable<int> Size { get; set; }
        public string Unit { get; set; }
        public float UseTime { get; set; }
        public string Note { get; set; }
        public bool IsUpgrade { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public decimal Price { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

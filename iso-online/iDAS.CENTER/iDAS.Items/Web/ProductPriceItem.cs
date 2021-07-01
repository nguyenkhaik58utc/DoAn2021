using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductPriceItem
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int ProductMethodID { get; set; }
        public string ProductMethodName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

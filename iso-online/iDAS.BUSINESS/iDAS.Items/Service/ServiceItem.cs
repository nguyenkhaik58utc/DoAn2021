using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
  public  class ServiceItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }

        public bool IsSelected { get; set; }
    }
}

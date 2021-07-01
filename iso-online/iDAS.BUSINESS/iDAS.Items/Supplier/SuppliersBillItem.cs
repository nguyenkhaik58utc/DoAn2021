using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class SuppliersBillItem
    {
        public int ID { get; set; }
        public int SuppliersOrderID { get; set; }
        public Nullable<decimal> PayedMoney { get; set; }
        public Nullable<System.DateTime> PayDate { get; set; }
        public Nullable<byte> BillType { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string Note { get; set; }

        public virtual SuppliersOrderItem SuppliersOrder { get; set; }
    }
}

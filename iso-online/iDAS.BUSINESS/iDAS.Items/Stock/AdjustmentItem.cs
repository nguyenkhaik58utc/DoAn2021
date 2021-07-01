using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class AdjustmentItem
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
        public string Ref_OrgNo { get; set; }
        public Nullable<int> RefType { get; set; }
        public Nullable<int> Employee_ID { get; set; }
        public Nullable<int> StockID { get; set; }
        public string Stock_Name { get; set; }
        public string Employee_Name { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public bool Accept { get; set; }
        public bool IsClose { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public System.DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

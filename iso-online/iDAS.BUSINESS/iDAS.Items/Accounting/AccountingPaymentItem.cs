using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class AccountingPaymentItem
    {
        public int ID { get; set; }
        public int CustomerContractID { get; set; }
        public string Content { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> Value { get; set; }
        public bool IsCustomer { get; set; }
        public Nullable<decimal> TotalContract { get; set; }
        public string TypeName
        {
            get
            {
                return IsCustomer ? "Đề xuất thanh toán của khách hàng" : "Đề xuất thanh toán của kế toán";
            }
        }
        public bool IsDelete { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string ActionForm { get; set; }
    }
}

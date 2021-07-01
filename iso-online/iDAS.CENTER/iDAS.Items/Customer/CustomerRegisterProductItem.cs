using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerRegisterProductItem
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Guid? CustomerFileId { get; set; }
        public int SystemID { get; set; }
        public int WebProductID { get; set; }
        public string ProductName { get; set; }
        public int WebProductMethodID { get; set; }
        public string ProductMethodName { get; set; }
        public int WebProductScopeID { get; set; }
        public string ProductScopeName { get; set; }
        public int WebProductDateSizeID { get; set; }
        public string ProductDatesizeName { get; set; }
        public bool IsNew { get; set; }
        public bool IsContact { get; set; }
        public bool IsAccept { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> RegisterAt { get; set; }
        public Nullable<System.DateTime> ContactAt { get; set; }
        public Nullable<System.DateTime> CompleteAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    }
}

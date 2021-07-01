using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class SuppliersBidOrderItem
    {
        public int ID { get; set; }
        public int SuppliersOrderID { get; set; }
        public int SupplierID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> ReceiveDate { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<bool> Status { get; set; }
        public string StatusDisp
        {
            get { return getStatus(); }
            set { }
        }

        private string getStatus()
        {
            string result = "";
            bool stt = Status.HasValue ? Status.Value : false;
            if(stt)
                result = "<span style=\"color:green\">Đã nhận</span>"; 
            else
                result = "<span style=\"color:red\">Chưa nhận</span>"; 
            return result;
        }
        public AttachmentFileItem AttachmentFiles { get; set; }
        public string Contents { get; set; }
        
        public virtual SupplierItem Supplier { get; set; }
        public virtual SuppliersOrderItem SuppliersOrder { get; set; }
        //public virtual ICollection<SupplierBidToAttachmentFileItem> SupplierBidToAttachmentFiles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    class SupplierBidToAttachmentFileItem
    {
        public int ID { get; set; }
        public int SupplierBidOrderID { get; set; }
        public System.Guid AttachmentFileID { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }

        public virtual AttachmentFileItem AttachmentFile { get; set; }
        public virtual SuppliersBidOrderItem SuppliersBidOrder { get; set; }
    }
}

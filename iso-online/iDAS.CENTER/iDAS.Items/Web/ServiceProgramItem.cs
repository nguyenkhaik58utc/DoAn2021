using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ServiceProgramItem
    {
        public int ID { get; set; }
        public int ServiceID { get; set; }
        public Nullable<System.Guid> FileID { get; set; }
        public AttachmentFileItem Image { get; set; }
        public string Name { get; set; }
        public string Html { get; set; }
        public string Tags { get; set; }
        public string Address { get; set; }
        public string Duration { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Note { get; set; }
        public bool IsFirst { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> StartAt { get; set; }
        public Nullable<System.DateTime> EndAt { get; set; }
        public Nullable<System.DateTime> RefreshAt { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

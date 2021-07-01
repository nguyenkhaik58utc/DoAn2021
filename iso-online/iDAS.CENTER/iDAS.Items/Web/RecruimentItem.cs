using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class RecruimentItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public Nullable<System.Guid> FileID { get; set; }
        public AttachmentFileItem Image { get; set; } 
        public string Html { get; set; }
        public string Role { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMail { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public Nullable<int> TotalView { get; set; }
        public int Quatity { get; set; }
        public bool IsFirst { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> FinishAt { get; set; }
        public Nullable<System.DateTime> RefreshAt { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerContactItem
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public Nullable<System.Guid> AttachmentFileID { get; set; }
        public string FileName { get; set; }
        public FileItem ImageFile { get; set; }
        public string Note { get; set; }
        public string ActionForm { get; set; }
    }
}
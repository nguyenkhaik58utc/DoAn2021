using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
  public  class ISOStandardInformationItem
    {
        public int ID { get; set; }
        public int ISOStandardID { get; set; }
        public int SystemID { get; set; }
        public string SystemName { get; set; }
        public string ISOStandardName { get; set; }
        public string ISOCode { get; set; }
        public string Name { get; set; }
        public bool IsShow { get; set; }
        public bool IsDelete { get; set; }
        public int Order { get; set; }
        public Nullable<System.Guid> FileID { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

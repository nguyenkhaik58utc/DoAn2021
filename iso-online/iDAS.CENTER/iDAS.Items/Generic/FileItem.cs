using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class FileItem
    {
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public double Size { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string TypeIcon { get; set; }
    }
}

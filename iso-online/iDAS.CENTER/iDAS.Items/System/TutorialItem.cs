using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class TutorialItem
    {
        public int ID { get; set; }
        public string SystemCode { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string FunctionCode { get; set; }
        public string FunctionName { get; set; }
        public FileItem AttachFile { get; set; }
        public Nullable<System.Guid> FileID { get; set; }
        public string FileName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    }
}

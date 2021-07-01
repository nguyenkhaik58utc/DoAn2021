using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class TemplateExportItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Font { get; set; }
        public Nullable<int> FontSize { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Guid? FileID { get; set; }
        public FileItem File { get; set; }
        public string FileName { get; set; }
        public string FunctionCode { get; set; }
        public string ModuleCode { get; set; }
        public Nullable<bool> Default { get; set; }
        public AttachmentFileItem AttachmentFile { get; set; }
        public List<Guid> AttachmentFileIDs
        {
            get
            {
                List<Guid> lst = new List<Guid>();
                if (FileID.HasValue) lst.Add(FileID.Value);
                return lst;
            }
            
        }
    }
    public class TemplateExportFieldItem
    {
        public int ID { get; set; }
        public int TemplateExportID { get; set; }
        public string Name { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public Nullable<int> Postition { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
    public class TemplateExportFieldStyleItem
    {
        public int ID { get; set; }
        public int TemplateExportFieldID { get; set; }
        public int StyleIndex { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
   
}

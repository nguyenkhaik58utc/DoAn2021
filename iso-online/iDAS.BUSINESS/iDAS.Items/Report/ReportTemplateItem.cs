using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ReportTemplateItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string File { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public Nullable<double> Size { get; set; }
        public bool IsMap { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public AttachmentFileItem FileUpload { get; set; }
        public DateTime? UpdateView {
            get {
                return UpdateAt ?? CreateAt;
            }
        }
        public string FileView {
            get {
                return File.Remove(File.LastIndexOf('.'));
            }
        }
        public string TypeView { 
            get{
                return File.Split('.').LastOrDefault();
            }
        }
        public string SizeView {
            get {
                return (Size/1024).Value.ToString("0") + " KB";
            }
        }
        public string ModuleCode { get; set; }
        public string FunctionCode { get; set; }
    }
}

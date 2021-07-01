using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class QualityAuditEmployeeItem
    {
        public int ID { get; set; }
        public int AuditProgramID { get; set; }
        public int EmployeeID { get; set; }
        public bool IsCaptain { get; set; }
        public bool IsAuditor { get; set; }
        public bool IsAbsent { get; set; }
        public string AbsentReason { get; set; }
        public string EmployeeName { get; set; }
        public string Role { get; set; }       
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public FileItem ImageFile { get; set; }    
        public string ImageUrl
        {
            get
            {
                var url = "data:image;base64,{0}";
                if (ImageFile == null || ImageFile.Data == null || Convert.ToBase64String(ImageFile.Data) == "") return "";
                var data = Convert.ToBase64String(ImageFile.Data);
                url = string.Format(url, data);
                return url;
            }
        }
        public Guid? FileID { get; set; }
        public string FileName { get; set; }
       
    }
}

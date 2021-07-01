using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanRoleItem
    {
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        [Display(Name = "Tên chức danh")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Name { get; set; }
        [Display(Name = "Kích hoạt")]
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool IsSelected { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public string DepartmentName { get; set; }
        public int AuditCriteRialFactiory { get; set; }
        public string Resposibility { get; set; }
        public string Authorize { get; set; }
        public string Competence { get; set; }
        public string ReportTo { get; set; }
        public string ReplaceBy { get; set; }
        public string Note { get; set; }
        public int? Position { get; set; }

        public string UpdateAtView {
            get
            {
                if (UpdateAt.HasValue)
                    return UpdateAt.Value.ToString("dd-MM-yyyy");
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}

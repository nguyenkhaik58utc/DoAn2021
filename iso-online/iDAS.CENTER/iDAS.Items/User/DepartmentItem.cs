using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iDAS.Items
{
    public class DepartmentItem
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int ParentID { get; set; }
        [Display(Name = "Tên nhóm")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Name { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string CreateBy { get; set; }
        [HiddenInput(DisplayValue = false)]
        public DateTime? CreateAt { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string UpdateBy { get; set; }
        [HiddenInput(DisplayValue = false)]
        public DateTime? UpdateAt { get; set; }
    }
}

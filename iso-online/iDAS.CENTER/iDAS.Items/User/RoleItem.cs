using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iDAS.Items
{
    public class RoleItem
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        [Display(Name = "Tên chức danh")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Name { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int? CreateBy { get; set; }
        [HiddenInput(DisplayValue = false)]
        public DateTime? CreateAt { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int? UpdateBy { get; set; }
        [HiddenInput(DisplayValue = false)]
        public DateTime? UpdateAt { get; set; }
        [HiddenInput(DisplayValue = false)]
        public bool IsSelected { get; set; }
    }
}

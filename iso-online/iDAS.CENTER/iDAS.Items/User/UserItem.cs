using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iDAS.Items
{
    public class UserItem
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Name { get; set; }
        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Account { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Xác nhận mật khẩu không đúng!")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Không đúng định dạng email như support@iDASvietnam.com")]
        public string Email { get; set; }
        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
        [HiddenInput(DisplayValue = false)]
        public DateTime? ActiveAt { get; set; }
        [HiddenInput(DisplayValue = false)]
        public DateTime? DeactivateAt { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int? InfoID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int? CreateBy { get; set; }
        [HiddenInput(DisplayValue = false)]
        public DateTime? CreateAt { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int? UpdateBy { get; set; }
        [HiddenInput(DisplayValue = false)]
        public DateTime? UpdateAt { get; set; }
    }
}

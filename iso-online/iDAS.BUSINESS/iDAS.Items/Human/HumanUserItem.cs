using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;

namespace iDAS.Items
{
    public class HumanUserItem
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
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
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public bool IsChangePass { get; set; }
        public bool IsProtected { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }


        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu mới!")]
        [DataType(DataType.Password)]
        public string PasswordNew { get; set; }
        [Display(Name = "Xác nhận mật khẩu mới")]
        [Required(ErrorMessage = "Yêu cầu nhập lại mật khẩu mới!")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("PasswordNew", ErrorMessage = "Xác nhận mật khẩu mới không đúng!")]
        public string ConfirmPasswordNew { get; set; }
    }
}

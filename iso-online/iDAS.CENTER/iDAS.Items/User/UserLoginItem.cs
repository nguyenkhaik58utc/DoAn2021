using iDAS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iDAS.Items
{
    public class UserLoginItem : IUserLogin
    {
        //[Display(Name = "Account", ResourceType = typeof(Resources.Resource))]
        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Account { get; set; }

        //[Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Display(Name = "Code", ResourceType = typeof(Resources.Resource))]
        public string Code { get; set; }

        //[Display(Name = "Languague", ResourceType = typeof(Resources.Resource))]
        [Display(Name = "Ngôn ngữ")]
        public string Languague { get; set; }

        private bool remember = true;
        [Display(Name = "Nhớ tài khoản")]
        public bool Remember
        {
            get
            {
                return remember;
            }
            set
            {
                remember = value;
            }
        }

        [Display(Name = "Captcha")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        [CaptchaValid(ErrorMessage = "Giá trị nhập không đúng!")]
        public string Captcha { get; set; }
    }
}

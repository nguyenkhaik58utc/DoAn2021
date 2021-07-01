using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;

namespace iDAS.Items
{
    public class BusinessUserLoginItem : IUserLogin
    {
        private string _Password;
        //[Display(Name = "Account", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        [Display(Name = "Tài khoản")]
        public string Account { get; set; }

        //[Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        [DataType(DataType.Password)]
        public string Password { 
            get {
                return _Password;
            }
            set {
                _Password = Encryptor.Encrypt(value);
            }
        }

        [Display(Name = "Mã iDAS cấp")]
        //[Display(Name = "Code", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Code { get; set; }

        [Display(Name = "Ngôn ngữ")]
        //[Display(Name = "Languague", ResourceType = typeof(Resources.Resource))]
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

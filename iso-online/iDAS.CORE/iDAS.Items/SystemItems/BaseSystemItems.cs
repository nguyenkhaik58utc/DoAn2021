using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using iDAS.Core;
namespace iDAS.Items
{
    public class SystemCultureItem {
        public string Text { get; set; }
        public string Culture { get; set; }
        public string NeutralCulture { get; set; }
    }

    public class SystemCustomerItem 
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Guid GUID { get; set; }

        [Display(Name = "NameCustomer", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Name { get; set; }

        [Display(Name = "Company", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Company { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Không đúng định dạng email như support@iDASvietnam.com")]
        public string Email { get; set; }

        [Display(Name = "Phone", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Phone { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Address { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Code { get; set; }

        [Display(Name = "Logo", ResourceType = typeof(Resources.Resource))]
        [HiddenInput(DisplayValue = false)]
        public string Logo { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string DatabaseName { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string DatabaseDataSource { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string DatabaseUserID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string DatabasePassword { get; set; }

        [Display(Name = "Active", ResourceType = typeof(Resources.Resource))]
        public bool IsActive { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<System.DateTime> ActiveAt { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Nullable<System.DateTime> CreateAt { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Nullable<int> CreateBy { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Nullable<System.DateTime> UpdateAt { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Nullable<int> UpdateBy { get; set; }
    }

    /// <summary>
    /// Module Menu Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class ModuleMenuItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsShow { get; set; }
    }

    /// <summary>
    /// Function Menu Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class FunctionMenuItem : ModuleMenuItem
    {
        public string ModuleCode { get; set; }
        public string ParentCode { get; set; }
        public bool IsGroup { get; set; }
        public string Url { get; set; }
    }

    /// <summary>
    /// Action Menu Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class ActionMenuItem : FunctionMenuItem
    {
        public string FunctionCode { get; set; }
        public string Url { get; set; }
    }

    /// <summary>
    /// Permission System Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class SystemPermissionItem
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        public int ActionID { get; set; }
        public int RoleID { get; set; }
        public bool IsEnable { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
    }

    /// <summary>
    /// Organization System Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class SystemOrganizationItem 
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
    }

    /// <summary>
    /// User Register System Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class SystemUserRegisterItem
    {
        [Display(Name="Họ tên")]
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
        [System.ComponentModel.DataAnnotations.Compare("Password",ErrorMessage="Xác nhận mật khẩu không đúng!")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage="Yêu cầu nhập!")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Không đúng định dạng email như support@iDASvietnam.com")]
        public string Email { get; set; }
        [Display(Name = "Mã công ty")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Code { get; set; }
    }

    /// <summary>
    /// User Login System Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class SystemUserLoginItem : IUserLogin
    {
        [Display(Name = "Account", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Account { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Resources.Resource))]
        //[Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Code { get; set; }

        [Display(Name = "Languague", ResourceType = typeof(Resources.Resource))]
        public string Languague { get; set; }

        private bool remember = true;
        [Display(Name = "Nhớ tài khoản")]
        public bool Remember { 
            get {
                return remember;
            } 
            set {
                remember = value;
            } 
        }

        [Display(Name = "Captcha")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        [CaptchaValid(ErrorMessage = "Giá trị nhập không đúng!")]
        public string Captcha { get; set; }

        public bool IsCustomer
        {
            get {
                return false;
            }
            set { }
        }
    }

    /// <summary>
    /// User System Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class SystemUserItem 
    {
        [HiddenInput(DisplayValue=false)]
        public int ID { get; set; }
        [Display(Name="Họ tên")]
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
        [System.ComponentModel.DataAnnotations.Compare("Password",ErrorMessage="Xác nhận mật khẩu không đúng!")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage="Yêu cầu nhập!")]
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

    /// <summary>
    /// Group System Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class SystemGroupItem 
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int ParentID { get; set; }
        [Display(Name = "Tên nhóm")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Name { get; set; }
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

    /// <summary>
    /// Role System Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class SystemRoleItem
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int GroupID { get; set; }
        [Display(Name = "Tên chức danh")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Name { get; set; }
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

    /// <summary>
    /// Icon System Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class SystemIconItem { 
        public string IconName {get;set;}
        public string IconUrl { get; set; }
    }

    /// <summary>
    /// Base System Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class BaseSystemItem : SystemIconItem
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public bool IsShow { get; set; }
        public byte? Status { get; set; }
        public string StatusView { get; set; }
        public int Position { get; set; }
        public string Description { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }
        public string Icon { get; set; }
    }

    /// <summary>
    /// Module System Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class SystemModuleItem : BaseSystemItem,IModule
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Function System Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class SystemFunctionItem : BaseSystemItem, IFunction
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string ParentCode { get; set; }
        public bool IsGroup { get; set; }
        public string Url { get; set; }
        public Type Type { get; set; }
    }

    /// <summary>
    /// Action System Item
    /// Author: TungNT
    /// Date: 2/05/2014
    /// </summary>
    public class SystemActionItem : BaseSystemItem, IAction
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FunctionCode { get; set; }
        public string FunctionName { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string Url { get; set; }
        public string Permission { get; set; }
        [HiddenInput(DisplayValue = false)]
        public bool IsEnable { get; set; }
    }
}
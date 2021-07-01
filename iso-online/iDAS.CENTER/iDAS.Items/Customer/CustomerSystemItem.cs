using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iDAS.Items
{
    public class CustomerSystemItem
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public Nullable<Guid> FileID { get; set; }
        public int SystemID { get; set; }
        public bool IsNew { get; set; }
        [Display(Name = "NameCustomer", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
        public int? DataSize { get; set; }
        public int? DataSizePlus { get; set; }
        public int? DataSizeUsing { get; set; }
        [Display(Name = "Company", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Company { get; set; }
        [Display(Name = "Code", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Code { get; set; }
        [Display(Name = "Logo", ResourceType = typeof(Resources.Resource))]
        [HiddenInput(DisplayValue = false)]
        public byte[] Logo { get; set; }//LogoUrl
        public string LogoUrl
        {
            get
            {
                if (ID != 0)
                {
                    if (IsUpdate) return "/Customer/CustomerSystem/LoadLogoByID?customerId=" + ID.ToString()+"&IsUpdate=true";
                    return "/Customer/CustomerSystem/LoadLogoByID?customerId=" + ID.ToString();
                }
                return "../../Content/images/iDAS-logo.png";
            }
        }
        public bool IsUpdate { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string DBName { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string DBSource { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string DBUserID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string DBPassword { get; set; }
        [Display(Name = "Active", ResourceType = typeof(Resources.Resource))]
        public bool IsActive { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Nullable<System.DateTime> CreateAt { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Nullable<int> CreateBy { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Nullable<System.DateTime> UpdateAt { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Nullable<int> UpdateBy { get; set; }

        #region Thông tin xem chi tiết
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime? AcceptAt { get; set; }

        public string AcceptBy { get; set; }

        public DateTime? ActiveAt { get; set; }

        public string ActiveBy { get; set; }
        #endregion
    }
}

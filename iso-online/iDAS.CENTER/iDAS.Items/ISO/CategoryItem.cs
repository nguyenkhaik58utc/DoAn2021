using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using iDAS.Core;

namespace iDAS.Items
{
    public class CategoryItem
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
               
        //[Display(Name = "NameLicense", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
        public string SystemCode { get; set; }
        public string SystemName { get; set; }
     
        //[Display(Name = "abc")]
        //[Required(ErrorMessage = "Yêu cầu nhập!")]
        public bool IsActive { get; set; }

        //[Display(Name = "abc")]
        //[Required(ErrorMessage = "Yêu cầu nhập!")]
        public bool IsShow { get; set; }

        //[Display(Name = "Description", ResourceType = typeof(Resources.Resource))]
        //[Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Icon { get; set; }
        public string PhotoBanne { get; set; }
     

        //[Display(Name = "Description", ResourceType = typeof(Resources.Resource))]
        //[Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Description { get; set; }

        //[Display(Name = "CreateAt", ResourceType = typeof(Resources.Resource))]
        public Nullable<System.DateTime> CreateAt { get; set; }

        //[Display(Name = "CreateName", ResourceType = typeof(Resources.Resource))] 
        public string CreateName { get; set; }

        //[Display(Name = "UpdateAt", ResourceType = typeof(Resources.Resource))]
        public Nullable<System.DateTime> UpdateAt { get; set; }

        //[Display(Name = "UpdateName", ResourceType = typeof(Resources.Resource))]      
        public string UpdateName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CreateBy { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int UpdateBy { get; set; }
    }
}

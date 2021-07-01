using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace iDAS.Items
{
    public class ISOStandardItem
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        public int IsoID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string UrlAvatar { get; set; }
        public bool IsActive { get; set; }
        public bool IsAnnex { get; set; }
        public DateTime? CreateAt { get; set; }
        public string CreateName { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string UpdateName { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int CreateBy { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int UpdateBy { get; set; }
        public bool IsSelected { get; set; }
    }
    public class SystemPriceItem
    {
        public int ID { get; set; }
        public string SystemName { get; set; }
        public string ISOName { get; set; }
        public string FormName { get; set; }
        public decimal Price { get; set; }
    }
}

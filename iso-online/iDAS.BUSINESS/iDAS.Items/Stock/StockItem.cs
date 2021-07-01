using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class StockItem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Mobi { get; set; }
        public string Manager { get; set; }
        public byte[] ImageMap { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public System.DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ImageUrl
        {
            get
            {
                var url = "data:image;base64,{0}";
                if (ImageMap == null) return "/Content/images/underfind.jpg";
                var data = Convert.ToBase64String(ImageMap);
                url = string.Format(url, data);
                return url;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iDAS.Items
{
    public class ConfigItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace iDAS.Items
{
    public class ISOIndexCriteriaItem
    {
        public int ID { get; set; }
        public int ISOIndexID { get; set; }
        public string Name { get; set; }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}

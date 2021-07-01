using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace iDAS.Items
{
    public class ISOControlItem
    {
        public int ID { get; set; }
        public int ISOStandardID { get; set; }
        public string Clause { get; set; }
        public string Target { get; set; }
        public string Control { get; set; }
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string ActionForm { get; set; }
        public string ISOName { get; set; }
    }
}

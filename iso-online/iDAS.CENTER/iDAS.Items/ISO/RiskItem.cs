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
    public class RiskItem
    {
        public int ID { get; set; }
        public int CenterRiskCategoryID { get; set; }
        public string RiskCategoryName { get; set; }
        public string Name { get; set; }
        public string Weakness { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }

        public string ActionForm { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;

namespace iDAS.Items
{
    public class HumanProfileCertificateItem
    {
        public int ID { get; set; }
        public string Name{get;set;}
        public string Type { get; set; }
        public string Level { get; set; }
        public Nullable<System.DateTime> DateIssuance { get; set; }
        public Nullable<System.DateTime> DateExpiration { get; set; }
        public string PlaceOfTraining { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public int EmployeeID { get; set; }
    }
}

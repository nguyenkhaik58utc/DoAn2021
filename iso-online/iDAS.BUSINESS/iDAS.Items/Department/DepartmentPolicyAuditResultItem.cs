using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class DepartmentPolicyAuditResultItem
    {
        public int ID { get; set; }
        public int DepartmentPolicyID { get; set; }
        public int HumanDepartmentID { get; set; }
        public bool IsPass { get; set; }
        public string ResultName { get; set; }
        public Nullable<System.DateTime> AuditAt { get; set; }
        public HumanEmployeeViewItem HumanEmployee { get; set; }
        public string Evidence { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }

        public string PolicyName { get; set; }

        public int TotalFail { get; set; }

        public string ISOName { get; set; }

        public string ActionForm { get; set; }
        public Nullable<int> QualityNCID { get; set; }
        public string AuditNote { get; set; }
        public bool IsObs{get;set;}
        public bool IsMedium{get;set;}
        public bool IsMaximum{get;set;}

        public int? ISOID { get; set; }

        public bool IsUse { get; set; }
    }
}

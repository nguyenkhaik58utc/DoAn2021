using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class QualityAuditResultItem
    {
        public int ID { get; set; }
        public int QualityCAPARequirementID { get; set; }
        public int QualityAuditProgramISOID { get; set; }
        public Nullable<int> QualityNCID { get; set; }
        public int ISOIndexCriteriaID { get; set; }
        public int DepartmentID { get; set; }
        public string ISOIndexCriteriaName { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<bool> IsPass { get; set; }
        public int Index { get; set; }
        public Nullable<int> AuditBy { get; set; }
        public Nullable<System.DateTime> AuditAt { get; set; }
        public string AuditNote { get; set; }
        public HumanEmployeeViewItem Create { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool IsObs { get; set; }
        public bool IsMaximum { get; set; }
        public bool IsMedium { get; set; }
        public bool ResultPass { get; set; }
        public bool ResultNotPass { get; set; }
        public bool IsHasCAPARequirement
        {
            get;
            set;
        }
        public bool IsCorrertive
        {
            get;
            set;
        }
       
    }
}

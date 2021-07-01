using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class QualityAuditProgramISOIndexItem
    {
        public int ID { get; set; }
        public string ProgramName { get; set; }
        public int QualityAuditProgramID { get; set; }
        public int ISOIndexID { get; set; }
        public string IsoIndexCode { get; set; }
        public string IsoIndexName { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool IsPass { get; set; }
        public bool IsAudit { get; set; }
        public Nullable<int> AuditBy { get; set; }
        public Nullable<System.DateTime> AuditAt { get; set; }
        public string AuditNote { get; set; }
        public int Obs { get; set; }
        public int Maximum { get; set; }
        public int Medium { get; set; }
        public string ListModuleName { get; set; }
        public int DepartmentID { get; set; }

        public bool IsObs { get; set; }

        public bool IsMaximum { get; set; }

        public string DepartmentName { get; set; }

        public bool IsMedium { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class QualityAuditRecordedVoteItem
    {
       public int? QualityAuditProgramISOID;
        public int ID { get; set; }
        public int QualityAuditProgramID { get; set; }
        public int HumanDepartmentID { get; set; }
        public string HumanDepartmentName { get; set; }
        public int AuditBy { get; set; }
        public Nullable<System.DateTime> AuditAt { get; set; }
        public string AuditName { get; set; }
        public string Contents { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public HumanEmployeeViewItem Auditer { get; set; }

        public string QualityAuditProgram { get; set; }

        public bool IsMaximum { get; set; }

        public bool IsMedium { get; set; }

        public bool IsObs { get; set; }

        public int? ISOIndexID { get; set; }

        public string ModuleCode { get; set; }

        public string FunctionCode { get; set; }

        public bool IsComplete { get; set; }

        public string ISOIndexName { get; set; }

        public string ModuleName { get; set; }

        public string FunctionName { get; set; }
        public TypeModel Enums
        {
            get
            {
                var enums = new TypeModel();
                if (IsObs) enums.Type = EType.Obs;
                if (IsMaximum) enums.Type = EType.M;
                if (IsMedium) enums.Type = EType.m_;
                return enums;
            }
            set
            {
                switch (value.Type)
                {
                    case EType.Obs: IsObs = true; break;
                    case EType.m_: IsMedium = true; break;
                    case EType.M: IsMaximum = true; break;
                }
            }
        }
    }
}

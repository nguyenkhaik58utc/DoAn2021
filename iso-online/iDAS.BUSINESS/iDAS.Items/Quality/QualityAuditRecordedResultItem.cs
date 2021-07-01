using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class QualityAuditRecordedResultItem
    {
        public bool IsComplete;
        public int ID { get; set; }
        public int QualityAuditProgamID { get; set; }
        public int QualityAuditRecordedVoteID { get; set; }
        public int? ISOIndexID { get; set; }
        public int ModuleID { get; set; }              
        public int FunctionID { get; set; }
        public string ModuleCode { get; set; }
        public string FunctionCode { get; set; }
        public bool IsObs { get; set; }
        public bool IsMaximum { get; set; }
        public bool IsMedium { get; set; }
        public string AuditNote { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
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
        public string ModuleName { get; set; }
        public string FunctionName { get; set; }
        public string ISOIndexName { get; set; }
        public bool IsVerify { get; set; }

        public string HumanDepartmentName { get; set; }

        public int HumanDepartmentID { get; set; }

        public string AuditName { get; set; }

        public DateTime? AuditAt { get; set; }
    }
}

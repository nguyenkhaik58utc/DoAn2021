using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public enum EType
    {
        Obs = 0,
        m_ = 1,
        M = 2
    }
    public class TypeModel
    {
        public EType Type { get; set; }
    }

    public class QualityNCItem
    {
        public int ID { get; set; }
        public HumanDepartmentViewItem Department { get; set; }
        public string CategoryName { get; set; }
        public AuditCriteriaViewItem Criteria { get; set; }
        public string Source { get; set; }
        public System.DateTime Time { get; set; }
        public string Content { get; set; }
        public string LisEmployeeID { get; set; }
        public string LisRoleID { get; set; }
        public bool IsMaximum { get; set; }
        public bool IsMedium { get; set; }
        public bool IsObs { get; set; }
        public bool IsVerify { get; set; }
        public bool IsTrue { get; set; }
        public bool IsCompleteAuto { get; set; }
        public bool IsComplete { get; set; }
        public HumanEmployeeViewItem CheckBy { get; set; }
        public Nullable<System.DateTime> CheckAt { get; set; }
        public string CheckNote { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string Status
        {
            get
            {
                string result = "";
                result = IsVerify ?
                    (IsTrue ? "<span style=\"color:blue\">Duyệt</span>" : "<span style=\"color:#41519f\">Không duyệt</span>")
                    : "<span style=\"color:green\">Mới</span>"; 
                return result;
            }
            set { }
        }
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

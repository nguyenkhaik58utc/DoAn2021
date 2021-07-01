using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class RiskLegalItem
    {
        public int ID { get; set; }
        public int RiskID { get; set; }
        public int DepartmentLegalID { get; set; }
        public string DepartmentLegalName { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string RiskName { get; set; }
        public string RiskWeakness { get; set; }
        public HumanEmployeeViewItem RiskOwner { get; set; }
        public float LikeLiHood { get; set; }
        public float Impact { get; set; }
        public float Consequence { get; set; }
        public bool IsSend { get; set; }
        public int RiskLevelID { get; set; }
    }
}

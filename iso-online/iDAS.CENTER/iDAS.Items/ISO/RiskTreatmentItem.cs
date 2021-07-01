using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class RiskTreatmentItem
    {
        public int ID { get; set; }
        public int CenterRiskID { get; set; }
        public int CenterRiskMethodID { get; set; }
        public string Level { get; set; }
        public string Color { get; set; }
        public string Method { get; set; }
        public string Action { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string ActionForm { get; set; }
    }
}

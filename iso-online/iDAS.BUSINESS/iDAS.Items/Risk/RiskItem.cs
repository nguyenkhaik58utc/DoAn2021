using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class RiskItem
    {
        public int ID { get; set; }
        public int HumanDepartmentID { get; set; }
        public int RiskCategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Weakness { get; set; }
        public float LikeLiHood { get; set; }
        public float Impact { get; set; }
        public float Consequence { get; set; }
        public int HumanEmployeeID { get; set; }
        public bool IsFromLegal { get; set; }
        public int LegalID { get; set; }
        public string LegalName { get; set; }
        public bool IsFromRegulatory { get; set; }
        public int RegulatoryID { get; set; }
        public string RegulatoryName { get; set; }
        public bool IsFromRequire { get; set; }
        public int ContractID { get; set; }
        public string ContractCode { get; set; }
        public bool IsFromAction { get; set; }
        public bool IsSend { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public HumanDepartmentViewItem Department { get; set; }
        public HumanEmployeeViewItem RiskOwner { get; set; }
        public string ActionForm { get; set; }
        public float RiskValue { get; set; }
        public int RiskLevelID { get; set; }
        public int RiskTreatmentID { get; set; }
        public string ListRiskTreatmentID { get; set; }
        public bool IsNew { get; set; }
        public string Action { get; set; }
    }
}

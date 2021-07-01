using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Human.Models
{
    public class V3_HumanProfileWorkTrialDTO
    {
        public int HumanEmployeeID { get; set; }
        public string RoleName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsApproval { get; set; }
        public bool IsAccept { get; set; }
        public bool IsDelete { get; set; }
        public int ApprovalBy { get; set; }
        public DateTime ApprovalAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpadateAt { get; set; }
        public int DirectorApproval { get; set; }
        public DateTime DirectorApprovalAt { get; set; }
        public string ApprovalNote { get; set; }
        public string ContractTypeName { get; set; }
        public DateTime ContractStartTime { get; set; }
        public string QualityCriterialName { get; set; }
        public int EmployeePoint { get; set; }
        public int ManagerPoint { get; set; }
    }
}

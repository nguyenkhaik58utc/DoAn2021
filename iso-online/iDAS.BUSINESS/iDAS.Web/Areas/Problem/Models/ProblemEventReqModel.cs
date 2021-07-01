using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Problem.Models
{
    public class ProblemEventReqModel
    {
        public int? PageSize { get; set; } = 10;
        public int? PageNumber { get; set; } = 1;
        public int? ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? EmergencyTypeID { get; set; }
        public int? ProblemTypeID { get; set; }
        public int? ProblemGroupID { get; set; }
        public int? CriticalLevelID { get; set; }
        public DateTime? OccuredDateFrom { get; set; }
        public DateTime? OccuredDateTo { get; set; }
        public int? StatusID { get; set; }
        public bool? IsProblemOrEvent { get; set; }
        public int? Reporter { get; set; }
        public int? Receiver { get; set; }
        public bool? IsTemplate { get; set; }
        public int? DepartmentIdFix { get; set; }
    }
}
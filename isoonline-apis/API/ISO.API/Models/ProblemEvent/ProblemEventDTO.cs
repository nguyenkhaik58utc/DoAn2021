using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class ProblemEventDTO
    {
        public int? ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? EmergencyTypeID { get; set; }
        public string ProblemEmergencyName { get; set; }
        public int? ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public int? CriticalLevelID { get; set; }
        public string CriticalLevelName { get; set; }
        public int? ProblemGroupID { get; set; }
        public string ProblemGroupName { get; set; }
        public DateTime? OccuredDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public string Propose { get; set; }
        public string Solution { get; set; }
        public string Result { get; set; }
        public int? StatusID { get; set; }
        public string ProblemStatusName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsProblemOrEvent { get; set; }
        public bool? IsDelete { get; set; }
        public string Reporter { get; set; }
        public string ReporterDepartment { get; set; }
        //public string ReporterName { get; set; }
        public int? Receiver { get; set; }
        public string ReceiverName { get; set; }
        public string ContactNumber { get; set; }
        public string ReporterEmail { get; set; }
        public bool? IsTemplate { get; set; }
        public int? RequestDepID { get; set; }
        public int? ResidentAgencyID { get; set; }
        public int? ResidentAgencyGroupID { get; set; }
        public string RequestDepName { get; set; }
        public string RelateDep { get; set; }
        public string Resolvers { get; set; }
        public bool? YourselfFix { get; set; }
        public bool? OnDuty { get; set; }
        public string? lng { get; set; }
        public string? lat { get; set; }
    }
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
        public string Reporter { get; set; }
        public int? Receiver { get; set; }
        public bool? IsTemplate { get; set; }
        public int? DepartmentIdFix { get; set; }
        public string? lng { get; set; }
        public string? lat { get; set; }
    }
}

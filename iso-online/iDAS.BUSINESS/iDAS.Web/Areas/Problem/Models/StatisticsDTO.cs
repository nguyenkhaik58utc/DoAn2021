using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace iDAS.Web.Areas.Problem.Models
{
    public class StatisticsDTO
    {

    }
    public class ProblemEventUserReportDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int DepartmentID { get; set; }
        public string DepName { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
    }
    public class ProblemEventUserReportReqModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    public class ProblemEventUserReportResModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int DepartmentID { get; set; }
        public string DepName { get; set; }
        public List<ProblemType> ProblemTypes { get; set; }
    }
    public class ProblemType
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public int EventCount { get; set; }
    }
    public class CriticalLevel
    {
        public int CriticalLevelID { get; set; }
        public string CriticalLevelName { get; set; }
        public int EventCount { get; set; }
    }
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }

    public class ProblemTypeReportDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public int DepartmentID { get; set; }
        public string DepName { get; set; }
    }
    public class ProblemTypeReqModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    public class ProblemTypeResModel
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public List<Dep> Departments { get; set; }
    }
    public class CriticalTypeReportDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CriticalLevelID { get; set; }
        public string CriticalLevelName { get; set; }
        public int DepartmentID { get; set; }
        public string DepName { get; set; }
    }
    public class CriticalTypeReqModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    public class CriticalTypeResModel
    {
        public int CriticalLevelID { get; set; }
        public string CriticalLevelName { get; set; }
        public List<Dep> Departments { get; set; }
    }

    public class ProblemTypeEventRequestDepResModel
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public List<EventReqDep> EventRequestDep { get; set; }
    }

    public class ProblemTypeEventResidentAgencyResModel
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public List<ResidentAgency> ResidentAgencies { get; set; }
    }

    public class ProblemCriticalEventRequestDepResModel
    {
        public int ProblemCriticalLevelID { get; set; }
        public string ProblemCriticalLevelName { get; set; }
        public List<EventReqDep> EventRequestDep { get; set; }
    }

    public class DepartmentEventResidentAgencyResModel
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public List<ResidentAgency> ResidentAgencies { get; set; }
    }

    public class Dep
    {
        public int DepartmentID { get; set; }
        public string DepName { get; set; }
        public int EventCount { get; set; }
    }

    public class EventReqDep
    {
        public int EventRequestDepID { get; set; }
        public string EventRequestDepName { get; set; }
        public int EventCount { get; set; }
    }

    public class ResidentAgency
    {
        public int ResidentAgencyID { get; set; }
        public string ResidentAgencyName { get; set; }
        public int EventCount { get; set; }
    }

    public class ProblemEventReport_ByDepartmentTypeDTO
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
    public class ProblemEventReport_ByDepartmentCriticalDTO
    {
        public int CriticalLevelID { get; set; }
        public string CriticalLevelName { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
    public class ProblemEventReport_ByDepartment
    {
        public List<ProblemEventReport_ByDepartmentTypeDTO> problemEventReport_ByDepartmentTypeDTOs { get; set; }
        public List<ProblemEventReport_ByDepartmentCriticalDTO> problemEventReport_ByDepartmentCriticalDTOs { get; set; }
    }

    public class ProblemEventReport_GetByUserDTO
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public int CriticalLevelID { get; set; }
        public string CriticalLevelName { get; set; }
        public int Count { get; set; }
    }

    public class ProblemEventReport_GetByUserReqModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int UserID { get; set; }
    }
    public class ProblemEventReport_GetByUserDetailReqModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int ProblemTypeID { get; set; }
        public int CriticalID { get; set; }
        public int UserID { get; set; }
    }

    public class ProblemOnDutyResModel
    {
        public bool OnDuty { get; set; }
        public string OnDutyName { get { if (OnDuty) return "Trực điều độ tiếp nhận"; else return "Hỗ trợ trực tiếp"; } }
        public List<Dep> Departments { get; set; }
    }

}

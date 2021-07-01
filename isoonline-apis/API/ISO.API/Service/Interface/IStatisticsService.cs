using ISO.API.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IStatisticsService
    {
        List<ProblemTypeReportDTO> ProblemTypeReport(DateTime from, DateTime to);
        List<ProblemEventDTO> ProblemTypeReport_Detail(out int totalRows, ProblemTypeReportDetailReqModel model);
        List<ProblemEventDTO> CriticalTypeReport_Detail(out int totalRows, CriticalTypeReportDetailReqModel model);
        List<ProblemEventDTO> ProblemTypeReportEventRequestDep_Detail(out int totalRows, ProblemTypeReportEventRequestDepDetailReqModel model);
        List<ProblemEventDTO> CriticalTypeReportEventRequestDep_Detail(out int totalRows, CriticalTypeReportEventRequestDepDetailReqModel model);
        List<CriticalTypeReportDTO> CriticalTypeReport(DateTime from, DateTime to);
        List<ProblemEventUserReportDTO> ProblemEventUser_ReportByUser(DateTime from, DateTime to);
        ProblemEventReport_ByDepartment ProblemEventReport_ByDepartment(DateTime from, DateTime to, int departmentID);
        List<ProblemEventDTO> ProblemEventReport_ByDepDetail(out int totalRows, ProblemEventReport_ByDepDetailReqModel model);
        List<ProblemEventReport_GetByUserDTO> ProblemEventReport_GetByUser(ProblemEventReport_GetByUserReqModel model);
        List<ProblemEventDTO> ProblemEventReport_GetByUserDetail(out int totalRows, ProblemEventReport_GetByUserDetailReqModel model);
        List<ProblemEventReport_ByEventRequestDepCriticalDTO> CriticalTypeReportEventRequest(DateTime from, DateTime to);
        List<ProblemEventReport_ByEventRequestDepTypeDTO> ProblemTypeReportEventRequest(DateTime from, DateTime to);
        List<ProblemEventReport_ByResidentAgencyDepartmentDTO> DepartmentReportResidentAgency(DateTime from, DateTime to);
        List<ProblemEventReport_ByResidentAgencyTypeDTO> ProblemTypeReportResidentAgency(DateTime from, DateTime to);
        List<ProblemEventDTO> ProblemTypeReportResidentAgency_Detail(out int totalRows, ProblemTypeReportResidentAgencyDetailReqModel model);
        List<ProblemEventDTO> DepartmentReportResidentAgency_Detail(out int totalRows, DepartmentReportResidentAgencyDetailReqModel model);
        List<ProblemOnDutyReportDTO> ProblemOnDutyReport(DateTime from, DateTime to);
        List<ProblemEventDTO> ProblemOnDutyReport_Detail(out int totalRows, ProblemOnDutyReportDetailReqModel model);
    }
}

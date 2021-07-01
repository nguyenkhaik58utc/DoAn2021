using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public class StatisticsService : IStatisticsService
    {
        public List<CriticalTypeReportDTO> CriticalTypeReport(DateTime from, DateTime to)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { From = from, To = to };
            List<CriticalTypeReportDTO> criticalTypeReportDTOs = exec.GetByField<CriticalTypeReportDTO>("sp_CriticalTypeReport", param, commandType: CommandType.StoredProcedure);
            return criticalTypeReportDTOs;
        }

        public List<ProblemEventReport_ByEventRequestDepCriticalDTO> CriticalTypeReportEventRequest(DateTime from, DateTime to)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { From = from, To = to };
            List<ProblemEventReport_ByEventRequestDepCriticalDTO> criticalTypeReportDTOs = exec.GetByField<ProblemEventReport_ByEventRequestDepCriticalDTO>("sp_CriticalTypeReportEventRequest", param, commandType: CommandType.StoredProcedure);
            return criticalTypeReportDTOs;
        }

        public List<ProblemEventDTO> CriticalTypeReportEventRequestDep_Detail(out int totalRows, CriticalTypeReportEventRequestDepDetailReqModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@From", model.From, DbType.DateTime);
            param.Add("@To", model.To, DbType.DateTime);
            param.Add("@CriticalID", model.CriticalID, DbType.Int32);
            param.Add("@EventRequestDepID", model.EventRequestDepID, DbType.Int32);
            param.Add("@PageSize", model.PageSize, DbType.Int32);
            param.Add("@PageNumber", model.PageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemEventDTO> problemEventDTOs = exec.GetPaging<ProblemEventDTO>(out totalRows, "sp_CriticalTypeReportEventRequestDep_Detail", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemEventDTOs;
        }

        public List<ProblemEventDTO> CriticalTypeReport_Detail(out int totalRows, CriticalTypeReportDetailReqModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@From", model.From, DbType.DateTime);
            param.Add("@To", model.To, DbType.DateTime);
            param.Add("@CriticalID", model.CriticalID, DbType.Int32);
            param.Add("@DepartmentID", model.DepartmentID, DbType.Int32);
            param.Add("@PageSize", model.PageSize, DbType.Int32);
            param.Add("@PageNumber", model.PageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemEventDTO> problemEventDTOs = exec.GetPaging<ProblemEventDTO>(out totalRows, "sp_CriticalTypeReport_Detail", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemEventDTOs;
        }

        public List<ProblemEventReport_ByResidentAgencyDepartmentDTO> DepartmentReportResidentAgency(DateTime from, DateTime to)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { From = from, To = to };
            List<ProblemEventReport_ByResidentAgencyDepartmentDTO> departmentReportDTOs = exec.GetByField<ProblemEventReport_ByResidentAgencyDepartmentDTO>("sp_DepartmentReportByResidentAgency", param, commandType: CommandType.StoredProcedure);
            return departmentReportDTOs;
        }

        public List<ProblemEventDTO> DepartmentReportResidentAgency_Detail(out int totalRows, DepartmentReportResidentAgencyDetailReqModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@From", model.From, DbType.DateTime);
            param.Add("@To", model.To, DbType.DateTime);
            param.Add("@ResidentAgencyID", model.ResidentAgencyID, DbType.Int32);
            param.Add("@DepartmentID", model.DepartmentID, DbType.Int32);
            param.Add("@PageSize", model.PageSize, DbType.Int32);
            param.Add("@PageNumber", model.PageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemEventDTO> problemEventDTOs = exec.GetPaging<ProblemEventDTO>(out totalRows, "sp_DepartmentReportResidentAgency_Detail", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemEventDTOs;
        }

        public ProblemEventReport_ByDepartment ProblemEventReport_ByDepartment(DateTime from, DateTime to, int departmentID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { From = from, To = to, DepartmentID = departmentID };
            var multi = exec.QueryMulti("sp_ProblemEventReport_ByDepartment", param, commandType: CommandType.StoredProcedure);
            var problemEventReport_ByDepartmentTypeDTO = multi.Read<ProblemEventReport_ByDepartmentTypeDTO>();
            var problemEventReport_ByDepartmentCriticalDTO = multi.Read<ProblemEventReport_ByDepartmentCriticalDTO>();
            ProblemEventReport_ByDepartment model = new ProblemEventReport_ByDepartment()
            {
                problemEventReport_ByDepartmentCriticalDTOs = problemEventReport_ByDepartmentCriticalDTO.ToList(),
                problemEventReport_ByDepartmentTypeDTOs = problemEventReport_ByDepartmentTypeDTO.ToList()
            };
            return model;
        }

        public List<ProblemEventDTO> ProblemEventReport_ByDepDetail(out int totalRows, ProblemEventReport_ByDepDetailReqModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@From", model.From, DbType.DateTime);
            param.Add("@To", model.To, DbType.DateTime);
            param.Add("@ProblemTypeID", model.ProblemTypeID, DbType.Int32);
            param.Add("@CriticalID", model.CriticalID, DbType.Int32);
            param.Add("@UserID", model.UserID, DbType.Int32);
            param.Add("@DepartmentID", model.DepartmentID, DbType.Int32);
            param.Add("@PageSize", model.PageSize, DbType.Int32);
            param.Add("@PageNumber", model.PageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemEventDTO> problemEventDTOs = exec.GetPaging<ProblemEventDTO>(out totalRows, "sp_ProblemEventReport_ByDepDetail", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemEventDTOs;
        }


        public List<ProblemEventReport_GetByUserDTO> ProblemEventReport_GetByUser(ProblemEventReport_GetByUserReqModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { From = model.From, To = model.To, UserID = model.UserID };
            List<ProblemEventReport_GetByUserDTO> data = exec.GetByField<ProblemEventReport_GetByUserDTO>("sp_ProblemEventReport_GetByUser", param, commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<ProblemEventDTO> ProblemEventReport_GetByUserDetail(out int totalRows, ProblemEventReport_GetByUserDetailReqModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@From", model.From, DbType.DateTime);
            param.Add("@To", model.To, DbType.DateTime);
            param.Add("@ProblemTypeID", model.ProblemTypeID, DbType.Int32);
            param.Add("@CriticalID", model.CriticalID, DbType.Int32);
            param.Add("@UserID", model.UserID, DbType.Int32);
            param.Add("@PageSize", model.PageSize, DbType.Int32);
            param.Add("@PageNumber", model.PageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemEventDTO> problemEventDTOs = exec.GetPaging<ProblemEventDTO>(out totalRows, "sp_ProblemEventReport_GetByUserDetail", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemEventDTOs;
        }

        public List<ProblemEventUserReportDTO> ProblemEventUser_ReportByUser(DateTime from, DateTime to)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { From = from, To = to };
            List<ProblemEventUserReportDTO> problemEventUserReportDTOs = exec.GetByField<ProblemEventUserReportDTO>("sp_ProblemEventUser_ReportByUser", param, commandType: CommandType.StoredProcedure);
            return problemEventUserReportDTOs;
        }

        public List<ProblemTypeReportDTO> ProblemTypeReport(DateTime from, DateTime to)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { From = from, To = to };
            List<ProblemTypeReportDTO> problemTypeReportDTOs = exec.GetByField<ProblemTypeReportDTO>("sp_ProblemTypeReport", param, commandType: CommandType.StoredProcedure);
            return problemTypeReportDTOs;
        }

        public List<ProblemEventReport_ByEventRequestDepTypeDTO> ProblemTypeReportEventRequest(DateTime from, DateTime to)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { From = from, To = to };
            List<ProblemEventReport_ByEventRequestDepTypeDTO> criticalTypeReportDTOs = exec.GetByField<ProblemEventReport_ByEventRequestDepTypeDTO>("sp_ProblemTypeReportEventRequest", param, commandType: CommandType.StoredProcedure);
            return criticalTypeReportDTOs;
        }

        public List<ProblemEventDTO> ProblemTypeReportEventRequestDep_Detail(out int totalRows, ProblemTypeReportEventRequestDepDetailReqModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@From", model.From, DbType.DateTime);
            param.Add("@To", model.To, DbType.DateTime);
            param.Add("@ProblemTypeID", model.ProblemTypeID, DbType.Int32);
            param.Add("@EventRequestDepID", model.EventRequestDepID, DbType.Int32);
            param.Add("@PageSize", model.PageSize, DbType.Int32);
            param.Add("@PageNumber", model.PageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemEventDTO> problemEventDTOs = exec.GetPaging<ProblemEventDTO>(out totalRows, "sp_ProblemTypeReportEventRequestDep_Detail", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemEventDTOs;
        }

        public List<ProblemEventReport_ByResidentAgencyTypeDTO> ProblemTypeReportResidentAgency(DateTime from, DateTime to)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { From = from, To = to };
            List<ProblemEventReport_ByResidentAgencyTypeDTO> problemTypeReportDTOs = exec.GetByField<ProblemEventReport_ByResidentAgencyTypeDTO>("sp_ProblemTypeReportResidentAgency", param, commandType: CommandType.StoredProcedure);
            return problemTypeReportDTOs;
        }

        public List<ProblemEventDTO> ProblemTypeReportResidentAgency_Detail(out int totalRows, ProblemTypeReportResidentAgencyDetailReqModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@From", model.From, DbType.DateTime);
            param.Add("@To", model.To, DbType.DateTime);
            param.Add("@ProblemTypeID", model.ProblemTypeID, DbType.Int32);
            param.Add("@ResidentAgencyID", model.ResidentAgencyID, DbType.Int32);
            param.Add("@PageSize", model.PageSize, DbType.Int32);
            param.Add("@PageNumber", model.PageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemEventDTO> problemEventDTOs = exec.GetPaging<ProblemEventDTO>(out totalRows, "sp_ProblemTypeReportResidentAgency_Detail", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemEventDTOs;
        }

        public List<ProblemEventDTO> ProblemTypeReport_Detail(out int totalRows, ProblemTypeReportDetailReqModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@From", model.From, DbType.DateTime);
            param.Add("@To", model.To, DbType.DateTime);
            param.Add("@ProblemTypeID", model.ProblemTypeID, DbType.Int32);
            param.Add("@DepartmentID", model.DepartmentID, DbType.Int32);
            param.Add("@PageSize", model.PageSize, DbType.Int32);
            param.Add("@PageNumber", model.PageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemEventDTO> problemEventDTOs = exec.GetPaging<ProblemEventDTO>(out totalRows, "sp_ProblemTypeReport_Detail", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemEventDTOs;
        }

        public List<ProblemOnDutyReportDTO> ProblemOnDutyReport(DateTime from, DateTime to)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { From = from, To = to };
            List<ProblemOnDutyReportDTO> problemTypeReportDTOs = exec.GetByField<ProblemOnDutyReportDTO>("sp_ProblemOnDutyReport", param, commandType: CommandType.StoredProcedure);
            return problemTypeReportDTOs;
        }
        public List<ProblemEventDTO> ProblemOnDutyReport_Detail(out int totalRows, ProblemOnDutyReportDetailReqModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@From", model.From, DbType.DateTime);
            param.Add("@To", model.To, DbType.DateTime);
            param.Add("@OnDuty", model.OnDuty, DbType.Boolean);
            param.Add("@DepartmentID", model.DepartmentID, DbType.Int32);
            param.Add("@PageSize", model.PageSize, DbType.Int32);
            param.Add("@PageNumber", model.PageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemEventDTO> problemEventDTOs = exec.GetPaging<ProblemEventDTO>(out totalRows, "sp_ProblemOnDutyReport_Detail", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemEventDTOs;
        }
    }
}

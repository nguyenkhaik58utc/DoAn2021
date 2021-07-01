using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public class ProblemEventService : IProblemEventService
    {
        public int Delete(int problemEventID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = problemEventID };
            int rs = exec.ExcuteScalar("sp_V3ProblemEvent_Delete", param);
            return rs;
        }

        public ProblemEventDTO GetByID(int problemEventID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = problemEventID };
            ProblemEventDTO problemEventDTO = exec.GetByField<ProblemEventDTO>("sp_V3ProblemEvent_GetByID", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return problemEventDTO;
        }

        public List<ProblemEventDTO> GetPaging(out int totalRows, ProblemEventReqModel problemEventReqModel)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", problemEventReqModel.Code, DbType.String);
            param.Add("@Name", problemEventReqModel.Name, DbType.String);
            param.Add("@ProblemGroupID", problemEventReqModel.ProblemGroupID, DbType.Int32);
            param.Add("@ProblemTypeID", problemEventReqModel.ProblemTypeID, DbType.Int32);
            param.Add("@EmergencyTypeID", problemEventReqModel.EmergencyTypeID, DbType.Int32);
            param.Add("@CriticalLevelID", problemEventReqModel.CriticalLevelID, DbType.Int32);
            param.Add("@IsProblemOrEvent", problemEventReqModel.IsProblemOrEvent, DbType.Boolean);
            param.Add("@IsTemplate", problemEventReqModel.IsTemplate, DbType.Boolean);
            param.Add("@OccuredDateFrom", problemEventReqModel.OccuredDateFrom, DbType.DateTime);
            param.Add("@OccuredDateTo", problemEventReqModel.OccuredDateTo, DbType.DateTime);
            param.Add("@Reporter", problemEventReqModel.Reporter, DbType.String);
            param.Add("@Receiver", problemEventReqModel.Receiver, DbType.Int32);
            param.Add("@StatusID", problemEventReqModel.StatusID, DbType.Int32);
            param.Add("@DepartmentIdFix", problemEventReqModel.DepartmentIdFix, DbType.Int32);
            param.Add("@PageSize", problemEventReqModel.PageSize, DbType.Int32);
            param.Add("@PageNumber", problemEventReqModel.PageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemEventDTO> problemEventDTOs = exec.GetPaging<ProblemEventDTO>(out totalRows, "sp_V3ProblemEvent_GetPaging", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemEventDTOs;
        }

        public int InsertUpdate(ProblemEventDTO problemEventDTO)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();

            param.Add("@ID", problemEventDTO.ID, DbType.Int32);
            param.Add("@Code", problemEventDTO.Code, DbType.String);
            param.Add("@Name", problemEventDTO.Name, DbType.String);
            //param.Add("@OccuredDepartmentID", problemEventDTO.OccuredDepartmentID, DbType.Int32);
            //param.Add("@ResolvedDepartmentID", problemEventDTO.ResolvedDepartmentID, DbType.Int32);
            param.Add("@EmergencyTypeID", problemEventDTO.EmergencyTypeID, DbType.Int32);
            param.Add("@ProblemTypeID", problemEventDTO.ProblemTypeID, DbType.Int32);
            param.Add("@CriticalLevelID", problemEventDTO.CriticalLevelID, DbType.Int32);
            param.Add("@ProblemGroupID", problemEventDTO.ProblemGroupID, DbType.Int32);
            param.Add("@OccuredDate", problemEventDTO.OccuredDate, DbType.DateTime);
            param.Add("@ResolvedDate", problemEventDTO.ResolvedDate, DbType.DateTime);
            param.Add("@Description", problemEventDTO.Description, DbType.String);
            param.Add("@Reason", problemEventDTO.Reason, DbType.String);
            param.Add("@Propose", problemEventDTO.Propose, DbType.String);
            param.Add("@Solution", problemEventDTO.Solution, DbType.String);
            param.Add("@Result", problemEventDTO.Result, DbType.String);
            param.Add("@StatusID", problemEventDTO.StatusID, DbType.Int32);
            param.Add("@CreatedAt", problemEventDTO.CreatedAt, DbType.DateTime);
            param.Add("@CreatedBy", problemEventDTO.CreatedBy, DbType.Int32);
            param.Add("@UpdatedAt", problemEventDTO.UpdatedAt, DbType.DateTime);
            param.Add("@UpdatedBy", problemEventDTO.UpdatedBy, DbType.Int32);
            param.Add("@IsProblemOrEvent", problemEventDTO.IsProblemOrEvent, DbType.Boolean);
            param.Add("@IsDelete", problemEventDTO.IsDelete, DbType.Boolean);
            param.Add("@Reporter", problemEventDTO.Reporter, DbType.String);
            param.Add("@ReporterDepartment", problemEventDTO.ReporterDepartment, DbType.String);
            param.Add("@Receiver", problemEventDTO.Receiver, DbType.Int32);
            param.Add("@ContactNumber", problemEventDTO.ContactNumber, DbType.String);
            param.Add("@ReporterEmail", problemEventDTO.ReporterEmail, DbType.String);
            param.Add("@IsTemplate", problemEventDTO.IsTemplate, DbType.Boolean);
            param.Add("@RequestDepID", problemEventDTO.RequestDepID, DbType.Int32);
            param.Add("@ResidentAgencyID", problemEventDTO.ResidentAgencyID, DbType.Int32);
            param.Add("@ResidentAgencyGroupID", problemEventDTO.ResidentAgencyGroupID, DbType.Int32);
            param.Add("@YourselfFix", problemEventDTO.YourselfFix, DbType.Boolean);
            param.Add("@OnDuty", problemEventDTO.OnDuty, DbType.Boolean);
            param.Add("@lng", problemEventDTO.lng, DbType.String);
            param.Add("@lat", problemEventDTO.lat, DbType.String);

            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var rs = exec.ExcuteReturnValue("sp_V3ProblemEvent_InsUpd", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return rs;
        }
    }
}

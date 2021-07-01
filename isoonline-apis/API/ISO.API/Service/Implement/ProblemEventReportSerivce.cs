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
    public class ProblemEventReportSerivce : IProblemEventReportService
    {
        public int Delete(ProblemEventReportDeleteModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = model.ID, UpdatedAt = model.UpdatedAt, UpdatedBy = model.UpdatedBy };
            int rs = exec.ExcuteScalar("sp_V3ProblemEventReport_Delete", param);
            return rs;
        }

        public ProblemEventReportDTO GetByID(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = id };
            ProblemEventReportDTO problemEventReportDTO = exec.GetByField<ProblemEventReportDTO>("sp_V3ProblemEventReport_GetByID", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return problemEventReportDTO;
        }

        public List<ProblemEventReportDTO> GetByProblemEventID(int id)
        {
            throw new NotImplementedException();
        }

        public List<ProblemEventReportDTO> GetPaging(out int totalRows, ProblemEventReportSearchModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Name", model.Name, DbType.String);
            param.Add("@Reporter", model.Reporter, DbType.Int32);
            param.Add("@ProblemEventID", model.ProblemEventID, DbType.Int32);
            param.Add("@Status", model.Status, DbType.Int32);
            param.Add("@PageSize", model.PageSize, DbType.Int32);
            param.Add("@PageNumber", model.PageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemEventReportDTO> problemEventDTOs = exec.GetPaging<ProblemEventReportDTO>(out totalRows, "sp_V3ProblemEventReport_GetPaging", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemEventDTOs;
        }

        public int InsertUpdate(ProblemEventReportInsModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@ID", model.ID, DbType.Int32);
            param.Add("@ProblemEventID", model.ProblemEventID, DbType.Int32);
            param.Add("@Name", model.Name, DbType.String);
            param.Add("@Content", model.Content, DbType.String);
            param.Add("@From", model.From, DbType.DateTime);
            param.Add("@To", model.To, DbType.DateTime);
            param.Add("@Reporter", model.Reporter, DbType.Int32);
            param.Add("@DepartmentID", model.DepartmentID, DbType.Int32);
            param.Add("@ReportDate", model.ReportDate, DbType.DateTime);
            param.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            param.Add("@Status", model.Status, DbType.Int32);
            param.Add("@Percent", model.Percent, DbType.Double);
            param.Add("@CreatedAt", model.CreatedAt, DbType.DateTime);
            param.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            param.Add("@UpdatedAt", model.UpdatedAt, DbType.DateTime);
            param.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);
            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var rs = exec.ExcuteReturnValue("sp_V3ProblemEventReport_InsUpd", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return rs;
        }
    }
}

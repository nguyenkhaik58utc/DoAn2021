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
    public class ProblemResidentAgencyService : IProblemResidentAgencyService
    {

        public int Delete(int problemResidentAgencyID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = problemResidentAgencyID };
            int rs = exec.ExcuteScalar("sp_V3ProblemResidentAgency_Delete", param);
            return rs;
        }

        public ProblemResidentAgencyDTO GetByID(int problemResidentAgencyID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = problemResidentAgencyID };
            ProblemResidentAgencyDTO problemResidentAgency = exec.GetByField<ProblemResidentAgencyDTO>("sp_V3ProblemResidentAgency_GetByID", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return problemResidentAgency;
        }

        public List<ProblemResidentAgencyDTO> GetPaging(out int totalRows, ProblemResidentAgencyReqModel agencyReqModel)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Name", agencyReqModel.Name, DbType.String);
            param.Add("@PageSize", agencyReqModel.PageSize, DbType.String);
            param.Add("@PageNumber", agencyReqModel.PageNumber, DbType.String);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemResidentAgencyDTO> problemResidentAgencyGroups = exec.GetPaging<ProblemResidentAgencyDTO>(out totalRows, "sp_V3ProblemResidentAgency_GetPaging", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemResidentAgencyGroups;
        }
        public List<ProblemResidentAgencyDTO> GetByGroupID(int groupID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@GroupID", groupID, DbType.Int32);
            List<ProblemResidentAgencyDTO> problemResidentAgencyGroups = exec.GetByField<ProblemResidentAgencyDTO>("sp_V3ProblemResidentAgency_GetByGroupID", param, commandType: CommandType.StoredProcedure);
            return problemResidentAgencyGroups;
        }

        public int InsertUpdate(ProblemResidentAgencyDTO problemResidentAgency)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@CreatedAt", problemResidentAgency.CreatedAt, DbType.DateTime);
            param.Add("@CreatedBy", problemResidentAgency.CreatedBy, DbType.Int32);
            param.Add("@Description", problemResidentAgency.Description, DbType.String);
            param.Add("@ID", problemResidentAgency.ID, DbType.Int32);
            param.Add("@ResidentAgencyGroupID", problemResidentAgency.ResidentAgencyGroupID, DbType.Int32);
            param.Add("@IsDelete", problemResidentAgency.IsDelete, DbType.Boolean);
            param.Add("@Name", problemResidentAgency.Name, DbType.String);
            param.Add("@UpdatedAt", problemResidentAgency.UpdatedAt, DbType.DateTime);
            param.Add("@UpdatedBy", problemResidentAgency.UpdatedBy, DbType.Int32);
            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var rs = exec.ExcuteReturnValue("sp_V3ProblemResidentAgency_InsUpd", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return rs;
        }
    }
}

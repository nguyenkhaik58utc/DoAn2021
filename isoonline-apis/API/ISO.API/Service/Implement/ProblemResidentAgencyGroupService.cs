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
    public class ProblemResidentAgencyGroupService : IProblemResidentAgencyGroupService
    {

        public int Delete(int problemResidentAgencyGroupID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = problemResidentAgencyGroupID };
            int rs = exec.ExcuteScalar("sp_V3ProblemResidentAgencyGroup_Delete", param);
            return rs;
        }

        public ProblemResidentAgencyGroupDTO GetByID(int problemResidentAgencyGroupID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = problemResidentAgencyGroupID };
            ProblemResidentAgencyGroupDTO problemResidentAgencyGroup = exec.GetByField<ProblemResidentAgencyGroupDTO>("sp_V3ProblemResidentAgencyGroup_GetByID", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return problemResidentAgencyGroup;
        }

        public List<ProblemResidentAgencyGroupDTO> GetPaging(out int totalRows, ProblemResidentAgencyGroupReqModel agencyGroupReqModel)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Name", agencyGroupReqModel.Name, DbType.String);
            param.Add("@PageSize", agencyGroupReqModel.PageSize, DbType.String);
            param.Add("@PageNumber", agencyGroupReqModel.PageNumber, DbType.String);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemResidentAgencyGroupDTO> problemResidentAgencyGroups = exec.GetPaging<ProblemResidentAgencyGroupDTO>(out totalRows, "sp_V3ProblemResidentAgencyGroup_GetPaging", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemResidentAgencyGroups;
        }

        public int InsertUpdate(ProblemResidentAgencyGroupDTO problemResidentAgencyGroup)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@CreatedAt", problemResidentAgencyGroup.CreatedAt, DbType.DateTime);
            param.Add("@CreatedBy", problemResidentAgencyGroup.CreatedBy, DbType.Int32);
            param.Add("@Description", problemResidentAgencyGroup.Description, DbType.String);
            param.Add("@ID", problemResidentAgencyGroup.ID, DbType.Int32);
            param.Add("@IsDelete", problemResidentAgencyGroup.IsDelete, DbType.Boolean);
            param.Add("@Name", problemResidentAgencyGroup.Name, DbType.String);
            param.Add("@UpdatedAt", problemResidentAgencyGroup.UpdatedAt, DbType.DateTime);
            param.Add("@UpdatedBy", problemResidentAgencyGroup.UpdatedBy, DbType.Int32);
            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var rs = exec.ExcuteReturnValue("sp_V3ProblemResidentAgencyGroup_InsUpd", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return rs;
        }
    }
}

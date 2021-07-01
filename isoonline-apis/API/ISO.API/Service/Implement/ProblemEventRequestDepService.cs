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
    public class ProblemEventRequestDepService : IProblemEventRequestDepService
    {
        public int Delete(ProblemEventRequestDepDelModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = model.ID, UpdatedAt = model.UpdatedAt, UpdatedBy = model.UpdatedBy };
            int rs = exec.ExcuteScalar("sp_V3ProblemEventRequestDep_Delete", param);
            return rs;
        }

        public ProblemEventRequestDepDTO GetByID(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = id };
            ProblemEventRequestDepDTO model = exec.GetByField<ProblemEventRequestDepDTO>("sp_V3ProblemEventRequestDep_GetByID", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return model;
        }

        public List<ProblemEventRequestDepDTO> GetPaging(out int totalRows, ProblemEventRequestDepSearchModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Name", model.Name, DbType.String);
            param.Add("@PageSize", model.PageSize, DbType.String);
            param.Add("@PageNumber", model.PageNumber, DbType.String);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);

            List<ProblemEventRequestDepDTO> data = exec.GetPaging<ProblemEventRequestDepDTO>(out totalRows, "sp_V3ProblemEventRequestDep_GetPaging", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return data;
        }

        public int InsertUpdate(ProblemEventRequestDepInsUpdModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@CreatedAt", model.CreatedAt, DbType.DateTime);
            param.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            param.Add("@Description", model.Description, DbType.String);
            param.Add("@ID", model.ID, DbType.Int32);
            //param.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            param.Add("@Name", model.Name, DbType.String);
            param.Add("@UpdatedAt", model.UpdatedAt, DbType.DateTime);
            param.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);
            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var rs = exec.ExcuteReturnValue("sp_V3ProblemEventRequestDep_InsUpd", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return rs;
        }
    }
}

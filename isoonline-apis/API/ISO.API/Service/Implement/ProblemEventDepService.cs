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
    public class ProblemEventDepService : IProblemEventDepService
    {
        public int Delete(ProblemEventDepDelModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            //object param = new { ID = model.ID, UpdatedAt = model.UpdatedAt, UpdatedBy = model.UpdatedBy };
            int rs = exec.ExcuteScalar("sp_V3ProblemEventDep_Delete", model);
            return rs;
        }

        public int DeleteByProblemEvent(ProblemEventDepDelModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            //object param = new { ID = model.ID, UpdatedAt = model.UpdatedAt, UpdatedBy = model.UpdatedBy };
            int rs = exec.ExcuteScalar("sp_V3ProblemEventDep_DeleteByProblemEvent", model);
            return rs;
        }

        public List<ProblemEventDepDTO> GetByProblemEvent(int problemEventID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProblemEventID", problemEventID, DbType.String);
            List<ProblemEventDepDTO> problemEventDepDTOs = exec.GetByField<ProblemEventDepDTO>("sp_V3ProblemEventDep_GetByProblemEvent", param);
            return problemEventDepDTOs;
        }

        public int Insert(ProblemEventDepInsModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            //param.Add("@ID", model.ID, DbType.Int32);
            param.Add("@ProblemEventID", model.ProblemEventID, DbType.Int32);
            param.Add("@DepartmentID", model.DepartmentID, DbType.Int32);
            param.Add("@IsDelete", model.IsDelete, DbType.Boolean);
            param.Add("@CreatedAt", model.CreatedAt, DbType.DateTime);
            param.Add("@CreatedBy", model.CreatedBy, DbType.Int32);
            param.Add("@UpdatedAt", model.UpdatedAt, DbType.DateTime);
            param.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);
            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var rs = exec.ExcuteReturnValue("sp_V3ProblemEventDep_Ins", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return rs;
        }
        public int InsertMulti(ProblemEventDepInsModel2[] model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var rs = exec.Execute("sp_V3ProblemEventDep_InsUpd", model, commandType: CommandType.StoredProcedure);
            return rs;
        }
    }
}

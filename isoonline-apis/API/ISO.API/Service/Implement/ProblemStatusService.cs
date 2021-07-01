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
    public class ProblemStatusService : IProblemStatusService
    {
        public int Default(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int problemType = exec.ExcuteScalar("sp_ProblemStatus_Default", new { ID = ID });
            return problemType;
        }

        public int Delete(int problemStatusID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = problemStatusID };
            int rs = exec.ExcuteScalar("sp_V3ProblemStatus_Delete", param);
            return rs;
        }

        public ProblemStatusDTO GetByID(int problemStatusID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = problemStatusID };
            ProblemStatusDTO problemStatusDTO = exec.GetByField<ProblemStatusDTO>("sp_V3ProblemStatus_GetByID", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return problemStatusDTO;
        }

        public List<ProblemStatusDTO> GetPaging(out int totalRows, ProblemStatusReqModel problemStatusReqModel)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", problemStatusReqModel.Code, DbType.String);
            param.Add("@Name", problemStatusReqModel.ProblemStatusName, DbType.String);
            param.Add("@PageSize", problemStatusReqModel.PageSize, DbType.String);
            param.Add("@PageNumber", problemStatusReqModel.PageNumber, DbType.String);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemStatusDTO> problemStatusDTOs = exec.GetPaging<ProblemStatusDTO>(out totalRows, "sp_V3ProblemStatus_GetPaging", param, "TotalRows",commandType: CommandType.StoredProcedure);
            return problemStatusDTOs;
        }

        public int InsertUpdate(ProblemStatusDTO problemStatusDTO)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", problemStatusDTO.Code, DbType.String);
            param.Add("@CreatedAt", problemStatusDTO.CreatedAt, DbType.DateTime);
            param.Add("@CreatedBy", problemStatusDTO.CreatedBy, DbType.Int32);
            param.Add("@Description", problemStatusDTO.Description, DbType.String);
            param.Add("@ID", problemStatusDTO.ID, DbType.Int32);
            param.Add("@IsDelete", problemStatusDTO.IsDelete, DbType.Boolean);
            param.Add("@ProblemStatusName", problemStatusDTO.ProblemStatusName, DbType.String);
            param.Add("@UpdatedAt", problemStatusDTO.UpdatedAt, DbType.DateTime);
            param.Add("@UpdatedBy", problemStatusDTO.UpdatedBy, DbType.Int32);
            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var rs = exec.ExcuteReturnValue("sp_V3ProblemStatus_InsUpd", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return rs;
        }
    }
}

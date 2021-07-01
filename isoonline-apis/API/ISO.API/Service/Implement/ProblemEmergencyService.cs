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
    public class ProblemEmergencyService : IProblemEmergencyService
    {
        public int Default(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int problemType = exec.ExcuteScalar("sp_ProblemEmergency_Default", new { ID = id });
            return problemType;
        }

        public int Delete(int problemEmergencyID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = problemEmergencyID };
            int rs = exec.ExcuteScalar("sp_V3ProblemEmergency_Delete", param);
            return rs;
        }

        public ProblemEmergencyDTO GetByID(int problemEmergencyID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = problemEmergencyID };
            ProblemEmergencyDTO problemEmergencyDTO = exec.GetByField<ProblemEmergencyDTO>("sp_V3ProblemEmergency_GetByID", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return problemEmergencyDTO;
        }

        public List<ProblemEmergencyDTO> GetPaging(out int totalRows, ProblemEmergencyReqModel problemEmergencyReqModel)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", problemEmergencyReqModel.Code, DbType.String);
            param.Add("@Name", problemEmergencyReqModel.ProblemEmergencyName, DbType.String);
            param.Add("@PageSize", problemEmergencyReqModel.PageSize, DbType.String);
            param.Add("@PageNumber", problemEmergencyReqModel.PageNumber, DbType.String);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);

            List<ProblemEmergencyDTO> problemEmergencyDTOs = exec.GetPaging<ProblemEmergencyDTO>(out totalRows, "sp_V3ProblemEmergency_GetPaging", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemEmergencyDTOs;
        }

        public int InsertUpdate(ProblemEmergencyDTO problemEmergencyDTO)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", problemEmergencyDTO.Code, DbType.String);
            param.Add("@CreatedAt", problemEmergencyDTO.CreatedAt, DbType.DateTime);
            param.Add("@CreatedBy", problemEmergencyDTO.CreatedBy, DbType.Int32);
            param.Add("@Description", problemEmergencyDTO.Description, DbType.String);
            param.Add("@ID", problemEmergencyDTO.ID, DbType.Int32);
            param.Add("@IsDelete", problemEmergencyDTO.IsDelete, DbType.Boolean);
            param.Add("@ProblemEmergencyName", problemEmergencyDTO.ProblemEmergencyName, DbType.String);
            param.Add("@UpdatedAt", problemEmergencyDTO.UpdatedAt, DbType.DateTime);
            param.Add("@UpdatedBy", problemEmergencyDTO.UpdatedBy, DbType.Int32);
            param.Add("@Color", problemEmergencyDTO.Color, DbType.String);
            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var rs = exec.ExcuteReturnValue("sp_V3ProblemEmergency_InsUpd", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return rs;
        }
    }
}

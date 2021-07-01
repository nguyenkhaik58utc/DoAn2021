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
    public class ProblemGroupService : IProblemGroupService
    {
        public int Delete(int problemStatusID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = problemStatusID };
            int rs = exec.ExcuteScalar("sp_V3ProblemGroup_Delete", param);
            return rs;
        }

        public List<ProblemGroupDTO> GetByID(int problemGroupID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = problemGroupID };
            List<ProblemGroupDTO> problemGroupDTO = exec.GetByField<ProblemGroupDTO>("sp_V3ProblemGroup_GetByID", param, commandType: CommandType.StoredProcedure).ToList();
            return problemGroupDTO;
        }

        public List<ProblemGroupDTO> GetByParent(int parentID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ParentID = parentID };
            List<ProblemGroupDTO> problemGroupDTOs = exec.GetByField<ProblemGroupDTO>("sp_V3ProblemGroup_GetByParent", param, commandType: CommandType.StoredProcedure);
            return problemGroupDTOs;
        }

        public List<ProblemGroupDTO> GetByType(int problemTypeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ProblemTypeID = problemTypeID };
            List<ProblemGroupDTO> problemGroupDTOs = exec.GetByField<ProblemGroupDTO>("sp_V3ProblemGroup_GetByType", param, commandType: CommandType.StoredProcedure);
            return problemGroupDTOs;
        }

        public List<ProblemGroupDTO> GetPaging(out int totalRows, ProblemGroupReqModel problemGroupReqModel)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", problemGroupReqModel.Code, DbType.String);
            param.Add("@Name", problemGroupReqModel.ProblemGroupName, DbType.String);
            param.Add("@PageSize", problemGroupReqModel.PageSize, DbType.String);
            param.Add("@PageNumber", problemGroupReqModel.PageNumber, DbType.String);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<ProblemGroupDTO> problemGroupDTOs = exec.GetPaging<ProblemGroupDTO>(out totalRows, "sp_V3ProblemGroup_GetPaging", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return problemGroupDTOs;
        }

        public int InsertUpdate(ProblemGroupDTO problemGroupDTO)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", problemGroupDTO.Code, DbType.String);
            param.Add("@CreatedAt", problemGroupDTO.CreatedAt, DbType.DateTime);
            param.Add("@CreatedBy", problemGroupDTO.CreatedBy, DbType.Int32);
            param.Add("@Description", problemGroupDTO.Description, DbType.String);
            param.Add("@ID", problemGroupDTO.ID, DbType.Int32);
            param.Add("@IsDelete", problemGroupDTO.IsDelete, DbType.Boolean);
            param.Add("@ProblemGroupName", problemGroupDTO.ProblemGroupName, DbType.String);
            param.Add("@UpdatedAt", problemGroupDTO.UpdatedAt, DbType.DateTime);
            param.Add("@UpdatedBy", problemGroupDTO.UpdatedBy, DbType.Int32);
            param.Add("@ParentID", problemGroupDTO.ParentID, DbType.Int32);
            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var rs = exec.ExcuteReturnValue("sp_V3ProblemGroup_InsUpd", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            for (int i = 0; i < problemGroupDTO.lstType.Count; i++)
            {
                DynamicParameters paramType = new DynamicParameters();
                paramType.Add("@TypeID", problemGroupDTO.lstType[i].ID, DbType.Int32);
                paramType.Add("@GroupID", rs, DbType.Int32);
                paramType.Add("@CreatedAt", problemGroupDTO.CreatedAt, DbType.DateTime);
                paramType.Add("@CreatedBy", problemGroupDTO.CreatedBy, DbType.Int32);
                paramType.Add("@UpdatedAt", problemGroupDTO.UpdatedAt, DbType.DateTime);
                if(problemGroupDTO.lstType[i].IsSelect == true)
                {
                    paramType.Add("@IsSelect", 1 , DbType.Int32);
                }
                else
                {
                    paramType.Add("@IsSelect", 0, DbType.Int32);
                }
                paramType.Add("@UpdatedBy", problemGroupDTO.UpdatedBy, DbType.Int32);
                paramType.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
                exec.ExcuteScalar("sp_V3ProblemGroupType_InsUpd", paramType);
            }
            return rs;
        }
    }
}

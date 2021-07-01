using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using ISO.API.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public class ProblemTypeService : IProblemTypeService
    {
        public int Add(object param) 
        {
            ExecuteQuery exec = new ExecuteQuery();
            int problemType = exec.ExcuteScalar("sp_ProblemType_Insert",param);
            return problemType;
        }

        public int Default(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int problemType = exec.ExcuteScalar("sp_ProblemType_Default", new { ID = id });
            return problemType;
        }

        public int Delete(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int problemType = exec.ExcuteScalar("sp_ProblemType_Delete", new { ID = id });
            return problemType;
        }

        public List<ProblemTypeDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<ProblemTypeDTO> ProblemTypeDTOs = exec.GetAll<ProblemTypeDTO>("sp_ProblemType_GetAll", commandType: CommandType.StoredProcedure);
            return ProblemTypeDTOs;
        }

        public List<ProblemTypeDTO> GetByID(int ID)
        {
            var p = new DynamicParameters();
            ExecuteQuery exec = new ExecuteQuery();
            List<ProblemTypeDTO> ProblemTypeDTOs = exec.GetByField<ProblemTypeDTO>("sp_ProblemType_GetByID", new { ID = ID }, commandType: CommandType.StoredProcedure);
            return ProblemTypeDTOs;

        }

        public List<ProblemTypeDTO> GetListByID(int ID)
        {
            var p = new DynamicParameters();
            ExecuteQuery exec = new ExecuteQuery();
            List<ProblemTypeDTO> ProblemTypeDTOs = exec.GetByField<ProblemTypeDTO>("sp_ProblemType_GetListByID", new { ID = ID }, commandType: CommandType.StoredProcedure);
            return ProblemTypeDTOs;
        }

        public int Update(object param)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int problemType = exec.ExcuteScalar("sp_ProblemType_Update", param);
            return problemType;
        }
    }
}

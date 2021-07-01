using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class ProblemCriticalLevelService : IProblemCriticalLevelService
    {
        public int AddProblemCriticalLevel(object param)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int problemCriticalLevelDTO = exec.ExcuteScalar("sp_ProblemCriticalLevel_Insert", param);
            return problemCriticalLevelDTO;
        }

        public int Default(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int problemType = exec.ExcuteScalar("sp_ProblemCriticalLevel_Default", new { ID = id });
            return problemType;
        }

        public int DeleteProblemCriticalLevel(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int problemCriticalLevelDTO = exec.ExcuteScalar("sp_ProblemCriticalLevel_Delete", new { ID = id });
            return problemCriticalLevelDTO;
        }

        public List<ProblemCriticalLevelDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<ProblemCriticalLevelDTO> problemCriticalLevelDTOs = exec.GetAll<ProblemCriticalLevelDTO>("sp_ProblemCriticalLevel_GetAll", commandType: CommandType.StoredProcedure);
            return problemCriticalLevelDTOs;
        }

        public List<ProblemCriticalLevelDTO> GetByID(int id)
        {
            var p = new DynamicParameters();
            ExecuteQuery exec = new ExecuteQuery();
            List<ProblemCriticalLevelDTO> problemCriticalLevelDTOs = exec.GetByField<ProblemCriticalLevelDTO>("sp_ProblemCriticalLevel_GetByID", new { ID = id }, commandType: CommandType.StoredProcedure);
            return problemCriticalLevelDTOs;
        }

        public int UpdateProblemCriticalLevel(object param)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int problemCriticalLevelDTO = exec.ExcuteScalar("sp_ProblemCriticalLevel_Update", param);
            return problemCriticalLevelDTO;
        }
    }
}

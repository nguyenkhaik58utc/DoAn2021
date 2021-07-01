using ISO.API.DataAccess;
using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public class ProblemEventUserService : IProblemEventUserService
    {
        public List<ProblemEventUserDTO> GetByProblemEvent(int problemEventID, string listType)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ProblemEventID = problemEventID, ListType = listType };
            //List<ProblemEventUserDTO> problemEventUserDTOs = exec.GetByField<ProblemEventUserDTO>("sp_V3ProblemEventUser_GetByProblemEventID", param, commandType: CommandType.StoredProcedure);
            List<ProblemEventUserDTO> problemEventUserDTOs = exec.GetByField<ProblemEventUserDTO>("sp_V3ProblemEventUser_GetByProblemEventID_ver01", param, commandType: CommandType.StoredProcedure);
            return problemEventUserDTOs;
        }

        public bool InsertUpdate(ProblemRelativePeopleInsReqModelDTO problemEventUserDTO)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var p = new
            {
                problemEventID = problemEventUserDTO.problemEventID,
                lstSelectedRole = problemEventUserDTO.lstSelectedRole,
                deptId = problemEventUserDTO.deptId,
                lstHumanEmployeeId = problemEventUserDTO.lstHumanEmployeeId
            };

            var rs = exec.ExcuteScalar("sp_V3ProblemEventUser_RelatedPeople", p);
            return rs > 0;
        }
        public bool Delete(string param)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var p = new
            {
                LstDeletedRelatePeople = param
            };

            var rs = exec.ExcuteScalar("sp_V3ProblemEventUser_Delete_ver01", p);
            return rs > 0;
        }

        public int DeleteMulti(ProblemEventUserDelModel model)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { IDs = string.Join(',', model.IDs), UpdatedAt = model.UpdatedAt, UpdatedBy = model.UpdatedBy };
            var rs = exec.ExcuteScalar("sp_V3ProblemEventUser_DeleteMulti", param);
            return rs;
        }

        public List<ProblemUserFix> GetUserFix(int problemEventID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ProblemEventID = problemEventID};
            List<ProblemUserFix> result = exec.GetByField<ProblemUserFix>("sp_ProblemEvent_GetUserFix", param, commandType: CommandType.StoredProcedure);
            return result;
        }

        public List<HumanRoleHumanDepartmentHumanEmployeeDTO> GetListRoleDepartmentFromListHumanEmployeeID(string lstHumanEmployeeIDs)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { lstHumanEmployeeIDs = lstHumanEmployeeIDs };
            List<HumanRoleHumanDepartmentHumanEmployeeDTO> HumanRoleHumanDepartmentHumanEmployeeDTOs = exec.GetByField<HumanRoleHumanDepartmentHumanEmployeeDTO>("sp_V3GetListHumanRoleHumanDepartmentFromListHumanEmployeeID", param, commandType: CommandType.StoredProcedure);
            return HumanRoleHumanDepartmentHumanEmployeeDTOs;
        }
    }
}

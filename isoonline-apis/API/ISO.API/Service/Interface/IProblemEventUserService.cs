using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IProblemEventUserService
    {
        List<ProblemEventUserDTO> GetByProblemEvent(int ProblemEventID, string ListType);
        bool InsertUpdate(ProblemRelativePeopleInsReqModelDTO problemEventUserDTO);
        bool Delete(string param);
        int DeleteMulti(ProblemEventUserDelModel model);
        List<ProblemUserFix> GetUserFix(int problemEventID);
        List<HumanRoleHumanDepartmentHumanEmployeeDTO> GetListRoleDepartmentFromListHumanEmployeeID(string lstHumanEmployeeIDs);
    }
}

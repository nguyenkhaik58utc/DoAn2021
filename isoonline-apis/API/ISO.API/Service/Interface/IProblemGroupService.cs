using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IProblemGroupService
    {
        List<ProblemGroupDTO> GetPaging(out int totalRows, ProblemGroupReqModel problemGroupReqModel);
        int InsertUpdate(ProblemGroupDTO problemGroupDTO);
        int Delete(int problemStatusID);
        List<ProblemGroupDTO> GetByID(int problemGroupID);
        List<ProblemGroupDTO> GetByType(int problemTypeID);
        List<ProblemGroupDTO> GetByParent(int parentID);
    }
}

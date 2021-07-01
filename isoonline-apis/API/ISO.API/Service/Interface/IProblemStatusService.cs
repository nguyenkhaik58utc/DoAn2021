using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IProblemStatusService
    {
        List<ProblemStatusDTO> GetPaging(out int totalRows, ProblemStatusReqModel problemStatusReqModel);
        int InsertUpdate(ProblemStatusDTO problemStatusDTO);
        int Delete(int problemStatusID);
        ProblemStatusDTO GetByID(int problemStatusID);
        int Default(int ID);
    }
}

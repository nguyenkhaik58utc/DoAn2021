using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IProblemEventService
    {
        List<ProblemEventDTO> GetPaging(out int totalRows, ProblemEventReqModel problemEventReqModel);
        int InsertUpdate(ProblemEventDTO problemEventDTO);
        int Delete(int problemEventID);
        ProblemEventDTO GetByID(int problemEventID);
    }
}

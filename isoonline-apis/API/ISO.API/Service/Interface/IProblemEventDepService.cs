using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IProblemEventDepService
    {
        int Insert(ProblemEventDepInsModel model);
        int InsertMulti(ProblemEventDepInsModel2[] model);
        int Delete(ProblemEventDepDelModel model);
        int DeleteByProblemEvent(ProblemEventDepDelModel model);
        List<ProblemEventDepDTO> GetByProblemEvent(int problemEventID);
    }
}

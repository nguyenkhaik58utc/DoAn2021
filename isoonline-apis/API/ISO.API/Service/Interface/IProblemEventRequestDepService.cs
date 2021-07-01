using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IProblemEventRequestDepService
    {
        List<ProblemEventRequestDepDTO> GetPaging(out int totalRows, ProblemEventRequestDepSearchModel model);
        int InsertUpdate(ProblemEventRequestDepInsUpdModel model);
        int Delete(ProblemEventRequestDepDelModel model);
        ProblemEventRequestDepDTO GetByID(int id);
    }
}

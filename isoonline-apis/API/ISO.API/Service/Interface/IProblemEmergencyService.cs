using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IProblemEmergencyService
    {
        List<ProblemEmergencyDTO> GetPaging(out int totalRows, ProblemEmergencyReqModel problemEmergencyReqModel);
        int InsertUpdate(ProblemEmergencyDTO problemEmergencyDTO);
        int Delete(int problemEmergencyID);
        ProblemEmergencyDTO GetByID(int problemEmergencyID);
        int Default(int id);
    }
}

using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IProblemResidentAgencyService
    {
        List<ProblemResidentAgencyDTO> GetPaging(out int totalRows, ProblemResidentAgencyReqModel agencyReqModel);
        List<ProblemResidentAgencyDTO> GetByGroupID(int groupID);
        int InsertUpdate(ProblemResidentAgencyDTO problemResidentAgency);
        int Delete(int problemResidentAgencyID);
        ProblemResidentAgencyDTO GetByID(int problemResidentAgencyID);
    }
}
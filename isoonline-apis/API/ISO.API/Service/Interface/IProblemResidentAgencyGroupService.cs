using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IProblemResidentAgencyGroupService
    {
        List<ProblemResidentAgencyGroupDTO> GetPaging(out int totalRows, ProblemResidentAgencyGroupReqModel agencyGroupReqModel);
        int InsertUpdate(ProblemResidentAgencyGroupDTO problemResidentAgencyGroup);
        int Delete(int problemResidentAgencyID);
        ProblemResidentAgencyGroupDTO GetByID(int problemResidentAgencyID);
    }
}
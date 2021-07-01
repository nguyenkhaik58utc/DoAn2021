using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IProblemEventReportService
    {
        List<ProblemEventReportDTO> GetPaging(out int totalRows, ProblemEventReportSearchModel model);
        int InsertUpdate(ProblemEventReportInsModel model);
        ProblemEventReportDTO GetByID(int id);
        List<ProblemEventReportDTO> GetByProblemEventID(int id);
        int Delete(ProblemEventReportDeleteModel model);
    }
}

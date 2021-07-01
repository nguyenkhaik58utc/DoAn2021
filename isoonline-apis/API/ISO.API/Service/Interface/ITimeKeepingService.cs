using ISO.API.Models.TimeKeeping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface ITimeKeepingService
    {
        List<TimeKeepingOfEmployeeViewModel> GetByMonth(out int totalRows, GetTimeKeepingByMonthRequest request);

        List<TimeKeepingExplanationViewModel> GetExplanationByMonth(out int totalRows, GetTimeKeepingExplanationByMonthRequest request);

        int DeleteExplanation(int id);
    }
}
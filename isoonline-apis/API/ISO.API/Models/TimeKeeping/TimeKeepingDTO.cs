using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.TimeKeeping
{
    public class TimeKeepingDTO
    {
    }

    public class TimeKeepingViewModel
    {
        public int EmployeeID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime DateCheck { get; set; }

        public string TimeIn { get; set; }

        public string TimeOut { get; set; }
    }

    public class TimeKeepingOfEmployeeViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public List<TimeKeepingViewModel> ListTimeKeeping { get; set; }
    }

    public class GetTimeKeepingByMonthRequest
    {
        public int EmployeeId { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetTimeKeepingExplanationByMonthRequest
    {
        public int Status { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class TimeKeepingExplanationViewModel
    {
        public int Id { get; set; }
        public DateTime DateCheck { get; set; }

        public string TimeIn1 { get; set; }

        public string TimeOut1 { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string ApproveID { get; set; }
        public string ApproveName { get; set; }
        public string ReasonExplanation { get; set; }
    }
}
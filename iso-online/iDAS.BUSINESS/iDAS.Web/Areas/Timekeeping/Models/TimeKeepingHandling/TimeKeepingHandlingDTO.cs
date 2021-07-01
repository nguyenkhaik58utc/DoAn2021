using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Timekeeping.Models.TimeKeepingHandling
{
    public class TimeKeepingHandlingDTO
    {
    }

    public class TimeKeepingHandlingViewModel
    {
        public DateTime DateCheck { get; set; }

        public string TimeIn { get; set; }

        public string TimeOut { get; set; }
    }

    public class TimeKeepingHandlingOfEmployeeViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public List<TimeKeepingHandlingViewModel> ListTimeKeeping { get; set; }
    }

    public class GetTimeKeepingHandlingByMonthRequest
    {
        public int EmployeeId { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }
    }
}
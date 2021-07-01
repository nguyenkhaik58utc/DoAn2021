using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Timekeeping.Models.TimeKeeping
{
    public class TimeKeepingDTO
    {
    }

    public class TimeKeepingViewModel
    {
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
    }

    public class TimeKeepingExplanationViewModel
    {
        public int Id { get; set; }
        public DateTime DateCheck { get; set; }

        public string TimeIn1 { get; set; }

        public string TimeOut1 { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string ApproveID { get; set; }
        public string ApproveName { get; set; }
        public string ReasonExplanation { get; set; }
        public string Reason { get; set; }

        public string StatusText
        {
            get
            {
                if (Status == 0 && !string.IsNullOrEmpty(Reason))
                    return "Chưa yêu cầu giải trình";
                else if (Status == 1)
                    return "Chờ giải trình";
                else if (Status == 2)
                    return "Đã giải trình, Chờ xét duyệt";
                else if (Status == 3)
                    return "Đã duyệt";
                else if (Status == 4)
                    return "Từ chối";
                else
                    return "";
            }
        }

        public string DateOfWeek
        {
            get
            {
                return Ultility.GetNameDayOfWeek(DateCheck);
            }
        }
    }

    public class GetTimeKeepingExplanationByMonthRequest
    {
        public int Status { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }
    }
}
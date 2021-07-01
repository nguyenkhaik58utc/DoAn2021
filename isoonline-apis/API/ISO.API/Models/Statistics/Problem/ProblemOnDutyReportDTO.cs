using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class ProblemOnDutyReportDTO
    {
        public bool OnDuty { get; set; }
        public int DepartmentID { get; set; }
        public string DepName { get; set; }
        public int Count { get; set; }
    }
}

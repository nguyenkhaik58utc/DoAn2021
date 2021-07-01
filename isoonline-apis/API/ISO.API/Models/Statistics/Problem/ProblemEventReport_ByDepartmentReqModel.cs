using System;

namespace ISO.API.Models
{
    public class ProblemEventReport_ByDepartmentReqModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int DepartmentID { get; set; }

    }
}

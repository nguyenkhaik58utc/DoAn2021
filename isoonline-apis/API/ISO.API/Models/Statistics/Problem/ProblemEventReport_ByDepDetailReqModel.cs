using System;

namespace ISO.API.Models
{
    public class ProblemEventReport_ByDepDetailReqModel
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int DepartmentID { get; set; }
        public int? CriticalID { get; set; }
        public int? ProblemTypeID { get; set; }
        public int? UserID { get; set; }
    }
}

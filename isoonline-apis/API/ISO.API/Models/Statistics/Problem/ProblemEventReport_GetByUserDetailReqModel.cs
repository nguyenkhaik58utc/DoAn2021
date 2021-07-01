using System;

namespace ISO.API.Models
{
    public class ProblemEventReport_GetByUserDetailReqModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int ProblemTypeID { get; set; }
        public int CriticalID { get; set; }
        public int UserID { get; set; }
    }
}

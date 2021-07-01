using System;

namespace ISO.API.Models
{
    public class ProblemEventReport_GetByUserReqModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int UserID { get; set; }
    }
}

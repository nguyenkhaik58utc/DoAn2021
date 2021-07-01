using System.Collections.Generic;

namespace ISO.API.Models
{
    public class ProblemEventUserReportResModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int DepartmentID { get; set; }
        public string DepName { get; set; }
        public List<ProblemType> ProblemTypes { get; set; }
    }
}

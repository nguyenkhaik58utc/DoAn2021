namespace ISO.API.Models
{
    public class ProblemEventUserReportDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int DepartmentID { get; set; }
        public string DepName { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public int Count { get; set; }
    }
}

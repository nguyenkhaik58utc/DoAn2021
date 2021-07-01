namespace ISO.API.Models
{
    public class ProblemEventReport_ByDepartmentTypeDTO
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}

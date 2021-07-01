namespace ISO.API.Models
{
    public class ProblemEventReport_ByDepartmentCriticalDTO
    {
        public int CriticalLevelID { get; set; }
        public string CriticalLevelName { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}

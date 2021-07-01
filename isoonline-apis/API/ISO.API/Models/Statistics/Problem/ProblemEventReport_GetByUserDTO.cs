namespace ISO.API.Models
{
    public class ProblemEventReport_GetByUserDTO
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public int CriticalLevelID { get; set; }
        public string CriticalLevelName { get; set; }
        public int Count { get; set; }
    }
}

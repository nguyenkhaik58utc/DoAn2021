namespace ISO.API.Models
{
    public class ProblemEventReport_ByEventRequestDepTypeDTO
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public int Count { get; set; }
        public int RequestDepId { get; set; }
        public string Name { get; set; }
    }
}

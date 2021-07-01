namespace ISO.API.Models
{
    public class ProblemEventReport_ByResidentAgencyTypeDTO
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public int Count { get; set; }
        public int ResidentAgencyID { get; set; }
        public string ResidentAgencyName { get; set; }
    }
}

namespace ISO.API.Models
{
    public class ProblemEventReport_ByResidentAgencyDepartmentDTO
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int Count { get; set; }
        public int ResidentAgencyID { get; set; }
        public string ResidentAgencyName { get; set; }
    }
}

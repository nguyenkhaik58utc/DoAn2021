namespace ISO.API.Models
{
    public class CriticalTypeReportDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CriticalLevelID { get; set; }
        public string CriticalLevelName { get; set; }
        public int DepartmentID { get; set; }
        public string DepName { get; set; }
        public int Count { get; set; }
    }
}

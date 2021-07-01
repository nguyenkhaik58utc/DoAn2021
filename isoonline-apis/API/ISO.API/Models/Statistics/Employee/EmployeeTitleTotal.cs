namespace ISO.API.Models
{
    public class EmployeeTitleTotal
    {
        public int TitleID { get; set; }
        public string TitleCode { get; set; }
        public string TitleName { get; set; }

        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }

        public int EmpTotal { get; set; }
    }
}

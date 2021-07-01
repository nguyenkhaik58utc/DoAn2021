namespace ISO.API.Models
{
    public class EmployeeTotal
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int ParentID { get; set; }
        public int EmpTotal { get; set; }

        public bool Leaf { get; set; }
    }

    public class EmployeeTotalTemp
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int ParentID { get; set; }
        public int EmpID { get; set; }
    }
}

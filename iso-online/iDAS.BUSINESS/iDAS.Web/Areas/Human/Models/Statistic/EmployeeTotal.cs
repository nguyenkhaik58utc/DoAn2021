namespace iDAS.Web.Areas.Human.Models
{
    public class EmployeeTotal
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int ParentID { get; set; }
        public int EmpTotal { get; set; }
        public bool Leaf { get; set; }
    }
}

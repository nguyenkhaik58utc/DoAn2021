using System.Collections.Generic;

namespace iDAS.Web.Areas.Human.Models
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

    public class Dep
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int Count { get; set; }
    }

    public class EmployeeTitleStatistic
    {
        public int TitleID { get; set; }
        public string TitleName { get; set; }
        public List<Dep> Departments { get; set; }
    }
}

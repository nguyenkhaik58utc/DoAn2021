using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Problem.Models.RelativePeople
{
    public class ProblemRelativePeopleDTO
    {
        public int ID { get; set; }
        public int ProblemEventID { get; set; }
        public int UserID { get; set; }
        public bool View { get; set; }
        public bool Update { get; set; }
        public bool Review { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public string userName { get; set; }
        public string depName { get; set; }
        public string txtVaiTro { get; set; }
    }

    public class ProblemRelativePeopleInsUpdListDTO
    {
        public int? ID { get; set; }
        public int ProblemEventID { get; set; }
        public DepUser[] UserDep { get; set; }
        public bool View { get; set; }
        public bool Update { get; set; }
        public bool Review { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }

    public class DepUser
    {
        public int UserID { get; set; }
        public int DepartmentID { get; set; }
    }
    public class HumanRoleHumanDepartmentHumanEmployeeDTO
    {
        public int HumanEmployeeID { get; set; }
        public string Name { get; set; }
        public int HumanRoleID { get; set; }
        public int HumanDepartmentID { get; set; }
        public string TitleName { get; set; }
        public string DepartmentName { get; set; }
    }
}
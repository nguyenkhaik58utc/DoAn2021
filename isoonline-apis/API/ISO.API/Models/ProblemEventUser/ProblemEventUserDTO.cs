using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class ProblemEventUserDTO
    {
        public int ID { get; set; }
        public int ProblemEventID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int DepartmentID { get; set; }
        public string DepName { get; set; }
        public bool View { get; set; }
        public bool Update { get; set; }
        public bool Review { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
    public class ProblemEventUserInsUpdModel
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
    public class UserDepInsModel
    {
        public int UserID { get; set; }
        public bool View { get; set; }
        public bool Update { get; set; }
        public bool Review { get; set; }
    }
    public class ProblemEventUserDelModel
    {
        public int[] IDs { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    // HungNM. Add class for InsertProblemRelativePeopleList.
    public class ProblemRelativePeopleInsReqModelDTO
    {
        public int problemEventID { get; set; }
        public string lstSelectedRole { get; set; }
        public int deptId { get; set; }
        public string lstHumanEmployeeId { get; set; }
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
    public class BusinesslstHumanEmployeeIDs
    {
        public string lstHumanEmployeeIDs { get; set; }
    }
    // End.
}

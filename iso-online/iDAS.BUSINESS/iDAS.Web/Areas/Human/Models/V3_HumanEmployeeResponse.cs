using iDAS.Web.Areas.Department.Models.Title;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Human.Model
{
    public class V3_HumanEmployeeResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDay { get; set; }
        public string FileName { get; set; }
        public Guid? FileID { get; set; }
        public bool IsTrial { get; set; }
        public string DepartmentTitle { get; set; }
        public string ListIDDepartmentTitle { get; set; }
        public bool Gender { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public List<DepartmentTitleResponse> ListDepartmentTitle { get; set; }
    }
}
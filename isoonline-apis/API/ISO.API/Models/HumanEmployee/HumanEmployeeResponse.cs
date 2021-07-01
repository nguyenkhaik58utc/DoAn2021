using ISO.API.Models.DepartmentTitle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanEmployee
{
    public class HumanEmployeeResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? FileName { get; set; }
        public Guid? FileID { get; set; }
        public bool IsTrial { get; set; }
        public bool Gender { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public bool CheckedShow { get; set; }
        public bool CheckedEdit { get; set; }
        public bool CheckedDelete { get; set; }
        public List<DepartmentTitleResponse> ListDepartmentTitle { get; set; }
    }
}

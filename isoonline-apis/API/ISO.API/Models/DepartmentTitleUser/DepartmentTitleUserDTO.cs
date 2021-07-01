using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class DepartmentTitleUserDTO
    {
        public int ID { get; set; }
        public int DepartmentTitleID { get; set; }
        public int HumanEmployeeID { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }
    public class DepartmentTitleUserInsUpdModel
    {
        public int? ID { get; set; }
        public int DepartmentTitleID { get; set; }
        public int HumanEmployeeID { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
    public class DepartmentTitleUserDeleteReqModel
    {
        public int DepartmentTitleID { get; set; }
        public int HumanEmployeeID { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }
}

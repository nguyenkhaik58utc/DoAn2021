using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class ProblemEventDepDTO
    {
        public int ID { get; set; }
        public int ProblemEventID { get; set; }
        public int DepartmentID { get; set; }
        public string DepName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }
    public class ProblemEventDepInsModel
    {
        public int? ProblemEventID { get; set; }
        public int[] DepartmentIDs { get; set; }
        public int? DepartmentID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDelete { get; set; }
    }
    public class ProblemEventDepInsModel2
    {
        public int? ProblemEventID { get; set; }
        public int? DepartmentID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDelete { get; set; }
    }
    public class ProblemEventDepDelModel
    {
        public int ID { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}

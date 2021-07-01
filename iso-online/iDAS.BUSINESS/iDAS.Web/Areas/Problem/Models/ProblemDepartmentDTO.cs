using iDAS.Items;
using System;
using System.Collections.Generic;

namespace iDAS.Web.Areas.Problem.Models
{
    public class ProblemDepartmentDTO
    {
        public int? ID { get; set; }
        public int ProblemEventID { get; set; }
        public int DepartmentID { get; set; }
        public List<int> DepartmentIDs { get; set; }
        public int ObjectID { get { return DepartmentID; } }
        public string DepName { get; set; }
        public string Name { get { return DepName; } }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
    }
}
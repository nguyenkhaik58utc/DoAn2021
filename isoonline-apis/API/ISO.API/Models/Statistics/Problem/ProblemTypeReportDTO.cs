using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class ProblemTypeReportDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public int DepartmentID { get; set; }
        public string DepName { get; set; }
        public int Count { get; set; }
    }
}

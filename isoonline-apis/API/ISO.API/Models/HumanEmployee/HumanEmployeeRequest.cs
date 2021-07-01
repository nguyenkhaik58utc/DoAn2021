using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanEmployee
{
    public class HumanEmployeeRequest
    {
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int DepartmentID { get; set; }
    }
}

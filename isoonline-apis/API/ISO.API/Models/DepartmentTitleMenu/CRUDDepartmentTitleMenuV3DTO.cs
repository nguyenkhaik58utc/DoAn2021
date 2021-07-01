using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class CRUDDepartmentTitleMenuV3DTO
    {
        public int DepartmentID { get; set; }
        public int TitleID { get; set; }
        public string lstAdd { get; set; }
        public string lstDelete { get; set; }
    }
}

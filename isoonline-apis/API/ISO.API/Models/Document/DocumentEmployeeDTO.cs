using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class DocumentEmployeeDTO
    {
        public int ID { get; set; }
        public int DocumentID { get; set; }
        public int EmployeeID { get; set; }
    }


    public class DocumentEmployeeSaveDTO
    {
        public int ID { get; set; }
        public int DocumentID { get; set; }
        public int EmployeeID { get; set; }
        public bool CheckedShow { get; set; }
        public bool CheckedEdit { get; set; }

    }

    public class DocumentEmployeeSaveRequest
    {
        public int DocumentID { get; set; }
        public List<DocumentEmployeeSaveDTO> EmployeeList { get; set; }
    }

    public class DocumentPublicSaveRequest
    {
        public int DocumentID { get; set; }
        public bool IsPublic { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.Document
{
    public class DocumentDepartmentDTO
    {
        public int ID { get; set; }
        public int DocumentID { get; set; }
        public int DepartmentID { get; set; }
        public bool IsShow { get; set; }
        public bool IsUpdate { get; set; }
    }

    public class DocumentDepartmentSaveDTO
    {
        public int ID { get; set; }
        public bool IsShow { get; set; }
        public bool IsUpdate { get; set; }
    }



    public class DocumentDepartmentSaveRequest
    {
        public int DocumentID { get; set; }
        public List<DocumentDepartmentDTO> DepartmentIDList { get; set; }
    }
}

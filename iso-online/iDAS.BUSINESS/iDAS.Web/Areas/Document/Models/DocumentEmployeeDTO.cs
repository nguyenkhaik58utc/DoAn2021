using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Document.Models
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

        public DocumentEmployeeSaveDTO(int ID,  bool checkedShow, bool checkedEdit)
        {
            this.EmployeeID = ID;
            CheckedShow = checkedShow;
            CheckedEdit = checkedEdit;
        }
    }

    public class DocumentEmployeeSaveRequest
    {
        public int DocumentID { get; set; }
        public List<DocumentEmployeeSaveDTO> EmployeeList { get; set; }
    }


    public class DocumentEmployeeViewModel: DocumentEmployeeDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDay { get; set; }
        public bool CheckedShow { get; set; }
        public bool CheckedEdit { get; set; }
        public bool CheckedDelete { get; set; }
        public bool IsAccess { get; set; }
    }

    public class DocumentPublicSaveRequest
    {
        public int DocumentID { get; set; }
        public bool IsPublic { get; set; }
    }
}

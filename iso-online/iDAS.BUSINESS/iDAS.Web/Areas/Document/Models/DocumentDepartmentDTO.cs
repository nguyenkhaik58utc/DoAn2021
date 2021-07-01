using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Document.Models
{
    public class DocumentDepartmentDTO
    {
        public int ID { get; set; }
        public int DocumentID { get; set; }
        public int DepartmentID { get; set; }
        public bool IsShow { get; set; }
        public bool IsUpdate { get; set; }

        public DocumentDepartmentDTO(int iD,int documentID, bool isShow, bool isUpdate)
        {
            DepartmentID = iD;
            this.DocumentID = documentID;
            IsShow = isShow;
            IsUpdate = isUpdate;
        }
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

    public class DocumentDepartmentViewModel : HumanDepartmentItem
    {
        public bool IsAccess { get; set; }
        public bool IsShow { get; set; }
        public bool IsUpdate { get; set; }
    }
}
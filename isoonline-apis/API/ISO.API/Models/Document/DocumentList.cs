using System;

namespace ISO.API.Models
{
    public class DocumentList
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public DateTime? PublicDate { get; set; }//Ngày ban hành
        public int? PublicNumber { get; set; }//Lần ban hành
        public bool FormH { get; set; }
        public bool FormS { get; set; }
        public string Security { get; set; }
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Note { get; set; }
        public string Color { get; set; }
        public bool IsPublic { get; set; }
        public bool AllUserAccess { get; set; }
        public bool IsObs { get; set; }
        public bool HasRequest { get; set; }
        public int? ParentID { get; set; }

        public bool IsEdit { get; set; }
        public bool IsApproval { get; set; }
        public bool IsAccept { get; set; }
        public bool IsObsolete { get; set; }
        public int? ApprovalBy { get; set; }

        public bool IsEditPermistion { get; set; }
        
    }
}

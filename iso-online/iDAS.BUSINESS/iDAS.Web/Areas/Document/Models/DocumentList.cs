using iDAS.Items;
using System;
using System.Collections.Generic;

namespace iDAS.Web.Areas.Document.Models
{
    public class DocumentList: ApprovalItem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public Nullable<System.DateTime> PublicDate { get; set; }//Ngày ban hành
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
        public bool IsObsolete { get; set; }
        public bool HasRequest { get; set; }
        public int? ParentID { get; set; }
        public bool FlagModified { get; set; }//Đánh dấu là TL Ban hành đã có TL sửa đổi

        public bool IsEditPermistion { get; set; }

        public List<Guid> AttachmentFileIDs { get; set; }
    }
}

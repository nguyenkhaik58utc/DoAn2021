using System;

namespace iDAS.Web.Areas.Document.Models
{
    public class DocumentFolder
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int ParentID { get; set; }

        public bool DocumentOut { get; set; }

        public bool IsDelete { get; set; }
        public DateTime? CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int UpdateBy { get; set; }
    }
}

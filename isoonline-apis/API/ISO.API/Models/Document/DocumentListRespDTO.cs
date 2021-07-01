using System.Collections.Generic;

namespace ISO.API.Models
{
    public class DocumentListRespDTO
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }

        public List<DocumentList> lstDocument { get; set; }

        public DocumentListRespDTO()
        {
            lstDocument = new List<DocumentList>();
        }
    }
}

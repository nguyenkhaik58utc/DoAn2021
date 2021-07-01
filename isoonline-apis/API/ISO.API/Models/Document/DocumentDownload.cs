using System;

namespace ISO.API.Models
{
    public class DocumentDownload
    {
        public int DocumentID { get; set; }
       
        public bool QuickDownload { get; set; }
        public DateTime? DayDownloadLimited { get; set; }
    }
}

using System;

namespace ISO.API.Models
{
    public class CriticalTypeReportEventRequestDepDetailReqModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int CriticalID { get; set; }
        public int EventRequestDepID { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}

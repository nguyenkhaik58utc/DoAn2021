using System;

namespace ISO.API.Models
{
    public class DepartmentReportResidentAgencyDetailReqModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int DepartmentID { get; set; }
        public int ResidentAgencyID { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}

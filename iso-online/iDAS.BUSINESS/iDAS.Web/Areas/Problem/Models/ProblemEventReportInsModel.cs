using System;

namespace iDAS.Web.Areas.Problem.Models
{
    public class ProblemEventReportInsModel
    {
        public int? ID { get; set; }
        public int? ProblemEventID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public DateTime? ReportDate { get; set; }
        public int? Reporter { get; set; }
        public bool? IsDelete { get; set; }
        public int? Status { get; set; }
        public int? Percent { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
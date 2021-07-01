using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class ProblemEventReportDTO
    {
        public int ID { get; set; }
        public int ProblemEventID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Reporter { get; set; }
        public DateTime ReportDate { get; set; }
        public string ReporterName { get; set; }
        public bool IsDelete { get; set; }
        public int Status { get; set; }
        public int Percent { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public string DepName { get; set; }
    }
    public class ProblemEventReportInsModel
    {
        public int? ID { get; set; }
        public int? ProblemEventID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? Reporter { get; set; }
        public int? DepartmentID { get; set; }
        public DateTime? ReportDate { get; set; }
        public bool? IsDelete { get; set; }
        public int? Status { get; set; }
        public double? Percent { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
    public class ProblemEventReportSearchModel
    {
        public int? ProblemEventID { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string Name { get; set; }
        public int? Reporter { get; set; }
        public int? Status { get; set; }
    }
    public class ProblemEventReportDeleteModel
    {
        public int ID { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }
}

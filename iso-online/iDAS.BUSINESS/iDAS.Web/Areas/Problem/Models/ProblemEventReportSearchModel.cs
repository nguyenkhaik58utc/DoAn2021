namespace iDAS.Web.Areas.Problem.Models
{
    public class ProblemEventReportSearchModel
    {
        public int? ProblemEventID { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string Name { get; set; }
        public int? Reporter { get; set; }
        public int? Status { get; set; }
    }
}
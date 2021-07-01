using iDAS.Items;
using System;

namespace iDAS.Web.Areas.Problem.Models.RelativePeople
{
    public class ProblemRelativePeopleChkCbo
    {
        public bool? chkView { get; set; }
        public bool? chkUpdate { get; set; }
        public bool? chkReview { get; set; }
        public string rdbList { get; set; }
        public string lstRelativeDeptsFilter { get; set; }
    }
}
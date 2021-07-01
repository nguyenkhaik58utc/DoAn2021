using iDAS.Items.Problem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Problem.Models
{
    public class ProblemCriticalLevelRespModel
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public List<ProblemCriticalLevelItem> lstProblemCriticalLevel { get; set; }
        public ProblemCriticalLevelItem ProblemCriticalLevelItem { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Problem.Models
{
    public class ProblemStatusReqModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemStatusName { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
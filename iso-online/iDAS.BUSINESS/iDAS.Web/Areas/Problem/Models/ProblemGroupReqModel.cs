using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Problem.Models
{
    public class ProblemGroupReqModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemGroupName { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
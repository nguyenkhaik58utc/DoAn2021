using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Problem.Models
{
    public class ProblemTypeDTO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemTypeName { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
    }
}
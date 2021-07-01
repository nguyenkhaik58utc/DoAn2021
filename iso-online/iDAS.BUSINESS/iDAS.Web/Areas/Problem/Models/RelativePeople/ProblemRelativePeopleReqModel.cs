using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Problem.Models.RelativePeople
{
    public class ProblemRelativePeopleReqModel
    {
        public int ProblemEventID { get; set; }
    }

    public class ProblemRelativePeopleInsReqModelDTO
    {
        public int problemEventID { get; set; }
        public string lstSelectedRole { get; set; }
        public int deptId { get; set; }
        public string lstHumanEmployeeId { get; set; }
    }

    public class RequestModelBusinesslstHumanEmployeeIDs
    {
        public string lstHumanEmployeeIDs { get; set; }
    }
}
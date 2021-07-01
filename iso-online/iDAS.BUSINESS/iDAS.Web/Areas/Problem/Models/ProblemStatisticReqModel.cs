using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Problem.Models
{
    public class ProblemStatisticReqModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
      
        public int ProblemTypeID { get; set; }
        public int CriticalID { get; set; }

        public int DepartmentID { get; set; }
        public int UserID { get; set; }

        public bool OnDuty { get; set; }
    }

    public class ProblemStatisticEventRequestReqModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }

        public int ProblemTypeID { get; set; }
        public int CriticalID { get; set; }
        public int DepartmentID { get; set; }
        public int ResidentAgencyID { get; set; }

        public int EventRequestDepID { get; set; }
        public int UserID { get; set; }
    }
    public class ProblemStatisticEventResidentAgencyModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }

        public int ProblemTypeID { get; set; }
        public int DepartmentID { get; set; }
        public int ResidentAgencyID { get; set; }
    }

}
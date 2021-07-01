using System;
using System.Collections.Generic;

namespace iDAS.Web.Areas.Human.Models
{
    public class HumanEmployeeSearchRequest
    {
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int departmentID { get; set; }

        public string name { get; set; }
        public DateTime? birthDayFrom { get; set; }
        public DateTime? birthDayTo { get; set; }
        public string native { get; set; }
        public int? religion { get; set; }
        public int? ethnic { get; set; }

        public string placeOfWork { get; set; }
        public string position { get; set; }
        public string departmentName { get; set; }

        public string eduName { get; set; }
        public int? educationType { get; set; }
        public int? educationResult { get; set; }

        public string diplomaName { get; set; }
        public int? major { get; set; }
        public int? diplomaEucationType { get; set; }
        public int? diplomaEducationOrg { get; set; }
        public int? educationLevel { get; set; }
        public int? certificateLevel { get; set; }

        public string certificateName { get; set; }
        public int? level { get; set; }
        public int? @certificatetype { get; set; }

        public string numberOfDecision { get; set; }
        public string reason { get; set; }
        public int? form { get; set; }

        public bool? isLeave { get; set; }
    }

   public class GetByListEmployeeIdRequest
    {
        public List<int> lstEmployeeId { get; set; }
    }
}

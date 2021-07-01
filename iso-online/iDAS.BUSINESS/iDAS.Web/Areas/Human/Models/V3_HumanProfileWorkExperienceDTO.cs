using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Human.Models
{
    public class V3_HumanProfileWorkExperienceDTO
    {
        public int ID { get; set; }
        public int HumanEmployeeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Position { get; set; }

        public string JobDescription { get; set; }

        public string Department { get; set; }
       // public FileUploadBO FileAttachs { get; set; }
        public string PlaceOfWork { get; set; }
    }
}
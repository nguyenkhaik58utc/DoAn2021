using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanProfileWorkExperience
{
    public class HumanProfileWorkExperienceDTO
    {
        public string HumanEmployeeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Position { get; set; }
        public string PlaceOfWork { get; set; }
        public string Department { get; set; }
        public string JobDescription { get; set; }
    }
}

using System.Collections.Generic;

namespace ISO.API.Models
{
    public class ProblemTypeEventResidentAgencyResModel
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public List<ResidentAgency> ResidentAgencies { get; set; }
    }
}

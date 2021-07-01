using System.Collections.Generic;

namespace ISO.API.Models
{
    public class DepartmentEventResidentAgencyResModel
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public List<ResidentAgency> ResidentAgencies { get; set; }
    }
}

using System;

namespace iDAS.Web.Areas.Problem.Models
{
    public class ProblemEmergencyDTO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemEmergencyName { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDefault { get; set; }
    }
}

using System;

namespace iDAS.Web.Areas.Problem.Models
{
    public class ProblemGroupDTO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemGroupName { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class ProblemStatusDTO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemStatusName { get; set; }
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDefault { get; set; }
    }
    public class ProblemStatusReqModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemStatusName { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}

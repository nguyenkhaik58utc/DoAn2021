using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class ProblemGroupDTO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemGroupName { get; set; }
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public bool IsSelected { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public int ParentID { get; set; }
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public List<ProblemTypeDTO> lstType { get; set; }

    }
    public class ProblemGroupReqModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemGroupName { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}

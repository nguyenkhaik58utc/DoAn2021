using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class BusinessModuleDTO
    {
        public int ID { get; set; }
        public string SystemCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsShow { get; set; }
        public bool IsDelete { get; set; }
        public int Position { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }

    }
    public class BusinessModuleReqModel
    {
        public int PageSize { get; set; } = int.MaxValue;
        public int PageNumber { get; set; } = 1;
        public string SystemCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsShow { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }
}

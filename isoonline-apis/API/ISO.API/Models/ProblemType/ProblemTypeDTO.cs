using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class ProblemTypeDTO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemTypeName { get; set; }
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool IsDefault { get; set; }
        public bool IsSelect { get; set; }
    }
}

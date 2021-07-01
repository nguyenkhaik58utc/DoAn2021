using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.ManagementLevel
{
    public class ManagementLevelDTO
    {
        public int ID { get; set; }
        public string ManagementLevelCode { get; set; }
        public string ManagementLevelName { get; set; }
        public int Rank { get; set; }
        public bool IsDelete { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}

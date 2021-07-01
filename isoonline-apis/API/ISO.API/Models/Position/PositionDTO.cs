using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.Position
{
    public class PositionDTO
    {
        public int ID { get; set; }
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public int? ManagementLevelID { get; set; }
        public string ManagementLevelName { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}

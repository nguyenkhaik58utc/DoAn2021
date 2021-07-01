using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.ShiftHistory
{
    public partial class ShiftHistoryDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Status { get; set; }
        public int ProblemEventTotal { get; set; }
    }
}

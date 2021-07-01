using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.ShiftHistory
{
    public class ShiftHistoryReqModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ToUserID { get; set; }
        public DateTime ShiftTime { get; set; }
    }
}

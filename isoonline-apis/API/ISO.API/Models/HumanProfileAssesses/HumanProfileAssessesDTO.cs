using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanProfileAssesses
{
    public class HumanProfileAssessesDTO
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Score { get; set; }
        public string Result { get; set; }
        public string Note { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}

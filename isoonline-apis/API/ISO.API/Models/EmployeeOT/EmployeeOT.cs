using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class EmployeeOT
    {
        public int ID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int EmployeeID { get; set; }

        public string Reason { get; set; }

        public int Status { get; set; }
        public int ApprovedBy { get; set; }
        public int Level { get; set; }

        public bool IsDelete { get; set; }

        public int UserID { get; set; }
    }
}

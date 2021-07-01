using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items.Problem
{
    public class ProblemEmergencyItem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemEmergencyName { get; set; }
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public string Color { get; set; }
        public bool IsDefault { get; set; }
        public string textDefault { get; set; }
    }
}

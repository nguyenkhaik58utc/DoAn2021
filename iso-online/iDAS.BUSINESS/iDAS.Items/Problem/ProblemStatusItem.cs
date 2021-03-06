using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items.Problem
{
    public class ProblemStatusItem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemStatusName { get; set; }
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }

        public bool IsDefault { get; set; }
        public string textDefault { get; set; }
    }
}

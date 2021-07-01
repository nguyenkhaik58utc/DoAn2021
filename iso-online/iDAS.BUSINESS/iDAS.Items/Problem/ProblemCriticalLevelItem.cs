using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items.Problem
{
    public class ProblemCriticalLevelItem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string CriticalLevelName { get; set; }
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string Color { get; set; }
        public bool IsDefault { get; set; }
        public string textDefault { get; set; }
    }
}

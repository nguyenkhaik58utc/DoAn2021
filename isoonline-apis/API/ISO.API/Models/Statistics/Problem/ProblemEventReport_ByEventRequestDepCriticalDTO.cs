using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class ProblemEventReport_ByEventRequestDepCriticalDTO
    {
        public int CriticalLevelID { get; set; }
        public string CriticalLevelName { get; set; }
        public int Count { get; set; }
        public int RequestDepId { get; set; }
        public string Name { get; set; }
    }
}

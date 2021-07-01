using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class ProblemCriticalEventRequestDepResModel
    {
        public int ProblemCriticalLevelID { get; set; }
        public string ProblemCriticalLevelName { get; set; }
        public List<EventReqDep> EventRequestDep { get; set; }
    }
}

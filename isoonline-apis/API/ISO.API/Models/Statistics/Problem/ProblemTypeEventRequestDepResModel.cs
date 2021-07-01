using System.Collections.Generic;

namespace ISO.API.Models
{
    public class ProblemTypeEventRequestDepResModel
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public List<EventReqDep> EventRequestDep { get; set; }
    }
}

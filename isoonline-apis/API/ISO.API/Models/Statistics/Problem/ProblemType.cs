using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class ProblemType
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public List<Event> Events { get; set; }
        public int EventCount { get; set; }
    }
}

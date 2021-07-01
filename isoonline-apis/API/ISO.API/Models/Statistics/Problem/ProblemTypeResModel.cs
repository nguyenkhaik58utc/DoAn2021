using System.Collections.Generic;

namespace ISO.API.Models
{
    public class ProblemTypeResModel
    {
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public List<Dep> Departments { get; set; }
    }
}

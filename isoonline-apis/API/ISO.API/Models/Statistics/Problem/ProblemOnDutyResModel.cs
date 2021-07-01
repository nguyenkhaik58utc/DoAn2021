using System.Collections.Generic;

namespace ISO.API.Models
{
    public class ProblemOnDutyResModel
    {
        public bool OnDuty { get; set; }
        public List<Dep> Departments { get; set; }
    }
}

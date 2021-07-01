using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class Dep
    {
        public int DepartmentID { get; set; }
        public string DepName { get; set; }
        public List<Event> Events { get; set; }
        public int EventCount { get; set; }
    }
}

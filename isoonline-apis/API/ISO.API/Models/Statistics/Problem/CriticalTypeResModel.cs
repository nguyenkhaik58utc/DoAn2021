using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class CriticalTypeResModel
    {
        public int CriticalLevelID { get; set; }
        public string CriticalLevelName { get; set; }
        public List<Dep> Departments { get; set; }
    }
}

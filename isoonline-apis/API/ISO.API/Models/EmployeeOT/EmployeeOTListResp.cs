using System.Collections.Generic;

namespace ISO.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class EmployeeOTListResp
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }

        public List<EmployeeOT> lstEmployee { get; set; }

        public EmployeeOTListResp()
        {
            lstEmployee = new List<EmployeeOT>();
        }
    }
}

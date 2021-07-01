using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanProfileWorkTrial
{
    public class HumanProfileWorkTrialDTO
    {
        public int HumanEmployeeID { get; set; }
        public string HumanEmployeeName  { get; set; }
        public string DepartmentTitle  { get; set; }
        public string DepartmentName  { get; set; }
        public string TitleName  { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsAccept { get; set; }
        public bool IsEdit { get; set; }
        public bool IsApproval { get; set; }
    }
}

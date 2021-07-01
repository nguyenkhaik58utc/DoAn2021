using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanAuditTickResultCriteriaItem
    {
        public int ID { get; set; }
        public int CriteriaID { get; set; }
        public string Criteria { get; set; }
        public string CriteriaPoint { get; set; }
        public int CriteriaPointID { get; set; }
        public bool IsEmployeeSelect { get; set; }
        public bool IsManagementSelect { get; set; }
        public bool IsLeaderSelect { get; set; }
        public decimal Point { get; set; }

        public int? Factory { get; set; }
    }
}

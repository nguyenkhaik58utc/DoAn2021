using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class QualityAuditItem 
    {
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public int ISOStandardID { get; set; }
        public int QualityAuditPlanID { get; set; }
        public int EmployeeID { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public bool IsPass { get; set; }
        public System.DateTime Time { get; set; }
        public string ISOStandardName { get; set; }
        public string DepartmentName { get; set; }
        public HumanEmployeeViewItem Audit { get; set; }
        public int Obs { get; set; }
        public int Maximum { get; set; }
        public int Medium { get; set; }
        public string Note { get; set; }
    }
       
}

using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class AuditItem
    {
        public int ID { get; set; }
        public int QualityCriteriaCategoryID { get; set; }
        public string Content { get; set; }
        public int TaskID { get; set; }
        public int DepartmentID { get; set; }
        public int QualityAuditPlanID { get; set; }
        public int EmployeeID { get; set; }
        public int ISOStandardID { get; set; }
        public int ISOIndexID { get; set; }
        public string ISOIndexName { get; set; }
        public bool IsPass { get; set; }
        public System.DateTime Time { get; set; }
        public string ISOStandardName { get; set; }
        public string DepartmentName { get; set; }
        public HumanEmployeeViewItem Audit { get; set; }
        public int Obs { get; set; }
        public int Maximum { get; set; }
        public int Medium { get; set; }
        public string Note { get; set; }
        public string Name { get; set; }
        public string Result 
        {
            get
            {
                var result = string.Empty;                
                    result = IsPass ? "<span style=\"color:green\">Đạt</span>" : "<span style=\"color:red\">Không đạt</span>";  
                return result;
            }
        }
    }
}

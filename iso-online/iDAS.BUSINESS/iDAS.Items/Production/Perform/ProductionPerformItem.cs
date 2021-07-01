using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductionPerformItem
    {
        public int ID { get; set; }
        public int ProductionCommandID { get; set; }
        public string CommandCode { get; set; }
        public HumanEmployeeViewItem HumanEmployee { get; set; }
        public HumanDepartmentViewItem HumanDepartment { get; set; }
        public int ProductionShiftID { get; set; }
        public DateTime Date { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ShiftName { get; set; }

        public string ActionForm { get; set; }
    }
}

using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class EquipmentMeasureCalibrationItem
    {
        public int ID { get; set; }
        public int EquipmentMeasureID { get; set; }
        public int EquipmentCalibrationID { get; set; }
        public string EquipmentCalibrationName { get; set; }
        public HumanEmployeeViewItem HumanEmployee { get; set; }
        public bool IsPass { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }

        public string ActionForm { get; set; }
    }
}

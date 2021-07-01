using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class EquipmentCalibrationItem
    {
        public int ID { get; set; }
        public int EquipmentMeasureID { get; set; }
        public string Name { get; set; }
        public string AccreditationContent { get; set; }
        public Nullable<int> AccreditationPeriodic { get; set; }
        public Nullable<System.DateTime> AccreditationLastTime { get; set; }
        public Nullable<System.DateTime> AccreditationNextTime { get; set; }
        public string AccreditationBy { get; set; }
        public bool IsPass { get; set; }
        public Nullable<bool> Time { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }

        public string ActionForm { get; set; }
    }
}

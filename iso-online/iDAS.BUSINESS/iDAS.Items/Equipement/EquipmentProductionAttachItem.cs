using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class EquipmentProductionAttachItem
    {
        public int ID { get; set; }
        public int EquipmentProductionID { get; set; }
        public int EquipmentProductionName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string MadeIn { get; set; }
        public Nullable<System.DateTime> MadeYear { get; set; }
        public string Specifications { get; set; }
        public string Features { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsUsing { get; set; }
        public bool IsError { get; set; }
        public bool IsFixed { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }

        public string ActionForm { get; set; }
    }
}

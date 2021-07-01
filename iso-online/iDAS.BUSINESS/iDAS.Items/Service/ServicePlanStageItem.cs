using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ServicePlanStageItem
    {
        public int ID { get; set; }
        public int ServicePlanID { get; set; }
        public int ServiceStageID { get; set; }
        public int CustomerID { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public int Order { get; set; }
        public string StageName { get; set; }
        public string ServiceName { get; set; }
    }
}

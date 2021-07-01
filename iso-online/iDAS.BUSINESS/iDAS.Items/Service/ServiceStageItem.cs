using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
  public class ServiceStageItem
    {
        public int ID { get; set; }
        public int ServiceID { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string ServiceName { get; set; }
        public Nullable<float> Time { get; set; }
        public Boolean IsUse { get; set; }
        public Nullable<int> TimeUnitID { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Boolean IsSelected { get; set; }
    
    }
}

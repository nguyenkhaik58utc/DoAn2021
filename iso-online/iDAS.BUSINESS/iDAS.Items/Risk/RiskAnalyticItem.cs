using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class RiskAnalyticItem
    {
        public int ID { get; set; }
        public int RiskID { get; set; }
        public int Ability { get; set; }
        public int Level { get; set; }
        public Nullable<int> Value { get; set; }
        public string Management { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    }
}

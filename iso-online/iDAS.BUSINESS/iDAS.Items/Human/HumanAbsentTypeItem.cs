using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanAbsentTypeItem
    {
        public HumanAbsentTypeItem()
        {
            this.HumanAbsents = new HashSet<HumanAbsentItem>();
        }
    
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double IsPayed { get; set; }
        public int MaxDayAbsent { get; set; }
        public string Note { get; set; }
    
        public virtual ICollection<HumanAbsentItem> HumanAbsents { get; set; }
    }
}

using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class TaskCalendarEventsItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Note { get; set; }
        public bool IsWorking { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string UpdateByName { get; set; }
        public DateTime UpdateByTime { get; set; }
    }
}

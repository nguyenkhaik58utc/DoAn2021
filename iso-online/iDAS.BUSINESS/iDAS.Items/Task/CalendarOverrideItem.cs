using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class CalendarOverrideItem
    {
        public int ID { get; set; }
        public int CalendarCategoryID { get; set; }
        public int TaskCalendarEventID { get; set; }
        public string Title { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool IsDayOverrides { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string TitleInfo { get; set; }
        public string EventName { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public string DaySet { get; set; }

    }
   public class ListTime
   {
       public string Name { get; set; }
       public string StartTime { get; set; }
       public string EndTime { get; set; }
   }
    public class InfoDate
    {
        public string Title { get; set; }
        public List<ListTime> Times { get; set; }
    }
   public class HighLightItem
   {
       public string Title { get; set; }
       public string Date { get; set; }
       public int StyleID { get; set; }
   }
}

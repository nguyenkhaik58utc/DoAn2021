using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class CalendarTimeOverrideSV
    {
        private CalendarTimeOverrideDA calendarTimeOverrideDA = new CalendarTimeOverrideDA();
        public List<CalendarTimeOverrideItem> GetByCalendarOverride(int page, int pageSize, out int total, int id)
        {
            var data = calendarTimeOverrideDA.GetQuery(t => t.CalendarOverrideID == id).OrderByDescending(t => t.CreateAt).Page(page, pageSize, out total).ToList();
           List<CalendarTimeOverrideItem> lst = new List<CalendarTimeOverrideItem>();
           if (data.Count > 0)
           {
               foreach (var t in data)
               {
                   lst.Add(new CalendarTimeOverrideItem()
                   {
                       CalendarOverrideID = t.CalendarOverrideID,
                       EndTime = t.EndTime,
                       ID = t.ID,
                       Name = t.Name,
                       StartTime = t.StartTime,
                       CreateAt = t.CreateAt
                   });
               }
           }
         return lst;
        }
        public void Update(int id,int calendarId, string name, string startTime, string endTime, int userId)
        {
            if (id!=0)
            {
                var obj = calendarTimeOverrideDA.GetById(id);
                obj.StartTime = startTime;
                obj.CalendarOverrideID = calendarId;
                obj.Name = name;
                obj.EndTime = endTime;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = userId;
                calendarTimeOverrideDA.Update(obj);
                calendarTimeOverrideDA.Save();
            }
            else
            {
                var objnew = new CalendarTimeOverride();
                objnew.StartTime =startTime;
                objnew.EndTime = endTime;
                objnew.Name = name;
                objnew.CalendarOverrideID = calendarId;
                objnew.CreateAt = DateTime.Now;
                objnew.CreateBy = userId;
                calendarTimeOverrideDA.Insert(objnew);
                calendarTimeOverrideDA.Save();
            }
        }
        public void InsertRange(List<CalendarTimeOverrideItem> items, int userID)
        {
            var rprs = new List<CalendarTimeOverride>();
            rprs = items.Select(item => new CalendarTimeOverride()
            {
                StartTime =item.StartTime,
                EndTime = item.EndTime,
                Name = item.Name,
                CalendarOverrideID = item.CalendarOverrideID,   
                CreateAt = DateTime.Now,
                CreateBy = userID
            }).ToList();
            calendarTimeOverrideDA.InsertRange(rprs);
            calendarTimeOverrideDA.Save();
        }
        public void Delete(int id)
        {
            var obj = calendarTimeOverrideDA.GetById(id);
            calendarTimeOverrideDA.Delete(obj);
            calendarTimeOverrideDA.Save();
        }
    }
}

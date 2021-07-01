using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using Ext.Net;
using System.Globalization;


namespace iDAS.Services
{
    public class CalendarOverrideSV
    {
        private CalendarOverrideDA calendarOverrideDA = new CalendarOverrideDA();
        public List<ListTime> GetTime(int calendarOverrideId)
        {
            var dbo = calendarOverrideDA.Repository;
            List<ListTime> lst = new List<ListTime>();
            var data = dbo.CalendarTimeOverrides.Where(t => t.CalendarOverrideID == calendarOverrideId).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new ListTime()
                    {
                        Name = item.Name,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime
                    });
                }
            }
            return lst;
        }
        public InfoDate GetDateInfo(int calendarOverrideId)
        {
            var dbo = calendarOverrideDA.Repository;
            List<ListTime> lst = new List<ListTime>();
            var date = dbo.CalendarOverrides.Where(t=>t.ID==calendarOverrideId).FirstOrDefault();
            var data = dbo.CalendarTimeOverrides.Where(t => t.CalendarOverrideID == calendarOverrideId).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new ListTime()
                    {
                        Name = item.Name,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime
                    });
                }
            }
            InfoDate xxx = new InfoDate();
            xxx.Title = date.StartDate.ToString("dddd") + " ngày " + date.StartDate.ToString("dd/MM/yyyy") + " thuộc ";
            xxx.Title += date.TaskCalendarEvent != null ? date.TaskCalendarEvent.Name : string.Empty;
            xxx.Times = lst;
            return xxx;
        }

        public List<CalendarOverrideItem> GetDateOverride(int page, int pageSize, out int total, int calendarCategoryID)
        {
            List<CalendarOverrideItem> lst = new List<CalendarOverrideItem>();
            var data = calendarOverrideDA.GetQuery(t => t.CalendarCategoryID == calendarCategoryID && t.IsDayOverrides).OrderByDescending(t => t.CreateAt).Page(page, pageSize, out total).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new CalendarOverrideItem
                    {
                        ID = item.ID,
                        Title = item.Title,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        TitleInfo = item.StartDate.ToString("dddd") + " ngày " + item.StartDate.ToString("dd/MM/yyyy") + " thuộc ",
                        EventName = item.TaskCalendarEvent != null ? item.TaskCalendarEvent.Name : string.Empty
                    });

                }
            }
            return lst;
        }
        public bool CheckExitsDate(int cateId, DateTime date)
        {
            var obj = calendarOverrideDA.GetQuery(t => t.CalendarCategoryID == cateId && t.StartDate == date && t.EndDate == date && t.IsDayOverrides).FirstOrDefault();
            if (obj != null)
                return true;
            return false;
        }
        public bool CheckExitsDateEdit(int id, int cateId, DateTime date)
        {
            var obj = calendarOverrideDA.GetQuery(t => t.CalendarCategoryID == cateId && t.StartDate == date && t.EndDate == date && t.IsDayOverrides && t.ID != id).FirstOrDefault();
            if (obj != null)
                return true;
            return false;
        }
        public void Insert(int cateId, string title, DateTime date, int userId)
        {
            var obj = new CalendarOverride();
            obj.CalendarCategoryID = cateId;
            obj.Title = title;
            obj.StartDate = date;
            obj.EndDate = date;
            obj.IsDayOverrides = true;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            calendarOverrideDA.Insert(obj);
            calendarOverrideDA.Save();
        }
        public int InsertFromWeekendSet(int cateId, string title,int eventId,  DateTime date, int userId)
        {
            var obj = new CalendarOverride();
            obj.CalendarCategoryID = cateId;
            obj.Title = title;
            obj.StartDate = date;
            obj.TaskCalendarEventID = eventId;
            obj.EndDate = date;
            obj.IsDayOverrides = true;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            calendarOverrideDA.Insert(obj);
            calendarOverrideDA.Save();
            return obj.ID;
        }
        public void Update(int id, string title, DateTime date, int userId)
        {
            var obj = calendarOverrideDA.GetById(id);
            obj.Title = title;
            obj.StartDate = date;
            obj.EndDate = date;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            calendarOverrideDA.Update(obj);
            calendarOverrideDA.Save();
        }
        public void UpdateEvent(int id, int eventId, int userId)
        {
            var obj = calendarOverrideDA.GetById(id);
            obj.TaskCalendarEventID = eventId;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            calendarOverrideDA.Update(obj);
            calendarOverrideDA.Save();
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            calendarOverrideDA.DeleteRange(ids);
            calendarOverrideDA.Save();
        }

        public List<HighLightItem> GetDateHighLight(int cateId = 0)
        {
            var dbo = calendarOverrideDA.Repository;
            List<HighLightItem> dateNotWorking = new List<HighLightItem>();
            var data = calendarOverrideDA.GetQuery(t => t.CalendarCategoryID == cateId).ToList();
            foreach (var item in data)
            {
                if (item.StartDate.ToShortDateString() == item.EndDate.ToShortDateString())
                {
                    dateNotWorking.Add(new HighLightItem()
                    {
                        StyleID = item.TaskCalendarEventID.HasValue?item.TaskCalendarEventID.Value:0,
                        Date = item.StartDate.ToString("MM/dd/yyyy"),
                        Title = item.Title
                    });
                }
                else
                {
                    for (DateTime i = item.StartDate; i <= item.EndDate; i = i.AddDays(1))
                    {
                        dateNotWorking.Add(new HighLightItem()
                        {
                       
                            StyleID = item.TaskCalendarEventID.HasValue?item.TaskCalendarEventID.Value:0,
                            Date = i.ToString("MM/dd/yyyy"),
                            Title = item.Title
                        });
                    }
                }
            }
            return dateNotWorking;
        }

        public List<DisabledDate> GetDateNotWorking(int departmentId = 0)
        {
            var dbo = calendarOverrideDA.Repository;
            List<string> dateNotHighLight = new List<string>();
            List<DisabledDate> dateHighLight = new List<DisabledDate>();
            var departmentParentID = dbo.HumanDepartments.Where(t => t.ParentID == 0) != null ? dbo.HumanDepartments.Where(t => t.ParentID == 0).FirstOrDefault().ID : 0;
            var cateNotWorks = dbo.TaskCalendarEvents.Where(t => t.IsWorking).Select(t => t.ID).ToArray();
            var cateInWorks = dbo.TaskCalendarEvents.Where(t => !t.IsWorking).Select(t => t.ID).ToArray();
            var dataInWorking = calendarOverrideDA.GetQuery(t => t.CalendarCategory.HumanDepartmentID == departmentId && cateInWorks.Contains(t.TaskCalendarEventID.Value)).ToList();
            var dataNotWorking = calendarOverrideDA.GetQuery(t => t.CalendarCategory.HumanDepartmentID == departmentId && cateNotWorks.Contains(t.TaskCalendarEventID.Value)).ToList().Concat(calendarOverrideDA.GetQuery(t => t.CalendarCategory.HumanDepartmentID == departmentParentID && cateNotWorks.Contains(t.TaskCalendarEventID.Value)).ToList());
            foreach (var item in dataInWorking)
            {
                if (item.StartDate.ToShortDateString() == item.EndDate.ToShortDateString())
                {
                    dateNotHighLight.Add(item.StartDate.ToShortDateString());
                }
                else
                {
                    for (DateTime i = item.StartDate; i <= item.EndDate; i = i.AddDays(1))
                    {
                        dateNotHighLight.Add(i.ToShortDateString());
                    }
                }
            }
            foreach (var item in dataNotWorking)
            {

                if (item.StartDate.ToShortDateString() == item.EndDate.ToShortDateString())
                {
                    if (!dateNotHighLight.Contains(item.StartDate.ToShortDateString()))
                    {
                        dateHighLight.Add(new DisabledDate(item.StartDate.ToShortDateString()));
                    }
                }
                else
                {
                    for (DateTime i = item.StartDate; i <= item.EndDate; i = i.AddDays(1))
                    {
                        if (!dateNotHighLight.Contains(i.ToShortDateString()))
                        {
                            dateHighLight.Add(new DisabledDate(i.ToShortDateString()));
                        }
                    }
                }

            }
            return dateHighLight;
        }

        public CalendarOverrideItem GetByID(int id)
        {
            return calendarOverrideDA.GetQuery(t => t.ID == id).Select(t => new CalendarOverrideItem
            {
                ID = t.ID,
                Title = t.Title,
                StartDate = t.StartDate,
                TaskCalendarEventID = t.TaskCalendarEventID.HasValue ? t.TaskCalendarEventID.Value : 0,
                EndDate = t.EndDate,
            }).FirstOrDefault();
        }
        public int GetIDByDateAndCate(DateTime date, int cateId)
        {
            var data = calendarOverrideDA
                .GetQuery(t =>t.CalendarCategoryID == cateId)
                .Where(t=>t.StartDate==date)
                .FirstOrDefault();
            if (data != null)
                return data.ID;
            return 0;
        }
    }
}

using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Task.Controllers
{
    public class CalendarOverrideController : BaseController
    {
        //
        // GET: /Task/CalendarOverride/
        private CalendarOverrideSV calendarOverrideSV = new CalendarOverrideSV();
        private CalendarTimeOverrideSV calendarTimeOverrideSV = new CalendarTimeOverrideSV();
        public ActionResult LoadDataDateOverride(StoreRequestParameters parameters, int cateId)
        {
            int totalCount;
            var lst = calendarOverrideSV.GetDateOverride(parameters.Page, parameters.Limit, out totalCount, cateId);
            return this.Store(new Paging<CalendarOverrideItem>(lst, totalCount));
        }
        public ActionResult LoadDataTimeOverride(StoreRequestParameters parameters, int id)
        {
            int totalCount;
            var lst = calendarTimeOverrideSV.GetByCalendarOverride(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(new Paging<CalendarTimeOverrideItem>(lst, totalCount));
        }
        public ActionResult UpdateEvent(int id, int eventId)
        {
            calendarOverrideSV.UpdateEvent(id, eventId, User.ID);
            return this.Direct();
        }
        public ActionResult UpdateTime(int id, int calendarId, string name, string startTime, string endTime)
        {
            calendarTimeOverrideSV.Update(id, calendarId, name, startTime, endTime, User.ID);
            X.GetCmp<Store>("stCalendarTimeOverride").Reload();
            return this.Direct();
        }
        public ActionResult DeleteTime(int id)
        {
            try
            {
                calendarTimeOverrideSV.Delete(id);
                X.GetCmp<Store>("stCalendarTimeOverride").Reload();
                return this.Direct();
            }
            catch (Exception)
            {
                return this.Direct("Error");
            }
        }
        public ActionResult GetDateHighLight(int cateId)
        {
            List<HighLightItem> dates = new List<HighLightItem>();
            dates = new CalendarOverrideSV().GetDateHighLight(cateId);
            return this.Direct(dates);
        }
        public ActionResult SetWeekendCalendar(CalendarOverrideItem obj, int cateId, string arrTime)
        {
            if (obj.ToDate <= obj.FromDate)
            {
                Ultility.CreateMessageBox("Thông báo", "Từ ngày phải < Đến ngày", MessageBox.Icon.INFO);
                return this.Direct(false);
            }
            else
            {
                var lstTime = Ext.Net.JSON.Deserialize<object[]>(arrTime);
                TimeSpan diff = obj.ToDate - obj.FromDate;
                int days = diff.Days;
                for (int i = 0; i < days; i++)
                {
                    var dateWeekend = obj.FromDate.AddDays(i);
                    switch (dateWeekend.DayOfWeek)
                    {

                        case DayOfWeek.Saturday:
                            if (obj.DaySet != "Sunday")
                            {
                                var check = calendarOverrideSV.CheckExitsDate(cateId, DateTime.Parse(dateWeekend.ToLongDateString()));
                                if (!check)
                                {
                                    int id = calendarOverrideSV.InsertFromWeekendSet(cateId, "Thứ 7 ngày " + dateWeekend.ToLongDateString(), obj.TaskCalendarEventID, DateTime.Parse(dateWeekend.ToLongDateString()), User.ID);
                                    if (lstTime.Length > 0)
                                    {
                                        var CalendarTimeOverrideItems = new List<CalendarTimeOverrideItem>();
                                        foreach (var item in lstTime)
                                        {
                                            var s = Ext.Net.JSON.Deserialize<CalendarTimeOverrideItem>(item.ToString());
                                            s.CalendarOverrideID = id;
                                            s.EndTime = s.EndTime;
                                            s.StartTime = s.StartTime;
                                            var CalendarTimeOverrideItem = new CalendarTimeOverrideItem();
                                            CalendarTimeOverrideItem.CalendarOverrideID = id;
                                            CalendarTimeOverrideItems.Add(s);
                                        }
                                        calendarTimeOverrideSV.InsertRange(CalendarTimeOverrideItems, User.ID);
                                    }
                                }
                            }
                            break;
                        case DayOfWeek.Sunday:
                            if (obj.DaySet != "Saturday")
                            {
                                var check = calendarOverrideSV.CheckExitsDate(cateId, DateTime.Parse(dateWeekend.ToLongDateString()));
                                if (!check)
                                {
                                    int id = calendarOverrideSV.InsertFromWeekendSet(cateId, "Chủ nhật ngày " + dateWeekend.ToLongDateString(), obj.TaskCalendarEventID, DateTime.Parse(dateWeekend.ToLongDateString()), User.ID);
                                    if (lstTime.Length > 0)
                                    {
                                        var CalendarTimeOverrideItems = new List<CalendarTimeOverrideItem>();
                                        foreach (var item in lstTime)
                                        {
                                            var s = Ext.Net.JSON.Deserialize<CalendarTimeOverrideItem>(item.ToString());
                                            s.CalendarOverrideID = id;
                                            s.EndTime = s.EndTime;
                                            s.StartTime = s.StartTime;
                                            var CalendarTimeOverrideItem = new CalendarTimeOverrideItem();
                                            CalendarTimeOverrideItem.CalendarOverrideID = id;
                                            CalendarTimeOverrideItems.Add(s);
                                        }
                                        calendarTimeOverrideSV.InsertRange(CalendarTimeOverrideItems, User.ID);
                                    }
                                }
                            }
                            break;

                    }
                }
                return this.Direct(true);
            }

        }
        public ActionResult GetTime(int id)
        {
            var result = calendarOverrideSV.GetTime(id);
            return this.Direct(result);
        }
        public ActionResult GetInfoDate(DateTime date, int cateId)
        {
            var id = calendarOverrideSV.GetIDByDateAndCate(date, cateId);
            if(id!=0)
            {
                var result = calendarOverrideSV.GetDateInfo(id);
                 return this.Direct(result);
            }
            else
            {
                return this.Direct("default");
            }
        }
    }
}

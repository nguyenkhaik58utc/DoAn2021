using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using Ext.Net;

namespace iDAS.Services
{
    public class TaskCalendarSV
    {
        private TaskCalendarDA calendarsDA = new TaskCalendarDA();
        private TaskCalendarEventSV calendarEventsService = new TaskCalendarEventSV();
        public List<TaskCalendarEventsItem> GetEventAll()
        {
            return calendarEventsService.GetAll();
        }
        public List<TaskCalendarItem> GetAll(int departmentId)
        {
            List<TaskCalendarItem> lst = new List<TaskCalendarItem>();
            var data = calendarsDA.GetQuery(t => t.HumanDepartmentID == departmentId)
                .ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    DateTime now = DateTime.Now.Date;
                    lst.Add(new TaskCalendarItem
                        {
                            EventId = item.ID,
                            CalendarId = item.TaskCalendarEventID,
                            Title = item.Title,
                            StartDate = item.StartTime,
                            EndDate = item.EndTime,
                            IsAllDay = false,
                            Notes = item.Note
                        });
                }
            }
            return lst;
        }
        public void Insert(int departmentId, string title, string startDate, string endDate, int calendarId)
        {
            TaskCalendar obj = new TaskCalendar();
            obj.Title = title;
            obj.StartTime = DateTime.Parse(startDate);
            obj.EndTime = DateTime.Parse(endDate);
            obj.Day = DateTime.Parse(startDate).Day;
            obj.HumanDepartmentID = departmentId;
            obj.TaskCalendarEventID = calendarId;
            obj.Month = DateTime.Parse(startDate).Month;
            obj.Note = "";
            obj.Year = DateTime.Parse(startDate).Year;
            calendarsDA.Insert(obj);
            calendarsDA.Save();
        }
        public void Update(int id, string title, string startDate, string endDate, int calendarId)
        {
            TaskCalendar obj = calendarsDA.GetById(id);
            if (obj == null)
            {
                obj = calendarsDA.GetQuery().OrderByDescending(t => t.ID).FirstOrDefault();
            }
            obj.Title = title;
            obj.StartTime = DateTime.Parse(startDate);
            obj.EndTime = DateTime.Parse(endDate);
            obj.Day = DateTime.Parse(startDate).Day;
            obj.TaskCalendarEventID = calendarId;
            obj.Month = DateTime.Parse(startDate).Month;
            obj.Note = "";
            obj.Year = DateTime.Parse(startDate).Year;
            calendarsDA.Update(obj);
            calendarsDA.Save();
        }
        public void Delete(int id)
        {
            var obj = calendarsDA.GetById(id);
            calendarsDA.Delete(obj);
            calendarsDA.Save();

        }
        public List<DisabledDate> GetDateClose(int departmentId=0)
        {
            var dbo = calendarsDA.Repository;
            List<string> dateClose = new List<string>();
            List<string> dateWork = new List<string>();
            List<DisabledDate> dateNotWorking = new List<DisabledDate>();
            var dateclose=new List<TaskCalendar>() ;
            var dateworking = new List<TaskCalendar>();
            if (departmentId == 0)
            {
               departmentId = dbo.HumanDepartments.FirstOrDefault(t => t.ParentID == 0) != null ? dbo.HumanDepartments.FirstOrDefault(t => t.ParentID == 0).ID : 0;
               dateclose = calendarsDA.GetQuery(t => t.TaskCalendarEvent.Name.Contains("nghỉ") && t.HumanDepartmentID == departmentId).ToList();
               dateworking = calendarsDA.GetQuery(t => t.TaskCalendarEvent.Name.Contains("làm") && t.HumanDepartmentID == departmentId).ToList();
            }
            else
            {
                dateclose = calendarsDA.GetQuery(t => t.TaskCalendarEvent.Name.Contains("nghỉ") && t.HumanDepartmentID == departmentId).ToList();
                dateworking = calendarsDA.GetQuery(t => t.TaskCalendarEvent.Name.Contains("làm") && t.HumanDepartmentID == departmentId).ToList();
            }
            if (dateclose.Count() > 0)
            {
                foreach (var item in dateclose)
                {
                    if (item.StartTime.ToShortDateString() == item.EndTime.ToShortDateString())
                    {
                        dateClose.Add(item.StartTime.ToShortDateString());
                    }
                    else
                    {
                        for (DateTime i = item.StartTime; i <= item.EndTime; i = i.AddDays(1))
                        {
                            dateClose.Add(i.ToShortDateString());
                        }
                    }
                }
            }
            dateClose.Distinct();
            if (dateworking.Count() > 0)
            {
                foreach (var item in dateworking)
                {
                    if (item.StartTime.ToShortDateString() == item.EndTime.ToShortDateString())
                    {
                        dateWork.Add(item.StartTime.ToShortDateString());
                    }
                    else
                    {
                        for (DateTime i = item.StartTime; i <= item.EndTime; i = i.AddDays(1))
                        {
                            dateWork.Add(i.ToShortDateString());
                        }
                    }
                }
            }
            dateWork.Distinct();
            foreach (var item in dateWork)
            {
                if (dateClose.Contains(item))
                {
                    dateClose.Remove(item);
                }
            }
            foreach (var item in dateClose)
            {
                dateNotWorking.Add(new DisabledDate(item));
            }
            return dateNotWorking;
        }

        public bool CheckDayOff(string strShortDate)
        {
            DateTime shortDate = DateTime.Parse(strShortDate);
            bool vcheck = false;
            var rs = calendarsDA.GetQuery(t => t.StartTime <= shortDate && t.EndTime >= shortDate).Count();
            if (rs > 0) { vcheck = true; } else { vcheck = false; }
            return vcheck;
        }
        public string TimeCalculator(DateTime startDate, DateTime endDate)
        {

            string sDate = startDate.ToShortDateString();
            string sHour = startDate.Hour.ToString();
            string sMinute = startDate.Minute.ToString();
            string tDate = endDate.ToShortDateString();
            string tHour = endDate.Hour.ToString();
            string tMinute = endDate.Minute.ToString();


           // var sx = startDate.ToString("dddd");

            sDate = sDate.Replace("\"", "");
            tDate = tDate.Replace("\"", "");
            if (sHour.Length < 2) sHour = "0" + sHour;
            if (sMinute.Length < 2) sMinute = "0" + sMinute;
            if (tHour.Length < 2) tHour = "0" + tHour;
            if (tMinute.Length < 2) tMinute = "0" + tMinute;
            string strCal = "";
            double TotalDay = 0;
            double TotalHour = 0;
            double TotalHourFix = 0;
            double WorkingHourInDay = 0;
            double toAmHour = 0;
            double toAmMinute = 0;
            double fromPmHour = 0;
            double fromPmMinute = 0;
            double RelaxTime = 0;
            string Time1, Time2, Time3, Time4, TimeBegin, TimeEnd;
            TimeSpan t1 = TimeSpan.Parse("00:00:00");
            TimeSpan t2 = TimeSpan.Parse("00:00:00");
            TimeSpan t3 = TimeSpan.Parse("00:00:00");
            TimeSpan t4 = TimeSpan.Parse("00:00:00");
            TimeSpan tbegin, tend, diff, ts;
            //var objTime = GetTimeConfig();
            string DayName = "";
            string cdate1, cdate2;
            TimeBegin = sHour + ":" + sMinute + ":00";
            TimeSpan vbegin = TimeSpan.Parse(TimeBegin);
            TimeEnd = tHour + ":" + tMinute + ":00";
            TimeSpan vend = TimeSpan.Parse(TimeEnd);
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("vi-VN", true);
            DateTime StartDate, FinishDate, Date1, Date2 = new DateTime();
            StartDate = DateTime.ParseExact(sDate.Substring(0, 10), "dd/MM/yyyy", theCultureInfo);
            FinishDate = DateTime.ParseExact(tDate.Substring(0, 10), "dd/MM/yyyy", theCultureInfo);
            StartDate = StartDate.Date.Add(new TimeSpan(Convert.ToInt32(sHour), Convert.ToInt32(sMinute), 0));
            FinishDate = FinishDate.Date.Add(new TimeSpan(Convert.ToInt32(tHour), Convert.ToInt32(tMinute), 0));
            diff = FinishDate - StartDate;
            double hdiff = diff.TotalDays;
            if (hdiff < 1)
            {
                if (!CheckDayOff(StartDate.ToString("dd/MM/yyyy")))
                {
                    DayName = StartDate.ToString("dddd"); 
                    DayName = DayName.ToUpper();
                    //foreach (var item in objTime)
                    //{
                    //    if (item.DefineName == "MONDAY") WorkingHourInDay = (double)item.TotalHour;
                    //    if (item.DefineName == DayName)
                    //    {
                    //        TotalHourFix = (double)item.TotalHour;
                    //        toAmHour = (double)item.ToAMHour;
                    //        toAmMinute = (double)item.ToAMMinute;
                    //        fromPmHour = (double)item.FromPMHour;
                    //        fromPmMinute = (double)item.FromPMMinute;
                    //        Time1 = Convert.ToString(item.FromAMHour) + ":" + Convert.ToString(item.FromAMMinute) + ":00";
                    //        Time2 = Convert.ToString(item.ToAMHour) + ":" + Convert.ToString(item.ToAMMinute) + ":00";
                    //        Time3 = Convert.ToString(item.FromPMHour) + ":" + Convert.ToString(item.FromPMMinute) + ":00";
                    //        Time4 = Convert.ToString(item.ToPMHour) + ":" + Convert.ToString(item.ToPMMinute) + ":00";
                    //        t1 = TimeSpan.Parse(Time1);
                    //        t2 = TimeSpan.Parse(Time2);
                    //        t3 = TimeSpan.Parse(Time3);
                    //        t4 = TimeSpan.Parse(Time4);
                    //    }
                    //}
                    if (TotalHourFix > 0)
                    {
                        tbegin = vbegin;
                        tend = vend;
                        if (TimeSpan.Compare(vbegin, t1) == -1) tbegin = t1;
                        if (TimeSpan.Compare(vend, t1) == -1) tend = t1;
                        if (TimeSpan.Compare(vbegin, t4) == 1)
                        {
                            if (TimeSpan.Compare(t4, t3) == 1) tbegin = t4;
                        }
                        if (TimeSpan.Compare(vend, t4) == 1)
                        {
                            if (TimeSpan.Compare(t4, t3) == 1)
                            {
                                tend = t4;
                            }
                            else
                            {
                                if (TimeSpan.Compare(vend, t2) == 1) tend = t2;
                            }
                        }
                        if (TimeSpan.Compare(vbegin, t2) == 1 || TimeSpan.Compare(vbegin, t2) == 0 && TimeSpan.Compare(vbegin, t3) == -1) tbegin = t3;
                        if (TimeSpan.Compare(vend, t2) == 1 && TimeSpan.Compare(vend, t3) == -1 || TimeSpan.Compare(tend, t3) == 0) tend = t2;
                        cdate1 = StartDate.ToString("dd/MM/yyyy") + " " + tbegin;
                        Date1 = DateTime.ParseExact(cdate1, "dd/MM/yyyy H:mm:ss", theCultureInfo);
                        cdate2 = StartDate.ToString("dd/MM/yyyy") + " " + tend;
                        Date2 = DateTime.ParseExact(cdate2, "dd/MM/yyyy H:mm:ss", theCultureInfo);
                        ts = Date2 - Date1;
                        if (ts.TotalHours > 0)
                        {
                            TotalHour = ts.TotalHours;
                        }
                        else
                        {
                            TotalHour = 0;
                        }
                        if (TimeSpan.Compare(tbegin, t2) == -1 && TimeSpan.Compare(tend, t3) == 1)
                        {
                            cdate1 = StartDate.ToString("dd/MM/yyyy") + " " + t2;
                            Date1 = DateTime.ParseExact(cdate1, "dd/MM/yyyy H:mm:ss", theCultureInfo);
                            cdate2 = StartDate.ToString("dd/MM/yyyy") + " " + t3;
                            Date2 = DateTime.ParseExact(cdate2, "dd/MM/yyyy H:mm:ss", theCultureInfo);
                            ts = Date2 - Date1;
                            RelaxTime = ts.TotalHours;
                            if (RelaxTime > 0)
                            {
                                TotalHour = TotalHour - RelaxTime;
                            }
                        }
                    }
                    else
                    {
                        TotalHour = 0;
                    }
                }
                else
                {
                    TotalHour = 0;
                }
            }
            if (hdiff > 1)
            {
                int i = 0;
                DateTime tempDate = new DateTime();
                do
                {
                    tbegin = vbegin;
                    tend = vend;
                    RelaxTime = 0;
                    tempDate = StartDate.AddDays(i);
                    DayName = tempDate.ToString("dddd");
                    DayName = DayName.ToUpper();
                    //foreach (var item in objTime)
                    //{
                    //    if (item.DefineName == "MONDAY") WorkingHourInDay = (double)item.TotalHour;
                    //    if (item.DefineName == DayName)
                    //    {
                    //        TotalHourFix = (double)item.TotalHour;
                    //        toAmHour = (double)item.ToAMHour;
                    //        toAmMinute = (double)item.ToAMMinute;
                    //        fromPmHour = (double)item.FromPMHour;
                    //        fromPmMinute = (double)item.FromPMMinute;
                    //        Time1 = Convert.ToString(item.FromAMHour) + ":" + Convert.ToString(item.FromAMMinute) + ":00";
                    //        Time2 = Convert.ToString(item.ToAMHour) + ":" + Convert.ToString(item.ToAMMinute) + ":00";
                    //        Time3 = Convert.ToString(item.FromPMHour) + ":" + Convert.ToString(item.FromPMMinute) + ":00";
                    //        Time4 = Convert.ToString(item.ToPMHour) + ":" + Convert.ToString(item.ToPMMinute) + ":00";
                    //        t1 = TimeSpan.Parse(Time1);
                    //        t2 = TimeSpan.Parse(Time2);
                    //        t3 = TimeSpan.Parse(Time3);
                    //        t4 = TimeSpan.Parse(Time4);
                    //    }
                    //}
                    if (!CheckDayOff(tempDate.ToString("dd/MM/yyyy")))
                    {
                        if (TotalHourFix > 0)
                        {
                            if (TimeSpan.Compare(vbegin, t1) == -1) tbegin = t1;
                            if (TimeSpan.Compare(vbegin, t4) == 1)
                            {
                                if (TimeSpan.Compare(t4, t3) == 1) tbegin = t4;
                            }
                            if (TimeSpan.Compare(vbegin, t2) == 0)
                            {
                                if (TimeSpan.Compare(t4, t3) == 1)
                                {
                                    tbegin = t3;
                                }
                                else
                                {
                                    tbegin = t4;
                                }
                            }
                            if (TimeSpan.Compare(vend, t1) == -1) tend = t1;
                            if (TimeSpan.Compare(vend, t4) == 1)
                            {
                                if (TimeSpan.Compare(t4, t3) == 1)
                                {
                                    tend = t4;
                                }
                                else
                                {
                                    tend = t2;
                                }
                            }
                            if (TimeSpan.Compare(vend, t3) == 0)
                            {
                                tend = t2;
                            }
                            if (tempDate.ToString("dd/MM/yyyy") == StartDate.ToString("dd/MM/yyyy") || tempDate.ToString("dd/MM/yyyy") == FinishDate.ToString("dd/MM/yyyy"))
                            {
                                cdate1 = StartDate.ToString("dd/MM/yyyy") + " " + tbegin;
                                Date1 = DateTime.ParseExact(cdate1, "dd/MM/yyyy H:mm:ss", theCultureInfo);
                                cdate2 = StartDate.ToString("dd/MM/yyyy") + " " + tend;
                                Date2 = DateTime.ParseExact(cdate2, "dd/MM/yyyy H:mm:ss", theCultureInfo);
                                ts = Date2 - Date1;
                                if (ts.TotalHours > 0)
                                {
                                    TotalHour = TotalHour + ts.TotalHours;
                                }
                                if (TimeSpan.Compare(tbegin, t2) == -1 && TimeSpan.Compare(tend, t3) == 1)
                                {
                                    cdate1 = StartDate.ToString("dd/MM/yyyy") + " " + t2;
                                    Date1 = DateTime.ParseExact(cdate1, "dd/MM/yyyy H:mm:ss", theCultureInfo);
                                    cdate2 = StartDate.ToString("dd/MM/yyyy") + " " + t3;
                                    Date2 = DateTime.ParseExact(cdate2, "dd/MM/yyyy H:mm:ss", theCultureInfo);
                                    ts = Date2 - Date1;
                                    RelaxTime = ts.TotalHours;
                                    if (RelaxTime > 0)
                                    {
                                        TotalHour = TotalHour - RelaxTime;
                                    }
                                }
                            }
                            else
                            {
                                if (tempDate > StartDate && tempDate < FinishDate)
                                {
                                    TotalHour = TotalHour + TotalHourFix;
                                }
                            }
                        }
                    }
                    i++;
                } while (DateTime.Compare(StartDate.AddDays(i), FinishDate) < 0);
            }
            if (TotalHour > 0)
            {
                TotalDay = TotalHour / WorkingHourInDay;
            }
            else
            {
                TotalDay = 0;
            }
            strCal = TotalDay.ToString("0.##") + "$" + TotalHour.ToString("0.##");
            return strCal;
        }

    }
}

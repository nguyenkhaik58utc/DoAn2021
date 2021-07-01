using Ext.Net;
using iDAS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Items;
using iDAS.Base;

namespace iDAS.Services
{
    public class TaskCalendarEventSV
    {
        private TaskCalendarEventDA calendarEventsDA = new TaskCalendarEventDA();
        public List<TaskCalendarEventsItem> GetAll()
        {
            List<TaskCalendarEventsItem> lst = new List<TaskCalendarEventsItem>();
            var data = calendarEventsDA.GetQuery().ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new TaskCalendarEventsItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        IsWorking = item.IsWorking,
                        Color =item.Color                        
                    });
                }
            }
            return lst;
        }
        public List<TaskCalendarEventsItem> GetAll(int page, int pageSize, out int total)
        {
            var dbo = calendarEventsDA.Repository;
            List<TaskCalendarEventsItem> lst = new List<TaskCalendarEventsItem>();
            var data = calendarEventsDA.GetQuery().OrderByDescending(t => t.CreateAt).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    DateTime? updatetime;
                    string updator;
                    if (item.UpdateBy == null)
                    {
                        updator = dbo.HumanUsers.FirstOrDefault(t => t.ID == item.UpdateBy).HumanEmployee.Name;
                        updatetime = item.CreateAt;
                    }
                    else
                    {
                        updator = null;
                        updatetime = (DateTime)item.UpdateAt;
                    }
                    lst.Add(new TaskCalendarEventsItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Color = item.Color,
                        Note = item.Note,
                        IsActive = item.IsActive,
                        CreateBy = item.CreateBy,
                        IsWorking = item.IsWorking,
                        CreateAt = item.CreateAt,
                        UpdateBy = item.UpdateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateByName = updator,
                        UpdateByTime = (DateTime)updatetime
                    });
                }
            }
            return lst;
        }
        public List<TaskCalendarEventsItem> GetAllIsUse(int page, int pageSize, out int total)
        {
            var dbo = calendarEventsDA.Repository;
            List<TaskCalendarEventsItem> lst = new List<TaskCalendarEventsItem>();
            var data = calendarEventsDA.GetQuery(t => t.IsActive && !t.IsDelete).OrderByDescending(t => t.CreateAt).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {

                    lst.Add(new TaskCalendarEventsItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Color = item.Color,
                        IsWorking = item.IsWorking
                    });
                }
            }
            return lst;
        }
        public void Insert(string name, string color, bool isworking, bool inused, string des, int userId)
        {
            TaskCalendarEvent objItem = new TaskCalendarEvent
            {
                Name = name.Trim(),
                Color = color,
                Note = des,
                IsWorking= isworking,
                IsActive = inused,
                CreateAt = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
                UpdateAt = DateTime.Now
            };
            calendarEventsDA.Insert(objItem);
            calendarEventsDA.Save();

        }
        public void Update(int id, string name, string des, string color,bool isworking, bool inused, int userId)
        {
            var objNewItem = calendarEventsDA.GetById(id);
            objNewItem.Name = name.Trim();
            objNewItem.Color = color;
            objNewItem.Note = des;
            objNewItem.IsWorking = isworking;
            objNewItem.IsActive = inused;
            objNewItem.UpdateAt = DateTime.Now;
            objNewItem.UpdateBy = userId;
            calendarEventsDA.Update(objNewItem);
            calendarEventsDA.Save();
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            calendarEventsDA.DeleteRange(ids);
            calendarEventsDA.Save();
        }
        public int GetFirst()
        {
            var rs = calendarEventsDA.GetQuery().FirstOrDefault();
            if (rs != null)
                return rs.ID;
            return 0;
        }
        public bool CheckNameExits(string name)
        {
            var rs = (calendarEventsDA.GetQuery().Where(t => t.Name.ToUpper() == name.ToUpper())).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckNameEditExits(int id, string name)
        {
            var rs = (calendarEventsDA.GetQuery().Where(t => t.Name.ToUpper() == name.ToUpper())).FirstOrDefault();
            if (rs != null && rs.ID != id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

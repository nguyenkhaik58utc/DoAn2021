using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class CustomerContactCalendarSV
    {
        private CustomerContactCalendarDA calendarDA = new CustomerContactCalendarDA();
        /// <summary>
        /// Thiết lập lịch hẹn
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Appointment(CustomerContactCalendarItem item, int userID)
        {
            var customerCalendar = calendarDA.GetQuery(i => i.CustomerID == item.CustomerID).FirstOrDefault();
            if (customerCalendar != null)
            {
                customerCalendar.Time = item.Time;
                customerCalendar.CustomerContactFormID = item.CustomerContactFormID;
                customerCalendar.Note = item.Note;
                customerCalendar.Content = item.Content;
                customerCalendar.IsNew = item.IsNew;
                customerCalendar.CreateAt = DateTime.Now;
                customerCalendar.CreateBy = userID;
                calendarDA.Save();
            }
            else
            {
                var insert = new CustomerContactCalendar();
                insert.CustomerID = item.CustomerID;
                insert.Time = item.Time;
                insert.CustomerContactFormID = item.CustomerContactFormID;
                insert.Note = item.Note;
                insert.Content = item.Content;
                insert.IsNew = item.IsNew;
                insert.CreateAt = DateTime.Now;
                insert.CreateBy = userID;
                calendarDA.Insert(insert);
                calendarDA.Save();
            }
           
        }
        public CustomerContactCalendarItem GetAppointment(int customerId, int employeeId)
        {
            var dbo = calendarDA.Repository;
            var result = new CustomerContactCalendarItem() { CustomerID = customerId };
            var appointment = dbo.CustomerContactCalendars.Where(i => i.CustomerID == customerId)
               .Select(item => new CustomerContactCalendarItem
               {
                   ID = item.ID,
                   CustomerContactFormName = item.Customer == null ? string.Empty : item.CustomerContactForm.Name,
                   CustomerContactFormID = item.CustomerContactFormID,
                   CustomerID = item.CustomerID,
                   Note = item.Note,
                   Time = item.Time,
                   Content = item.Content,
                   IsNew = item.IsNew,
                   CreateAt = item.CreateAt,
                   AppointmentEmployee = new HumanEmployeeViewItem()
                           {
                               ID = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy) != null ? (int)dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployeeID : 0,
                               Name = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployee.Name,
                               Role = dbo.HumanOrganizations.FirstOrDefault(m => dbo.HumanUsers.FirstOrDefault(u => u.HumanEmployeeID == m.HumanEmployeeID).ID == item.CreateBy).HumanRole.Name,
                               Department = dbo.HumanOrganizations.FirstOrDefault(m => dbo.HumanUsers.FirstOrDefault(u => u.HumanEmployeeID == m.HumanEmployeeID).ID == item.CreateBy).HumanRole.HumanDepartment.Name,
                           },
               }).FirstOrDefault();
            if (appointment != null) result = appointment;
            if (result.AppointmentEmployee == null || result.AppointmentEmployee.ID == 0)
            {
                result.AppointmentEmployee = new iDAS.Items.HumanEmployeeViewItem()
                            {
                                ID = employeeId,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == employeeId).Name,
                                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId) == null ?
                                        "" : dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.Name,
                                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId) == null ?
                                            "" : dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.HumanDepartment.Name
                            };
            }
            return result;
        }

        public List<CustomerContactCalendarViewItem> GetByUser(int userId)
        {
            List<CustomerContactCalendarViewItem> lst = new List<CustomerContactCalendarViewItem>();
            var data = calendarDA.GetQuery(c =>c.CreateBy.HasValue && c.CreateBy == userId).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    DateTime now = DateTime.Now.Date;
                    lst.Add(new CustomerContactCalendarViewItem
                    {
                        EventId = item.ID,
                        CalendarId = item.CustomerContactFormID,
                        Title = item.Customer.Name,
                        StartDate = item.Time,
                        EndDate = item.Time,
                        Day= item.Time.Value.Day,
                        IsAllDay = true,
                        Notes = item.Note
                    });
                }
            }
            return lst;
        }

        public void Insert(int customerId, string time,string note, int calendarId,int userId)
        {
            var insert = new CustomerContactCalendar();
            insert.CustomerID = customerId;
            insert.Time = DateTime.Parse(time);
            insert.CustomerContactFormID = calendarId;
            insert.Note = note;
            insert.IsNew = true;
            insert.CreateAt = DateTime.Now;
            insert.CreateBy = userId;
            calendarDA.Insert(insert);
            calendarDA.Save();
        }

        public void Update(int id, string startDate,string note, int calendarId,int userId)
        {
            var customerCalendar = calendarDA.GetQuery(i => i.ID == id).FirstOrDefault();
            customerCalendar.Time = DateTime.Parse(startDate);
            customerCalendar.CustomerContactFormID = calendarId;
            customerCalendar.Note = note;
            customerCalendar.UpdateAt = DateTime.Now;
            customerCalendar.UpdateBy = userId;
            calendarDA.Save();
        }
        public void Move(int id, string startDate, int userId)
        {
            var customerCalendar = calendarDA.GetQuery(i => i.ID == id).FirstOrDefault();
            customerCalendar.Time = DateTime.Parse(startDate);
            customerCalendar.UpdateAt = DateTime.Now;
            customerCalendar.UpdateBy = userId;
            calendarDA.Save();
        }
        public void Delete(int id)
        {
            calendarDA.Delete(id);
            calendarDA.Save();
        }

    }
}

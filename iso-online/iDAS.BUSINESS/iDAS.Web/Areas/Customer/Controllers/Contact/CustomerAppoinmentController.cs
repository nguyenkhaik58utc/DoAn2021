using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using System.IO;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Customer.Controllers
{
    //Thông tin người liên hệ của khách hàng
    [SystemAuthorize(CheckAuthorize = false)]
    public class CustomerAppoinmentController : BaseController
    {
        private CustomerContactCalendarSV contactCalendarSV = new CustomerContactCalendarSV();
        private CustomerFormSV customerformSV = new CustomerFormSV();
        public ActionResult Appoinment(int id = 0, string name = "", bool isReadOnly = false)
        {
            var data = customerformSV.GetActive();
            string style = "";
            foreach (var item in data)
            {
                style += ".ext-color-" + item.ID + ",";
                style += ".ext-ie .ext-color-" + item.ID + "-ad,";
                style += ".ext-opera .ext-color-" + item.ID + "-ad {color: " + item.Color + "}";
                style += ".ext-cal-day-col .ext-color-" + item.ID + ",";
                style += ".ext-dd-drag-proxy .ext-color-" + item.ID + ",";
                style += ".ext-color-" + item.ID + "-ad,";
                style += ".ext-color-" + item.ID + "-ad .ext-cal-evm,";
                style += ".ext-color-" + item.ID + " .ext-cal-picker-icon,";
                style += ".ext-color-" + item.ID + "-x dl,";
                style += ".ext-color-" + item.ID + "-x .ext-cal-evb {background: " + item.Color + "}";
                style += ".ext-color-" + item.ID + "-x .ext-cal-evb,";
                style += ".ext-color-" + item.ID + "-x dl {border-color: " + item.Color + "}";
            }
            ViewBag.Style = style;
            ViewBag.CustomerId = id;
            ViewBag.Name = name;
            return View();
        }
        public ActionResult GetEvents(DateTime? start, DateTime? end)
        {
            var result = contactCalendarSV.GetByUser(User.ID);
            return this.Store(result);
        }
        public ActionResult GetCalendars()
        {
            var data = customerformSV.GetActive();
            return this.Store(data);
        }
        public ActionResult Insert(int customerId, string startDate, string note, int calendarId, int eventId = 0)
        {
            try
            {
                if (eventId == 0)
                {
                    contactCalendarSV.Insert(customerId, startDate, note, calendarId, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    contactCalendarSV.Update(eventId, startDate, note, calendarId, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw ex;
            }
        }
        public ActionResult Update(int id, string title, string startDate, string endDate, int calendarId)
        {
            try
            {
                contactCalendarSV.Move(id, startDate,User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError);
                throw ex;
            }
            return this.Direct();
        }
        public ActionResult Delete(int id)
        {
            try
            {
                contactCalendarSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                throw ex;
            }
            return this.Direct();
        }
        public ActionResult SubmitData(string data)
        {
            List<EventModel> events = JSON.Deserialize<List<EventModel>>(data);

            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "EventsViewer",
                ViewBag =
                {
                    Events = events
                }
            };
        }
        [DirectMethod(Namespace = "iDas")]
        public ActionResult ShowMsg(string msg)
        {
            X.Msg.Notify("Thông báo", msg).Show();
            return this.Direct();
        }
        public static SelectedRowCollection InitiallySelectedRows
        {
            get
            {
                return new SelectedRowCollection()
                {
                    new SelectedRow(0)             
                };
            }
        }
    }
}

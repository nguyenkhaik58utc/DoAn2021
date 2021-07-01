using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Web.Areas.Timekeeping.Models.TimeKeeping;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Timekeeping.Controllers
{
    [BaseSystem(Name = "Danh sách giải trình dữ liệu chấm công", Icon = "Clock", IsActive = true, IsShow = true)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class TimeKeepingExplanationController : BaseController
    {
        private TimeKeepingAPI _timeKeepingAPI = new TimeKeepingAPI();

        // GET: Timekeeping/TimeKeepingExplanation
        public ActionResult Index()
        {
            ViewBag.Month = DateTime.Now.Month;
            ViewBag.Year = DateTime.Now.Year;
            return View();
        }

        public ActionResult GetDataExplanation(StoreRequestParameters param, int Status = -1, string curMonth = default(string))
        {
            string TimeStartWork = ConfigurationManager.AppSettings["TimeStartWork"];
            string TimeEndWork = ConfigurationManager.AppSettings["TimeEndWork"];
            DateTime Month = DateTime.MinValue;
            if (!string.IsNullOrEmpty(curMonth))
            {
                Month = DateTime.Parse(curMonth);
            }
            if (param is null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            GetTimeKeepingExplanationByMonthRequest request = new GetTimeKeepingExplanationByMonthRequest()
            {
                Month = Month.Month,
                Year = Month.Year,
                Status = Status
            };

            List<TimeKeepingExplanationViewModel> data = new List<TimeKeepingExplanationViewModel>();
            data = _timeKeepingAPI.GetExplanationByMonth(request).Result.Data;
            foreach (var item in data)
            {
                DateTime RoleIn = DateTime.ParseExact(item.DateCheck.ToString("dd/MM/yyyy " + TimeStartWork), "dd/MM/yyyy HH:mm:ss", null);
                DateTime RoleOut = DateTime.ParseExact(item.DateCheck.ToString("dd/MM/yyyy " + TimeEndWork), "dd/MM/yyyy HH:mm:ss", null);
                //Đi muộn
                if (item.TimeIn1 != "V" && item.TimeIn1 != "F" && item.TimeIn1 != "--" && !string.IsNullOrEmpty(item.TimeIn1))
                {
                    DateTime In = DateTime.ParseExact(item.DateCheck.ToString("dd/MM/yyyy " + item.TimeIn1 + ":00"), "dd/MM/yyyy HH:mm:ss", null);
                    if (In.Subtract(RoleIn).Minutes > 0)
                    {
                        if (string.IsNullOrWhiteSpace(item.Reason))
                            item.Reason = "Đi muộn " + In.Subtract(RoleIn).TotalMinutes + " phút";
                        else
                            item.Reason += ", Đi muộn " + In.Subtract(RoleIn).TotalMinutes + " phút";
                    }
                }
                //Về sớm
                if (!string.IsNullOrEmpty(item.TimeOut1))
                {
                    DateTime Out = DateTime.ParseExact(item.DateCheck.ToString("dd/MM/yyyy " + item.TimeOut1 + ":00"), "dd/MM/yyyy HH:mm:ss", null);
                    if (RoleOut.Subtract(Out).Minutes > 0)
                    {
                        if (string.IsNullOrWhiteSpace(item.Reason))
                            item.Reason = "Về sớm " + RoleOut.Subtract(Out).TotalMinutes + " phút";
                        else
                            item.Reason += ", Về sớm " + RoleOut.Subtract(Out).TotalMinutes + " phút";
                    }
                }
                //Vắng KP
                if (item.TimeIn1 == "V")
                {
                    if (string.IsNullOrWhiteSpace(item.Reason))
                        item.Reason = "Vắng không phép";
                    else
                        item.Reason += ", Vắng không phép";
                }
            }
            return this.Store(data);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = false;
            if (ModelState.IsValid)
            {
                try
                {
                    int rs = _timeKeepingAPI.DeleteExplanation(id).Result.Data;
                    result = true;
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess, error: false);
                }
                catch (Exception)
                {
                    Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                }
            }
            return this.Direct(result: result);
        }
    }
}
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Web.Areas.Timekeeping.Models;
using iDAS.Web.Areas.Timekeeping.Models.TimeKeeping;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Timekeeping.Controllers
{
    [BaseSystem(Name = "Dữ liệu chấm công thô", Icon = "Clock", IsActive = true, IsShow = true)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class OriginalTimeKeepingController : BaseController
    {
        private TimeKeepingAPI _timeKeepingAPI = new TimeKeepingAPI();

        // GET: Timekeeping/OriginalTimeKeeping
        public ActionResult Index()
        {
            ViewBag.Month = DateTime.Now.Month;
            ViewBag.Year = DateTime.Now.Year;
            return View();
        }

        public ActionResult GetDataNotHandingConfig(StoreRequestParameters param, string curMonth = default(string), string employeeId = default(string))
        {
            //var day = ConfigurationManager.AppSettings["TimeKeepingFromdate"];
            DateTime month = DateTime.MinValue;
            int curEmployeeId = -1;
            if (!string.IsNullOrEmpty(curMonth))
            {
                month = DateTime.Parse(curMonth);
            }

            if (!string.IsNullOrEmpty(employeeId))
            {
                curEmployeeId = int.Parse(employeeId);
            }

            GetTimeKeepingByMonthRequest request = new GetTimeKeepingByMonthRequest()
            {
                //EmployeeId = User.EmployeeID,
                EmployeeId = curEmployeeId,
                Month = month.Month,
                Year = month.Year
            };

            var dataview = _timeKeepingAPI.GetByMonth(request).Result.Data;
            var tableview = TableView(month.Month, month.Year);
            for (int i = 0; i < dataview.Count; i++)
            {
                tableview.Rows.Add();
                tableview.Rows[i]["NameOnView"] = dataview[i].Name;
                tableview.Rows[i]["CodeView"] = dataview[i].Code;
                var lst = dataview[i].ListTimeKeeping;
                foreach (TimeKeepingViewModel item in lst)
                {
                    var days = item.DateCheck.Day;
                    tableview.Rows[i]["_" + days.ToString("00") + "_In"] = item.TimeIn;
                    tableview.Rows[i]["_" + days.ToString("00") + "_Out"] = item.TimeOut;
                }
            }
            return this.Store(tableview, tableview.Rows.Count);
        }

        public DataTable TableView(int month, int year)
        {
            var Days = DateTime.DaysInMonth(year, month);
            DataTable dataTable = new DataTable();
            // List<string> days = new List<string> { "29", "30", "31" };
            for (int i = 0; i < Days; i++)
            {
                string Day = i.ToString("00");
                dataTable.Columns.Add("_" + Day + "_In");
                dataTable.Columns.Add("_" + Day + "_Out");
                //days.Remove(Day);
            }
            //foreach (var item in days)
            //{
            //    dataTable.Columns.Add("_" + item + "_In");
            //    dataTable.Columns.Add("_" + item + "_Out");
            //}
            dataTable.Columns.Add("NameOnView");
            dataTable.Columns.Add("CodeView");
            dataTable.Columns.Add("NgayCong_NT");
            dataTable.Columns.Add("NgayCong_CT");
            dataTable.Columns.Add("GioCong_NT");
            dataTable.Columns.Add("GioCong_CT");
            dataTable.Columns.Add("VaoTre_Lan");
            dataTable.Columns.Add("VaoTre_Phut");
            dataTable.Columns.Add("RaSom_Lan");
            dataTable.Columns.Add("RaSom_Phut");
            dataTable.Columns.Add("TangCa_TC1");
            dataTable.Columns.Add("TangCa_TC2");
            dataTable.Columns.Add("TangCa_TC3");
            dataTable.Columns.Add("VangKP");
            dataTable.Columns.Add("OM");
            dataTable.Columns.Add("TS");
            dataTable.Columns.Add("R");
            dataTable.Columns.Add("Ro");
            dataTable.Columns.Add("P");
            dataTable.Columns.Add("F");
            dataTable.Columns.Add("CO");
            dataTable.Columns.Add("CD");
            dataTable.Columns.Add("H");
            dataTable.Columns.Add("CT");
            dataTable.Columns.Add("Le");
            return dataTable;
        }
    }
}
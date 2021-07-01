using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Web.API;
using iDAS.Web.Areas.Timekeeping.Models.TimeKeeping;
using System;
using System.Configuration;
using System.Data;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Timekeeping.Controllers
{
    [BaseSystem(Name = "Dữ liệu chấm công đã điều chỉnh", Icon = "Clock", IsActive = true, IsShow = true)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class TimeKeepingHandlingController : BaseController
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
            //thêm dòng gán dữ liệu vào từng dòng
            for (int i = 0; i < dataview.Count; i++)
            {
                tableview.Rows.Add();
                tableview.Rows[i]["NameOnView"] = dataview[i].Name;
                tableview.Rows[i]["CodeView"] = dataview[i].Code;
                var lst = dataview[i].ListTimeKeeping;
                DataHandlingViewModel model = new DataHandlingViewModel();
                //lặp dữ liệu chấm công hàng ngày của employee
                foreach (TimeKeepingViewModel item in lst)
                {
                    var days = item.DateCheck.Day;
                    tableview.Rows[i]["_" + days.ToString("00") + "_In"] = item.TimeIn;
                    tableview.Rows[i]["_" + days.ToString("00") + "_Out"] = item.TimeOut;
                    GetDataHandling(item.TimeIn, item.TimeOut, item.DateCheck, model);
                }
                //gán dữ liệu đã điều chỉnh

                tableview.Rows[i]["NgayCong_NT"] = model.NgayCong_NT;
                tableview.Rows[i]["NgayCong_CT"] = model.NgayCong_CT;
                tableview.Rows[i]["GioCong_NT"] = model.GioCong_NT;
                tableview.Rows[i]["GioCong_CT"] = model.GioCong_CT;
                tableview.Rows[i]["VaoTre_Lan"] = model.VaoTre_Lan;
                tableview.Rows[i]["VaoTre_Phut"] = model.VaoTre_Phut;
                tableview.Rows[i]["RaSom_Lan"] = model.RaSom_Lan;
                tableview.Rows[i]["RaSom_Phut"] = model.RaSom_Phut;
                tableview.Rows[i]["TangCa_TC1"] = model.TangCa_TC1;
                tableview.Rows[i]["TangCa_TC2"] = model.TangCa_TC2;
                tableview.Rows[i]["TangCa_TC3"] = model.TangCa_TC3;
                tableview.Rows[i]["VangKP"] = model.VangKP;
                tableview.Rows[i]["OM"] = model.OM;
                tableview.Rows[i]["TS"] = model.TS;
                tableview.Rows[i]["R"] = model.R;
                tableview.Rows[i]["Ro"] = model.Ro;
                tableview.Rows[i]["P"] = model.P;
                tableview.Rows[i]["F"] = model.F;
                tableview.Rows[i]["CO"] = model.CO;
                tableview.Rows[i]["CD"] = model.CD;
                tableview.Rows[i]["H"] = model.H;
                tableview.Rows[i]["CT"] = model.CT;
                tableview.Rows[i]["Le"] = model.Le;
            }
            return this.Store(tableview, tableview.Rows.Count);
        }

        /// <summary>
        /// Tạo bảng
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
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

        public void GetDataHandling(string TimeIn, string TimeOut, DateTime dateCheck, DataHandlingViewModel model)
        {
            string TimeStartWork = ConfigurationManager.AppSettings["TimeStartWork"];
            string TimeEndWork = ConfigurationManager.AppSettings["TimeEndWork"];
            DateTime RoleIn = DateTime.ParseExact(dateCheck.ToString("dd/MM/yyyy " + TimeStartWork), "dd/MM/yyyy HH:mm:ss", null);
            DateTime RoleOut = DateTime.ParseExact(dateCheck.ToString("dd/MM/yyyy " + TimeEndWork), "dd/MM/yyyy HH:mm:ss", null);
            //Ngày công and Giờ công
            if (TimeIn != "V" && TimeIn != "F" && TimeIn != "--" && !string.IsNullOrEmpty(TimeIn) && !string.IsNullOrEmpty(TimeOut))
            {
                DateTime In = DateTime.ParseExact(dateCheck.ToString("dd/MM/yyyy " + TimeIn + ":00"), "dd/MM/yyyy HH:mm:ss", null);
                DateTime Out = DateTime.ParseExact(dateCheck.ToString("dd/MM/yyyy " + TimeOut + ":00"), "dd/MM/yyyy HH:mm:ss", null);
                //Ngày công
                if (GetNameDayOfWeek(In) == "Chủ nhật" || GetNameDayOfWeek(In) == "Thứ bảy")
                    model.NgayCong_CT += 1;
                else
                    model.NgayCong_NT += 1;
                //Giờ công
                if (GetNameDayOfWeek(In) == "Chủ nhật" || GetNameDayOfWeek(In) == "Thứ bảy")
                    model.GioCong_CT += (int)Out.Subtract(In).TotalHours;
                else
                    model.GioCong_NT += (int)Out.Subtract(In).TotalHours;
            }
            //Đi muộn
            if (TimeIn != "V" && TimeIn != "F" && TimeIn != "--" && !string.IsNullOrEmpty(TimeIn))
            {
                DateTime In = DateTime.ParseExact(dateCheck.ToString("dd/MM/yyyy " + TimeIn + ":00"), "dd/MM/yyyy HH:mm:ss", null);
                if (In.Subtract(RoleIn).TotalMinutes > 0)
                {
                    model.VaoTre_Lan += 1;
                    model.VaoTre_Phut += (int)In.Subtract(RoleIn).TotalMinutes;
                }
            }
            //Về sớm
            if (!string.IsNullOrEmpty(TimeOut))
            {
                DateTime Out = DateTime.ParseExact(dateCheck.ToString("dd/MM/yyyy " + TimeOut + ":00"), "dd/MM/yyyy HH:mm:ss", null);
                if (RoleOut.Subtract(Out).TotalMinutes > 0)
                {
                    model.RaSom_Lan += 1;
                    model.RaSom_Phut += (int)RoleOut.Subtract(Out).TotalMinutes;
                }
            }
            //Vắng KP
            if (TimeIn == "V")
                model.VangKP += 1;
            //tăng ca
            if (TimeIn == "TC1")
                model.TangCa_TC1 += 1;
            else if (TimeIn == "TC2")
                model.TangCa_TC2 += 1;
            else if (TimeIn == "TC3")
                model.TangCa_TC3 += 1;

            if (TimeIn == "OM")
                model.OM += 1;
            if (TimeIn == "TS")
                model.TS += 1;
            if (TimeIn == "R")
                model.R += 1;
            if (TimeIn == "Ro")
                model.Ro += 1;
            if (TimeIn == "P")
                model.P += 1;
            if (TimeIn == "F")
                model.F += 1;
            if (TimeIn == "CO")
                model.CO += 1;
            if (TimeIn == "CD")
                model.CD += 1;
            if (TimeIn == "H")
                model.H += 1;
            if (TimeIn == "CT")
                model.CT += 1;
            if (TimeIn == "Le")
                model.Le += 1;
        }

        public class DataHandlingViewModel
        {
            public int NgayCong_CT { get; set; }
            public int NgayCong_NT { get; set; }
            public int GioCong_CT { get; set; }
            public int GioCong_NT { get; set; }
            public int VaoTre_Lan { get; set; }
            public int VaoTre_Phut { get; set; }
            public int RaSom_Lan { get; set; }
            public int RaSom_Phut { get; set; }
            public int VangKP { get; set; }
            public int TangCa_TC1 { get; set; }
            public int TangCa_TC2 { get; set; }
            public int TangCa_TC3 { get; set; }
            public int OM { get; set; }
            public int TS { get; set; }
            public int R { get; set; }
            public int Ro { get; set; }
            public int P { get; set; }
            public int F { get; set; }
            public int CO { get; set; }
            public int CD { get; set; }
            public int H { get; set; }
            public int CT { get; set; }
            public int Le { get; set; }
        }

        private string[] Name = new[] { "Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy" };

        private string GetNameDayOfWeek(DateTime date)
        {
            int day = (int)date.DayOfWeek;
            return Name[day];
        }
    }
}
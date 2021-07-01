using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web.API.Human;
using iDAS.Web.Areas.Human.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Human.Controllers
{
    public class DashboardController : Controller
    {
        HumanDashBoardAPI api = new HumanDashBoardAPI();
        // GET: Human/Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public StoreResult GetData(int year)
        {
            try
            {
                var lstData = api.GetAmountEmployeeByMothOfYear(year).Result;
                return new StoreResult(lstData.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StoreResult GetData2()
        {
            try
            {
                var lstData = api.GetStatisticalByEducationLevel().Result;
                return new StoreResult(lstData.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StoreResult GetData3()
        {
            try
            {
                var lstData = api.GetStatisticalByAge().Result;
                return new StoreResult(lstData.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StoreResult GetData4()
        {
            try
            {
                var lstData = api.GetStatisticalByGender().Result;
                foreach(var item in lstData)
                {
                    if (item.Name.Equals("False"))
                    {
                        item.Name = "Nam";
                    }
                    else
                        item.Name = "Nữ";
                }
                return new StoreResult(lstData.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StoreResult GetData5()
        {
            try
            {
                var lstData = api.GetStatisticalByContractType().Result;
                return new StoreResult(lstData.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetDataAward(StoreRequestParameters parameters)
        {
            try
            {
                int total = 0;
                var resp = api.GetDataAward().Result;
                return this.Store(resp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetDataNewEmployee(StoreRequestParameters parameters)
        {
            try
            {
                int total = 0;
                var resp = api.GetDataNewEmployee().Result;

                return this.Store(resp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetDataExpireDate(StoreRequestParameters parameters)
        {
            try
            {
                int total = 0;
                var resp = api.GetDataExpireDate().Result;

                return this.Store(resp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetAllCruitment(StoreRequestParameters parameters)
        {
            try
            {
                int total = 0;
                var resp = api.GetAllCruitment().Result;

                return this.Store(resp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetAllTraining(StoreRequestParameters parameters)
        {
            try
            {
                int total = 0;
                var resp = api.GetAllTraining().Result;

                return this.Store(resp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
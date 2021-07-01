using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;

namespace iDAS.Web.Areas.DevelopmentProduct.Controllers
{
    [BaseSystem(Name = "Thống kê yêu cầu phát triển sản phẩm", Icon = "ChartPie", IsActive = true, IsShow = true, Position = 1, Parent = GroupMenuStatisticalDevelopmentProductController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProductDevelopmentRequirementStatisticalController : BaseController
    {
        //
        // GET: /DevelopmentProduct/ProductDevelopmentRequirementStatistical/

        public ActionResult Index(string fromDate, string toDate)
        {
            return View();
        }
        public StoreResult GetDataColumn(string fromDate, string toDate)
        {
            DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = iDAS.Utilities.Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            return new StoreResult(ChartModel.GenerateDataProductDevelopment(searchFromDate, searchToDate));
        }
        public StoreResult GetDataPie(string fromDate, string toDate)
        {
            DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = iDAS.Utilities.Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            return new StoreResult(ChartModel.GenerateDataProductDevelopmentPie(searchFromDate, searchToDate));
        }
    }
}

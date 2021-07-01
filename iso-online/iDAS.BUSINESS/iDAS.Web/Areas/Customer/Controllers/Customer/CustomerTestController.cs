using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;
using iDAS.Web.API;

namespace iDAS.Web.Areas.Customer.Controllers
{
    [BaseSystem(Name = "Danh sách khách hàng", IsActive = true, IsShow = true, Position = 0, Icon = "ApplicationSideExpand")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class CustomerController : BaseController
    {
        private CustomerSV service = new CustomerSV();
        private CustomerAPI api = new CustomerAPI();

        #region View
        [BaseSystem(Name = "Xem", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            try
            {
                int total = 0;
                //var data = service.GetCustomers(parameters.Page, parameters.Limit, out total);
                var resp = api.GetCustomerAPI(parameters.Page, parameters.Limit).Result;
                var data = resp.lstCus;
                total = resp.total;
                return this.Store(data, total);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return this.Direct();
        }
        
        #endregion 
    }
}

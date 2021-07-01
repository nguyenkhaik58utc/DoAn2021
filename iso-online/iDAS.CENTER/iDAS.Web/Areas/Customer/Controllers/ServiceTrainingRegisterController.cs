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
using System.Text;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Customer.Controllers
{
    [BaseSystem(Name = "Đăng ký đào tạo", IsActive = true, IsShow = true, Position = 1, Icon = "ApplicationEdit", Parent = ServiceInfoRegisterController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ServiceTrainingRegisterController : BaseController
    {
        private CustomerRegisterTrainingSV CustomerRegisterTrainingSV = new CustomerRegisterTrainingSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var customer = CustomerRegisterTrainingSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(customer, totalCount);
        }
    }
}

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
    [BaseSystem(Name = "Đăng ký gói sản phẩm", IsActive = true, IsShow = true, Position = 1, Icon = "PackageStart", Parent = ProductInfoRegisterController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProductPakageRegisterController : BaseController
    {
        private CustomerRegisterPakageSV CustomerRegisterPakageSV = new CustomerRegisterPakageSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var customer = CustomerRegisterPakageSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(customer, totalCount);
        }
        public ActionResult SendPakageRegister(int id)
        {
            try
            {
                CustomerRegisterPakageSV.Send(id,User.ID);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
                X.GetCmp<Store>("StProductPakageRegister").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            return this.Direct();
        }
    }
}

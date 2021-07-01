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
    [BaseSystem(Name = "Đăng ký khối sản phẩm", IsActive = true, IsShow = true, Position = 2, Icon = "BoxPicture", Parent = ProductInfoRegisterController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProductBlockRegisterController : BaseController
    {
        private CustomerRegisterBlockSV CustomerRegisterBlockSV = new CustomerRegisterBlockSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var customer = CustomerRegisterBlockSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(customer, totalCount);
        }
    }
}

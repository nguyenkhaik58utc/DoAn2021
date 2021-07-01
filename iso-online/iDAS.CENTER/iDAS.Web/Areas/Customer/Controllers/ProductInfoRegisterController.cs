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
    [BaseSystem(Name = "Danh sách đăng ký sản phẩm", IsActive = true, IsShow = true, Position = 1, Icon = "ApplicationFormEdit",IsGroup=true)]
    public class ProductInfoRegisterController : BaseController
    {
        public const string Code = "ProductInfoRegister";
    }
}

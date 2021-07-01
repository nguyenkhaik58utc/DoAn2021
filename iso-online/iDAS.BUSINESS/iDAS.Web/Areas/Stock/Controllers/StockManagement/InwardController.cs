
using System;
using System.Collections.Generic;
using iDAS.Core;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Bảng kê nhập kho", Icon = "FolderImage", IsActive = true, IsShow = true, Parent = Stock_InwardController.Code, IsGroup = true)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class InwardController : BaseController
    {
        //
        // GET: /Stock/GroupList/     
        public const string Code = "Inward";    
    }
}


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
    [BaseSystem(Name = "Bảng kê tháo dỡ", Icon = "FolderPageWhite", IsActive = true, IsShow = true, Parent = AddinController.Code, IsGroup = true, Position = 6)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class BuildDismantleController : BaseController
    {
        //
        // GET: /Stock/Build/
        public const string Code = "BuildDismantle";    
    }
}

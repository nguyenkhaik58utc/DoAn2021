
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Bảng kê chuyển kho", Icon = "FolderPageWhite", IsActive = true, IsShow = true, Parent = AddinController.Code, Position = 2, IsGroup = true)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class TransferController : BaseController
    {
        public const string Code = "Transfer";
        //
        // GET: /Stock/Transfer/

        //

    }
}

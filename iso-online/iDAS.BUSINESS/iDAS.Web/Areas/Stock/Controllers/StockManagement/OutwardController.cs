
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
    [BaseSystem(Name = "Bảng kê xuất kho", IsActive = true, Icon = "FolderImage", IsShow = true, Parent = Stock_OutwardController.Code, IsGroup = true)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class OutwardController : BaseController
    {
        public const string Code = "Outward";   
        //
        // GET: /Stock/GroupList/        
       
    }
}

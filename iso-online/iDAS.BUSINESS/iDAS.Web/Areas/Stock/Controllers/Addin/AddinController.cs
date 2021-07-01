using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Stock.Controllers
{
   [BaseSystem(Name = "Tiện ích", Icon = "Plugin", IsActive = true, IsShow = true, Position = 3, IsGroup = true)]
   [SystemAuthorize(CheckAuthorize = false)]
    public class AddinController : BaseController
    {
        //
        // GET: /Stock/Addin/
       public const string Code = "Addin";    

    }
}

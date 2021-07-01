
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iDAS.Core;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Công cụ", Icon = "Wrench", IsActive = true, IsShow = true, Position = 2, IsGroup = true)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ToolController : BaseController
    {
        //
        // GET: /Stock/Addin/
       public const string Code = "Tool";    
    }
}
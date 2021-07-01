using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Quản lý tin tức", IsActive = true, IsShow = true, IsGroup = true, Position = 4, Icon = "PageWhite")]
    public class GroupNewsController : BaseController
    {
        public const string Code = "GroupNews";
    }
}
using Ext.Net;
using Ext.Net.MVC;
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
    [BaseSystem(Name = "Quản lý thư viện", IsActive = true, IsShow = true, IsGroup = true, Position = 5, Icon = "FolderUser")]
    public class GroupLibraryController : BaseController
    {
        public const string Code = "GroupLibrary";
    }
}
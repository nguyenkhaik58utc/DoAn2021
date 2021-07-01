using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Document.Controllers
{
    [BaseSystem(Name = "Kiểm soát tài liệu", Icon = "Page", IsActive = true, IsShow = true, Position =4, IsGroup = true)]
    public class GroupDocumentsController : BaseController
    {
        public const string Code = "GroupDocuments";
    }
}

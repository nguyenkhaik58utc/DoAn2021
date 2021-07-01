using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Human.Controllers
{
    [BaseSystem(Name = "Đào tạo nhân sự", Icon = "ApplicationForm", IsActive = true, IsShow = true, Position = 4, IsGroup = true)]
    public class GroupTrainingController : BaseController
    {
        public const string Code = "GroupTraining";
    }
}

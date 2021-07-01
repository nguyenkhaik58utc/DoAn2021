using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Human.Controllers.Absent
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Nghỉ phép cá nhân", Icon = "UserComment", IsActive = false, IsShow = false, Position = 3, Parent = GroupAbsentController.Code)]
    public class AbsentController : BaseController
    {
        //
        // GET: /Human/Absent/

        public ActionResult Index()
        {
            return View();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using System.IO;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Hồ sơ lý lịch", IsActive = true, IsShow = false, Parent = "ProfileEmployee")]
    public class ProfileCuriculmViateController : BaseController
    {
        private HumanProfileCuriculmViateSV profile = new HumanProfileCuriculmViateSV();
        [BaseSystem(Name = "Xem hồ sơ cá nhân")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ProfileForm(int id)
        {
            var result = new HumanEmployeeItem { ID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Profile", Model = result };
        }
        public ActionResult Index(int Id = 0)
        {
            var data = profile.GetByEmployeeId(Id);
            if (data != null)
                return View(data);
            else return View(new HumanProfileCuriculmViateItem { EmployeeID = Id });
        }
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Update(HumanProfileCuriculmViateItem updateData)
        {
            try
            {
                if (updateData.ID == 0)
                {
                    profile.Insert(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    profile.Update(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            return this.Direct();
        }
    }
}

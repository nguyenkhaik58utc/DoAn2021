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
using iDAS.Web.API.Human;

namespace iDAS.Web.Areas.Human.Controllers.Profile
{
  [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Hồ sơ lương", IsActive = false, IsShow = false, Parent = "ProfileEmployee")]
    public class V3_ProfileSalaryController : BaseController
    {
        private V3_HumanProfileSalaryAPI humanProfileSalaryAPI = new V3_HumanProfileSalaryAPI();
        private HumanProfileSalarySV profile = new HumanProfileSalarySV();
        #region   Hồ sơ lương
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index(int Id,int check)
        {
            ViewData["CheckPermission"] = check;
            var model = new HumanEmployeeItem
            {
                ID = Id
            };
            return View(model);
        }
        public ActionResult LoadProfileSalary(StoreRequestParameters parameters, int EmployeeID = 0)
        {
            int totalCount;
            var result = profile.GetAllByEmployeeId(parameters.Page, parameters.Limit, out totalCount, EmployeeID);
            return this.Store(result, totalCount);
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteProfileSalary(int id)
        {
            try
            {
                profile.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreProfileSalary").Reload();
            }
            return this.Direct();
        }
        #endregion
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int ID = 0, int EmployeeID = 0)
        {
            var data = new HumanProfileSalaryItem();
            if (ID != 0)
            {
                data = profile.GetById(ID);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
            }
            else
            {
                data.EmployeeID = EmployeeID;
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
            }

        }
        public ActionResult Update(HumanProfileSalaryItem updateData)
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
                X.GetCmp<Window>("winSalary").Close();
                X.GetCmp<Store>("StoreProfileSalary").Reload();
                return this.Direct();

            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return RedirectToAction("Index");
            }
        }
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int ID, int EmployeeID)
        {
            var data = profile.GetById(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }

        public ActionResult LoadProfileSalaryNotPaging(int EmployeeID)
        {
            var result = humanProfileSalaryAPI.GetByEmployeeID(EmployeeID).Result.Data;
            return this.Store(result);
        }

    }
}

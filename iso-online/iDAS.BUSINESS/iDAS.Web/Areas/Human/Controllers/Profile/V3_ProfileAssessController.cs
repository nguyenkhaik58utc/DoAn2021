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
    [BaseSystem(Name = "Hồ sơ đánh giá", IsActive = true, IsShow = false, Parent = "ProfileEmployee")]
    public class V3_ProfileAssessController : BaseController
    {
        private V3_HumanProfileAssessAPI humanProfileAssessAPI = new V3_HumanProfileAssessAPI();
        private HumanProfileAssessSV profile = new HumanProfileAssessSV();
        #region Hồ sơ đánh giá
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
        public ActionResult LoadProfileAssess(StoreRequestParameters parameters, int EmployeeID = 0)
        {
            int totalCount;
            var result = profile.GetAllByEmployeeId(parameters.Page, parameters.Limit, out totalCount, EmployeeID);
            return this.Store(result, totalCount);
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteProfileAssess(int id)
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
                X.GetCmp<Store>("StoreProfileAssess").Reload();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int ID, int EmployeeID)
        {
            var data = profile.GetById(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int ID = 0, int EmployeeID = 0)
        {
            var data = new HumanProfileAssessItem();
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateData"></param>
        /// <param name="exit"></param>
        /// <returns></returns>
        public ActionResult Update(HumanProfileAssessItem updateData)
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
                X.GetCmp<Window>("WinAssess").Close();
                X.GetCmp<Store>("StoreProfileAssess").Reload();
                return this.Direct();

            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return RedirectToAction("Index");
            }
        }
        
        #endregion

        public ActionResult LoadProfileAssessNotPaging(int EmployeeID)
        {
            var result = humanProfileAssessAPI.GetByEmployeeID(EmployeeID).Result.Data;
            return this.Store(result);
        }

    }
}

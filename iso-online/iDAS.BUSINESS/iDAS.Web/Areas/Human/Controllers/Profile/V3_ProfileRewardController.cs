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
using iDAS.Utilities;
using iDAS.Web.API.Human.V3_Category;
using iDAS.Web.API.Human;
using iDAS.Web.Areas.Human.Models;

namespace iDAS.Web.Areas.Human.Controllers.Profile
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Hồ sơ khen thưởng", IsActive = true, IsShow = false, Parent = "ProfileEmployee")]
    public class V3_ProfileRewardController : BaseController
    {

        private V3_AwardTypeAPI awardTypeAPI = new V3_AwardTypeAPI();
        private V3_HumanProfileRewardAPI humanProfileRewardAPI = new V3_HumanProfileRewardAPI();
        // private HumanProfileRewardSV profile = new HumanProfileRewardSV();

        #region   Hồ sơ khen thưởng
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int Id, int check)
        {
            ViewData["CheckPermission"] = check;
            var model = new V3_HumanProfileRewardResponse
            {
                ID = Id
            };
            return View(model);
        }

        public ActionResult LoadProfileReward(StoreRequestParameters parameters, int EmployeeID = 0)
        {
            int totalCount;
            var result = humanProfileRewardAPI.GetPagingByEmployeeID(parameters.Page, parameters.Limit, EmployeeID).Result;
            totalCount = result.TotalRows.Value;
            return this.Store(result.Data, totalCount);
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteProfileReward(int id)
        {
            try
            {
                humanProfileRewardAPI.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreProfileReward").Reload();
            }
            return this.Direct();
        }
        #endregion

        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int ID = 0, int EmployeeID = 0)
        {
            //danh sách hình thức khen thưởng
            ViewData["lstAwardType"] = awardTypeAPI.GetAll().Result.Data;
            var data = new V3_HumanProfileRewardDTO();
            if (ID != 0)
            {
                data = humanProfileRewardAPI.GetByID(ID).Result;
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data, ViewData = ViewData };
            }
            else
            {
                data.HumanEmployeeID = EmployeeID;
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data, ViewData = ViewData };
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateData"></param>
        /// <param name="exit"></param>
        /// <returns></returns>
        public ActionResult Update(V3_HumanProfileRewardDTO updateData)
        {
            try
            {
                if (updateData.ID == 0)
                {
                    humanProfileRewardAPI.Insert(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    humanProfileRewardAPI.Update(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                X.GetCmp<Window>("winReward").Close();
                X.GetCmp<Store>("StoreProfileReward").Reload();
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
            var data = humanProfileRewardAPI.GetDetailByID(ID).Result;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }

        public ActionResult LoadProfileRewardNotPaging(int EmployeeID)
        {
            var result = humanProfileRewardAPI.GetByEmployeeID(EmployeeID).Result.Data;
            return this.Store(result);
        }
    }
}

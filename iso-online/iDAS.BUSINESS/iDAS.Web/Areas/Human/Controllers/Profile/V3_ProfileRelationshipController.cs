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
    [BaseSystem(Name = "Hồ sơ quan hệ gia đình", IsActive = true, IsShow = false, Parent = "ProfileEmployee")]
    public class V3_ProfileRelationshipController : BaseController
    {
        private V3_FamilyRelationShipAPI familyRelationshipAPI = new V3_FamilyRelationShipAPI();
        private V3_HumanProfileRelationshipAPI humanProfileRelationshipAPI = new V3_HumanProfileRelationshipAPI();

        private HumanProfileRelationshipSV profile = new HumanProfileRelationshipSV();
        #region   Hồ sơ Quan hệ gia đình
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int Id, int check)
        {
            ViewData["CheckPermission"] = check;
            var model = new V3_HumanProfileRelationshipResponse
            {
                ID = Id
            };
            return View(model);
        }
        public ActionResult LoadProfileRelationship(StoreRequestParameters parameters, int EmployeeID = 0)
        {
            int totalCount;
            //var result = profile.GetAllByEmployeeId(parameters.Page, parameters.Limit, out totalCount, EmployeeID);
            var result = humanProfileRelationshipAPI.GetPagingByEmployeeID(parameters.Page, parameters.Limit, EmployeeID).Result;
            totalCount = result.TotalRows.Value;
            return this.Store(result.Data, totalCount);
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteProfileRelationship(int id)
        {
            try
            {
                humanProfileRelationshipAPI.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreProfileRelationship").Reload();
            }
            return this.Direct();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int ID = 0, int EmployeeID = 0)
        {
            //danh sách quan hệ
            ViewData["lstFamilyRelationShip"] = familyRelationshipAPI.GetAll().Result.Data;
            var data = new V3_HumanProfileRelationshipDTO();
            if (ID != 0)
            {
                data = humanProfileRelationshipAPI.GetByID(ID).Result;
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
        public ActionResult Update(V3_HumanProfileRelationshipDTO updateData)
        {
            try
            {
                if (updateData.ID == 0)
                {
                    humanProfileRelationshipAPI.Insert(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    humanProfileRelationshipAPI.Update(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                X.GetCmp<Window>("winRelationship").Close();
                X.GetCmp<Store>("StoreProfileRelationship").Reload();
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
            var data = humanProfileRelationshipAPI.GetDetailByID(ID).Result;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }

        public ActionResult LoadProfileRelationshipNotPaging(int EmployeeID)
        {
            var result = humanProfileRelationshipAPI.GetByEmployeeID(EmployeeID).Result.Data;
            return this.Store(result);
        }
    }
}

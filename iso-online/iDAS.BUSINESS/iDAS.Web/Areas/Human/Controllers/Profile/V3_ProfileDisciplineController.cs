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
    [BaseSystem(Name = "Hồ sơ kỷ luật", IsActive = true, IsShow = false, Parent = "ProfileEmployee")]
    public class V3_ProfileDisciplineController : BaseController
    {
        private V3_DisciplineCategoryAPI disciplineCategoryAPI = new V3_DisciplineCategoryAPI();
        private V3_HumanProfileDisciplineAPI humanProfileDisciplineAPI = new V3_HumanProfileDisciplineAPI();
        // private HumanProfileDisciplineSV profile = new HumanProfileDisciplineSV();
        #region   Hồ sơ Kỉ luật
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index(int Id, int check)
        {
            ViewData["CheckPermission"] = check;
            var model = new V3_HumanProfileDisciplineResponse
            {
                ID = Id
            };
            return View(model);
        }
        public ActionResult LoadProfileDiscipline(StoreRequestParameters parameters, int EmployeeID = 0)
        {
            int totalCount;
            //var result = profile.GetAllByEmployeeId(parameters.Page, parameters.Limit, out totalCount, EmployeeID);
            var result = humanProfileDisciplineAPI.GetPagingByEmployeeID(parameters.Page, parameters.Limit, EmployeeID).Result;
            totalCount = result.TotalRows.Value;
            return this.Store(result.Data, totalCount);
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult DeleteProfileDiscipline(int id)
        {
            try
            {
                humanProfileDisciplineAPI.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreProfileDiscipline").Reload();
            }
            return this.Direct();
        }
        #endregion
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới và sửa")]
        public ActionResult UpdateForm(int ID = 0, int EmployeeID = 0)
        {
            //danh sách loại hợp đồng
            ViewData["lstDisciplineCategory"] = disciplineCategoryAPI.GetAll().Result.Data;
            var data = new V3_HumanProfileDisciplineDTO();
            if (ID != 0)
            {
                data = humanProfileDisciplineAPI.GetByID(ID).Result;
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
        public ActionResult Update(V3_HumanProfileDisciplineDTO updateData)
        {
            try
            {
                if (updateData.ID == 0)
                {
                    humanProfileDisciplineAPI.Insert(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    humanProfileDisciplineAPI.Update(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                X.GetCmp<Window>("winDiscipline").Close();
                X.GetCmp<Store>("StoreProfileDiscipline").Reload();
                return this.Direct();

            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return RedirectToAction("Index");
            }
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult DetailForm(int ID, int EmployeeID)
        {
            var data = humanProfileDisciplineAPI.GetDetailByID(ID).Result;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }

        public ActionResult LoadProfileDisciplineNotPaging(int EmployeeID)
        {
            var result = humanProfileDisciplineAPI.GetByEmployeeID(EmployeeID).Result.Data;
            return this.Store(result);
        }
    }
}

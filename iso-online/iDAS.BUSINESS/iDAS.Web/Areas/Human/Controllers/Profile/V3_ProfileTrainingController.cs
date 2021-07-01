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
using iDAS.Web.API.Human;
using iDAS.Web.Areas.Human.Models;
using iDAS.Web.API.Human.V3_Category;

namespace iDAS.Web.Areas.Human.Controllers.Profile
{

   [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Hồ sơ đào tạo", IsActive = true, IsShow = false, Position = 1, Parent = "ProfileEmployee")]
    public class V3_ProfileTrainingController : BaseController
    {
        private V3_HumanProfileTrainingAPI humanProfileTrainingAPI = new V3_HumanProfileTrainingAPI();
        private V3_EducationResultAPI educationResultAPI = new V3_EducationResultAPI();
        private V3_EducationTypeAPI educationTypeAPI = new V3_EducationTypeAPI();
        private HumanProfileTrainingSV profile = new HumanProfileTrainingSV();

        #region   Hồ sơ quá trình đào tạo
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index(int Id,int check)
        {
            ViewData["CheckPermission"] = check;
            var model = new V3_HumanProfileTrainingResponse
            {
                ID = Id
            };
            return View(model);
        }
        public ActionResult LoadProfileTraining(StoreRequestParameters parameters, int EmployeeID = 0)
        {
            int totalCount;
            //var result = profile.GetAllByEmployeeId(parameters.Page, parameters.Limit, out totalCount, EmployeeID);


            var result1 = humanProfileTrainingAPI.GetPagingByEmployeeID(parameters.Page, parameters.Limit, EmployeeID).Result;
            totalCount = result1.TotalRows.Value;

            return this.Store(result1.Data, totalCount);
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult DeleteProfileTraining(int id)
        {
            try
            {
                humanProfileTrainingAPI.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreProfileTraining").Reload();
            }
            return this.Direct();
        }
        #endregion
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới và sửa")]
        public ActionResult UpdateForm(int ID = 0, int EmployeeID = 0)
        {
            //danh sách hình thức đào tạo
            ViewData["lstEducationType"] = educationTypeAPI.GetAll().Result.Data;
            //danh sách kết quả đào tạo
            ViewData["lstEducationResult"] = educationResultAPI.GetAll().Result.Data;
            var data = new V3_HumanProfileTrainingDTO();
            if (ID != 0)
            {
                data = humanProfileTrainingAPI.GetByID(ID).Result;
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
        public ActionResult Update(V3_HumanProfileTrainingDTO updateData)
        {
            try
            {
                if (updateData.ID == 0)
                {
                    humanProfileTrainingAPI.Insert(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    humanProfileTrainingAPI.Update(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                X.GetCmp<Window>("winTraining").Close();
                X.GetCmp<Store>("StoreProfileTraining").Reload();
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
        public ActionResult DetailForm(int ID)
        {
            var data = humanProfileTrainingAPI.GetDetailByID(ID).Result;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }

        public ActionResult LoadProfileTrainingNotPaging(int EmployeeID)
        {
            var result = humanProfileTrainingAPI.GetByEmployeeID(EmployeeID).Result.Data;
            return this.Store(result);
        }
    }
}

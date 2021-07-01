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
    [BaseSystem(Name = "Hồ sơ văn bằng", IsActive = true, IsShow = false, Parent = "ProfileEmployee")]
    public class V3_ProfileDiplomaController : BaseController
    {


        private V3_EducationFieldAPI educationFieldAPI = new V3_EducationFieldAPI();
        private V3_EducationTypeAPI educationTypeAPI = new V3_EducationTypeAPI();
        private V3_EducationOrgAPI educationOrgAPI = new V3_EducationOrgAPI();
        private V3_EducationLevelAPI educationLevelAPI = new V3_EducationLevelAPI();
        private V3_CertificateLevelAPI certificateLevelAPI = new V3_CertificateLevelAPI();
        private V3_HumanProfileDiplomaAPI humanProfileDiplomaAPI = new V3_HumanProfileDiplomaAPI();

       // private HumanProfileDiplomaSV profile = new HumanProfileDiplomaSV();
        #region   Hồ sơ Văn bằng
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int Id,int check)
        {
            ViewData["CheckPermission"] = check;
            var model = new V3_HumanProfileDiplomaResponse
            {
                ID = Id
            };
            return View(model);
        }
        public ActionResult LoadProfileDiploma(StoreRequestParameters parameters, int EmployeeID = 0)
        {
            int totalCount;

            var result = humanProfileDiplomaAPI.GetPagingByEmployeeID(parameters.Page, parameters.Limit, EmployeeID).Result;
            totalCount = result.TotalRows.Value;
            return this.Store(result.Data, totalCount);
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteProfileDiploma(int id)
        {
            try
            {
                humanProfileDiplomaAPI.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreProfileDiploma").Reload();
            }
            return this.Direct();
        }
        #endregion
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int ID = 0, int EmployeeID = 0)
        {
            //danh sách chuyên ngành
            ViewData["lstEducationField"] = educationFieldAPI.GetAll().Result.Data;
            //danh sách trình độ
            ViewData["lstEducationLevel"] = educationLevelAPI.GetAll().Result.Data;
            //danh sách hình thức đào tạo
            ViewData["lstEducationType"] = educationTypeAPI.GetAll().Result.Data;
            //danh sách xếp loại
            ViewData["lstCertificateLevel"] = certificateLevelAPI.GetAll().Result.Data;
            //danh sách nơi đào tạo
            ViewData["lstEducationOrg"] = educationOrgAPI.GetAll().Result.Data;
            var data = new V3_HumanProfileDiplomaDTO();
            if (ID != 0)
            {
                data = humanProfileDiplomaAPI.GetByID(ID).Result;
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
        public ActionResult Update(V3_HumanProfileDiplomaDTO updateData)
        {
            try
            {
                if (updateData.ID == 0)
                {
                    humanProfileDiplomaAPI.Insert(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    humanProfileDiplomaAPI.Update(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                X.GetCmp<Window>("winDiploma").Close();
                X.GetCmp<Store>("StoreProfileDiploma").Reload();
                return this.Direct();

            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return RedirectToAction("Index");
            }
        }
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int ID, int EmployeeID)
        {
            var data = humanProfileDiplomaAPI.GetDetailByID(ID).Result;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }

        public ActionResult LoadProfileDiplomaNotPaging(int EmployeeID)
        {
            var result = humanProfileDiplomaAPI.GetByEmployeeID(EmployeeID).Result.Data;
            return this.Store(result);
        }
    }
}

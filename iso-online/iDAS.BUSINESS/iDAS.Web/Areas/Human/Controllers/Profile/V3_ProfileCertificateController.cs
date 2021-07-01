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
    [BaseSystem(Name = "Hồ sơ chứng chỉ", IsActive = true, IsShow = false, Parent = "ProfileEmployee")]
    public class V3_ProfileCertificateController : BaseController
    {
        private V3_CertificateLevelAPI certificateLevelAPI = new V3_CertificateLevelAPI();
        private V3_CertificateTypeAPI certificateTypeAPI = new V3_CertificateTypeAPI();
        private V3_HumanProfileCertificateAPI humanProfileCertificateAPI = new V3_HumanProfileCertificateAPI();

        //private HumanProfileCertificateSV profile = new HumanProfileCertificateSV();
        #region   Hồ sơ chứng chỉ
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index(int Id,int check)
        {
            ViewData["CheckPermission"] = check;
            var model = new V3_HumanProfileCertificateResponse
            {
                ID = Id
            };
            return View(model);
        }
        public ActionResult LoadProfileCertificate(StoreRequestParameters parameters, int EmployeeID = 0)
        {
            int totalCount;
            var result = humanProfileCertificateAPI.GetPagingByEmployeeID(parameters.Page, parameters.Limit, EmployeeID).Result;
            totalCount = result.TotalRows.Value;
            return this.Store(result.Data, totalCount);
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult DeleteProfileCertificate(int id)
        {
            try
            {
                humanProfileCertificateAPI.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreProfileCertificate").Reload();
            }
            return this.Direct();
        }
        #endregion
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới và sửa")]
        public ActionResult UpdateForm(int ID = 0, int EmployeeID = 0)
        {
            //danh sách xếp loại chứng chỉ
            ViewData["lstCertificateLevel"] = certificateLevelAPI.GetAll().Result.Data;
            //danh sách loại chứng chỉ
            ViewData["lstCertificateType"] = certificateTypeAPI.GetAll().Result.Data;
            var data = new V3_HumanProfileCertificateDTO();
            if (ID != 0)
            {
                data = humanProfileCertificateAPI.GetByID(ID).Result;
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
        public ActionResult Update(V3_HumanProfileCertificateDTO updateData)
        {
            try
            {
                if (updateData.ID == 0)
                {
                    humanProfileCertificateAPI.Insert(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    humanProfileCertificateAPI.Update(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                X.GetCmp<Window>("WinCertificate").Close();
                X.GetCmp<Store>("StoreProfileCertificate").Reload();
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
            var data = humanProfileCertificateAPI.GetDetailByID(ID).Result;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }

        public ActionResult LoadProfileCertificateNotPaging(int EmployeeID)
        {
            var result = humanProfileCertificateAPI.GetByEmployeeID(EmployeeID).Result.Data;
            return this.Store(result);
        }
    }
}
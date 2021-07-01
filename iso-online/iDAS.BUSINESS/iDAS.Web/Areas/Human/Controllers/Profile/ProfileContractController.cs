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

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize=false)]
    [BaseSystem(Name = "Hồ sơ hợp đồng", IsActive = true, IsShow = false, Parent = "ProfileEmployee")]
    public class ProfileContractController : BaseController
    {
        private HumanProfileContractSV profile = new HumanProfileContractSV();
        #region   Hồ sơ Hợp đồng
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index(int Id)
        {
            var model = new HumanEmployeeItem
            {
                ID = Id
            };
            return View(model);
        }
        public ActionResult LoadProfileContract(StoreRequestParameters parameters, int EmployeeID = 0)
        {
            int totalCount;
            var result = profile.GetAllByEmployeeId(parameters.Page, parameters.Limit, out totalCount, EmployeeID);
            return this.Store(result, totalCount);
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult DeleteProfileContract(int id)
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
                X.GetCmp<Store>("StoreProfileContract").Reload();
            }
            return this.Direct();
        }
        #endregion
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới và sửa")]
        public ActionResult UpdateForm(int ID = 0, int EmployeeID = 0)
        {
            var data = new HumanProfileContractItem();
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
        public ActionResult Update(HumanProfileContractItem updateData)
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
                X.GetCmp<Window>("WinContract").Close();
                X.GetCmp<Store>("StoreProfileContract").Reload();
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
            var data = profile.GetById(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
    }
}

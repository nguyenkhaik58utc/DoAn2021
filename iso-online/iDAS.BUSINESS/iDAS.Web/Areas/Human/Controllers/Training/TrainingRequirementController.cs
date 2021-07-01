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
using iDAS.Web.Controllers;
namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize=false)]
    [BaseSystem(Name = "Đề nghị",Icon="ApplicationEdit",IsActive = true, IsShow = true, Position = 2, Parent = GroupTrainingController.Code)]
    public class TrainingRequirementController : BaseController
    {
        private HumanTrainingRequirementSV trainingRequirementSV = new HumanTrainingRequirementSV();
        #region Thông tin chung
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        /// <summary>
        /// Load yêu cầu đào tạo
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult LoadRequirement(StoreRequestParameters parameters, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            return this.Store(trainingRequirementSV.GetAll(filter, focusId), filter.PageTotal);
        }

        #endregion

        #region Thêm
        /// <summary>
        /// Trỏ đến form yêu cầu đào tạo
        /// </summary>
        /// <returns></returns>
        [BaseSystem(Name = "Thêm mới")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult AddForm()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Add" };
        }

        /// <summary>
        /// Thêm mới yêu cầu đào tạo
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public ActionResult Insert(HumanTrainingRequirementItem updateData)
        {
            try
            {
                trainingRequirementSV.Insert(updateData, User.ID);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            finally
            {

                X.GetCmp<Window>("winRequirementAdd").Close();
                X.GetCmp<Store>("StoreRequirement").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Sửa
        /// <summary>
        /// Trỏ đến form cập nhật yêu cầu đào tạo 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id = 0, string ActionForm = "")
        {
            var data = new HumanTrainingRequirementItem();
            if (id != 0)
            {
                data = trainingRequirementSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };

        }
        /// <summary>
        /// Cập nhật yêu cầu đào tạo
        /// </summary>
        /// <param name="updateData"></param>
        /// <param name="exit"></param>
        /// <returns></returns>
        public ActionResult Update(HumanTrainingRequirementItem updateData)
        {
            try
            {
                if (updateData.ID != 0)
                {
                    trainingRequirementSV.Update(updateData, User.ID);
                    
                    Ultility.CreateNotification(message: updateData.IsEdit ? RequestMessage.UpdateSuccess : RequestMessage.SendSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: updateData.IsEdit ? RequestMessage.UpdateError : RequestMessage.SendSuccess, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winRequirementUpdate").Close();
                X.GetCmp<Store>("StoreRequirement").Reload();

            }
            return this.Direct();
        }
        #endregion

        #region Xóa
        /// <summary>
        /// Xóa yêu cầu đào tạo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteRequirement(int id)
        {
            try
            {
                trainingRequirementSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRequirement").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Chi tiết
        /// <summary>
        /// Form chi tiết yêu cầu đào tạo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult DetailForm(int id)
        {
            var data = trainingRequirementSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Phê duyệt

        /// <summary>
        /// Trỏ đến form gửi duyệt, duyệt yêu cầu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ActionForm"></param>
        /// <returns></returns>
        public ActionResult ApproveForm(int id = 0)
        {
            var data = new HumanTrainingRequirementItem();
            if (id != 0)
            {
                data = trainingRequirementSV.GetById(id);
            }
            if(data.IsEdit)
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "SendApprove", Model = data };
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data };
        }
        /// <summary>
        /// Gửi duyệt yêu cầu
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public ActionResult SendAppproved(HumanTrainingRequirementItem updateData)
        {
            var success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    trainingRequirementSV.SendApprove(updateData, User.ID);
                    success = true;
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một đề nghị đào tạo nhân sự cần phê duyệt", updateData.CreateByName, updateData.Approval.ID, User, Common.TrainingRequirement, "TrainingRequirementID:" + updateData.ID.ToString());
                    Ultility.CreateNotification(message: RequestMessage.SendSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRequirement").Reload();

            }
            return this.FormPanel(success);
        }
        /// <summary>
        /// Phê duyệt yêu cầu
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public ActionResult Appproved(HumanTrainingRequirementItem updateData)
        {
            var success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    updateData.UpdateBy = User.ID;
                    trainingRequirementSV.Approve(updateData);
                    success = true;
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Đề nghị đào tạo của bạn đã được phê duyệt", updateData.CreateByName, updateData.CreateUserID, User, Common.TrainingRequirement, "TrainingRequirementID:" + updateData.ID.ToString());
                    
                    Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
                    X.GetCmp<Store>("StoreRequirement").Reload();
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError, error: true);
            }
            return this.FormPanel(success);
        }
        #endregion
    }
}

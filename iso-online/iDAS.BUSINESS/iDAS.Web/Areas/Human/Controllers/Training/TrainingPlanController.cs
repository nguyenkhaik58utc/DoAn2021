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
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Kế hoạch", Icon = "ApplicationViewColumns", IsActive = true, IsShow = true, Position = 3, Parent = GroupTrainingController.Code)]
    public class TrainingPlanController : BaseController
    {
        private HumanTrainingPlanSV trainingPlanSV = new HumanTrainingPlanSV();
        private HumanTrainningPlanAttachSV trainingPlanAttachSv = new HumanTrainningPlanAttachSV();
        private QualityPlanSV planSV = new QualityPlanSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult LoadPlan(StoreRequestParameters parameters,int focusId = 0, int department = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var Data = trainingPlanSV.GetPlanByDepartment(filter,focusId, department);
            return this.Store(Data, filter.PageTotal);
        }

        #region Thêm
        [BaseSystem(Name = "Thêm mới")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult AddForm()
        {
            var data = new HumanTrainingPlanItem();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Add", Model = data };
        }
        public ActionResult Insert(HumanTrainingPlanItem updateData)
        {
            bool success = false;
            string tpl = "Uploaded file: {0}<br/>Size: {1} bytes";
            try
            {
                int idIns= trainingPlanSV.Insert(updateData, User.ID);
                success = true;
                if (this.GetCmp<FileUploadField>("FileUploadField1").HasFile)
                {
                    HumanTrainingPlanAttachmentItem objAttach = new HumanTrainingPlanAttachmentItem
                    {
                       HumanTrainingPlanID = idIns,
                       Name = this.GetCmp<FileUploadField>("FileUploadField1").PostedFile.FileName,
                       Size = this.GetCmp<FileUploadField>("FileUploadField1").PostedFile.ContentLength.ToString()
                    };
                    trainingPlanAttachSv.Insert(objAttach,User.ID);
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO,
                        Title = "Success",
                        Message = string.Format(tpl, this.GetCmp<FileUploadField>("FileUploadField1").PostedFile.FileName, this.GetCmp<FileUploadField>("FileUploadField1").PostedFile.ContentLength)
                    });
                }
                if(updateData.Approval.ID>0)
                {
                    var data = trainingPlanSV.GetById(idIns);
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một kế hoạch đào tạo cần phê duyệt", User.Name, updateData.Approval.ID, User, Common.TrainingPlan, "TrainingPlanID:" + data.QuanlityPlanID.ToString());

                }
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePlan").Reload();
            }
            return this.FormPanel(success);
        }
        #endregion

        #region Chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id)
        {
            var data = trainingPlanSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Xóa
        /// <summary>
        /// Xóa kế hoạch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeletePlan(int id)
        {
            try
            {
                trainingPlanSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePlan").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Gửi duyệt
        public ActionResult SendApproveForm(int id)
        {
            var data = trainingPlanSV.GetById(id);
            ViewData["TrainingPlanID"] = data.ID;
            data.ID = data.QuanlityPlanID;
            QualityPlanItem dataSelect = data;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "SendApprove", Model = dataSelect, ViewData = ViewData };
        }
        /// <summary>
        /// Xử lý thông tin gửi duyệt
        /// </summary>
        /// <param name="updateData"></param>
        /// <param name="exit"></param>
        /// <returns></returns>
        public ActionResult SendApprove(QualityPlanItem updateData, bool IsEdit = false)
        {
            bool success = false;
            try
            {
                planSV.SendApproval(updateData, User.ID);
                success = true;
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Có một kế hoạch đào tạo cần phê duyệt", updateData.CreateByName, updateData.Approval.ID, User, Common.TrainingPlan, "TrainingPlanID:" + updateData.ID.ToString());

                Ultility.CreateNotification(message: RequestMessage.SendSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePlan").Reload();
            }
            return this.FormPanel(success);
        }
        #endregion

        #region Phê duyệt
        public ActionResult ApproveForm(int id)
        {
            var data = trainingPlanSV.GetById(id);
            ViewData["TrainingPlanID"] = data.ID;
            data.ID = data.QuanlityPlanID;
            QualityPlanItem dataSelect = data;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = dataSelect, ViewData = ViewData };
        }
        
        /// <summary>
        /// Thực hiện phê duyệt kế hoạch 
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public ActionResult Appproved(QualityPlanItem updateData)
        {
            bool success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    planSV.Approve(updateData);
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Kế hoạch đào tạo của bạn đã được phê duyệt", updateData.CreateByName, updateData.CreateUserID, User, Common.TrainingPlan, "TrainingPlanID:" + updateData.ID.ToString());

                    Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
                    success = true;
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePlan").Reload();

            }
            return this.FormPanel(success);
        }
        #endregion

        #region Thiết lập yêu cầu đào tạo
        public ActionResult RequirementForm(int id)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Requirement", Model = new HumanTrainingPlanItem() { ID = id } };
        }
        public ActionResult LoadRequirementByPlanId(StoreRequestParameters parameters, int id)
        {
            int totalCount;
            var Data = trainingPlanSV.GetRequireByPlan(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(Data, totalCount);
        }
        public ActionResult LoadRequirementSelectByPlanId(StoreRequestParameters parameters, int id)
        {
            int totalCount;
            var Data = trainingPlanSV.GetRequireSelectByPlan(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(Data, totalCount);
        }
        public ActionResult UpdateRequirementPlan(string data, int planId)
        {
            var PlanRequirementdata = Ext.Net.JSON.Deserialize<HumanTrainingRequirementSelectItem>(data);
            try
            {
                if (PlanRequirementdata.ID != 0)
                {
                    if (PlanRequirementdata.IsSelected)
                    {
                        var PlanRequiredItem = new HumanTrainingPlanRequirementItem();
                        PlanRequiredItem.RequirementID = PlanRequirementdata.ID;
                        PlanRequiredItem.PlanID = planId;
                        trainingPlanSV.InsertRequirement(PlanRequiredItem, User.ID);
                    }
                    else
                        trainingPlanSV.DeleteRequire(PlanRequirementdata.ID);
                }
                else
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRequirement").Reload();
            }
            return this.Direct();
        }
        #endregion

    }
}

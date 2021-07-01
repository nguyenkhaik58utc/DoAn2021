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
using System.IO;
using iDAS.Items;
using iDAS.Utilities;
using iDAS.Web.Controllers;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Quản lý kế hoạch", Icon = "ChartBarEdit", IsActive = true, IsShow = true, Position = 2, Parent = GroupMenuPlanController.Code)]
    public class PlanController : BaseController
    { 
        private QualityPlanSV planSV = new QualityPlanSV();
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadPlanPartialView()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "PlanSelectWindow" };
        }
        public ActionResult LoadTargetPartialView()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "TargetSelectWindow" };
        }
        public ActionResult LoadPlan(StoreRequestParameters parameters, int departmentID = 0)
        {
            int totalCount;
            if (departmentID == 0 || departmentID == 1)
            {
                return this.Store(planSV.GetAll(parameters.Page, parameters.Limit, out totalCount), totalCount);
            }
            else
            {
                return this.Store(planSV.GetByDepartmentID(parameters.Page, parameters.Limit, out totalCount, departmentID), totalCount);
            }
        }
        public ActionResult LoadTargetStore()
        {
            var data = planSV.GetAllTarget();
            return this.Store(data);
        }
        public ActionResult LoadParentPlanStore()
        {
            var data = planSV.GetAll();
            return this.Store(data);
        }
        #region Thêm
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới")]
        public ActionResult UpdateForm(int id = 0)
        {
            var data = new QualityPlanItem();
            ViewData["EmployeeId"] = User.EmployeeID;
            if (id == 0)
            {
                data.ActionForm = Form.Add;
            }
            else
            {
                data = planSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data, ViewData = ViewData };
        }
        /// <summary>
        /// Thêm mới kế hoạch 
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public ActionResult Update(QualityPlanItem data, QualityMeetingItem b)
        {
            bool success = false;
            try
            {
                if (data.ID == 0)
                {
                    if (data.ApprovalBy == User.EmployeeID && data.IsApproval)
                    {
                        data.IsAccept = true;
                    }
                    // Lưu thông tin kế hoạch 
                    planSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: data.IsApproval ? RequestMessage.SendSuccess : RequestMessage.CreateSuccess);
                    success = true;
                }
                else
                {
                    // Lưu thông tin kế hoạch 
                    planSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    success = true;
                }

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult DetailForm(int ID)
        {
            var data = planSV.GetById(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion
        #region Xóa
        /// <summary>
        /// Xóa kế hoạch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult DeletePlan(int id)
        {
            try
            {

                planSV.Delete(id);
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
        #region Phê duyệt
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ApproveForm(int ID = 0)
        {
            var data = new QualityPlanItem();
            if (ID != 0)
            {
                data = planSV.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data };

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
                notify.Notify(this.ModuleCode, "Có một kế hoạch cần phê duyệt", updateData.Name, updateData.Approval.ID, User, Common.TrainingPlan, "TrainingPlanID:" + updateData.ID.ToString());
                    
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
                    notify.NotifyUser(this.ModuleCode, "Kế hoạch của bạn đã được phê duyệt", updateData.Name, updateData.CreateBy.Value, User, Common.TrainingPlan, "TrainingPlanID:" + updateData.ID.ToString());
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
        #region PartialView Plan

        public ActionResult PlanWindowView(string urlPlanStore = "", string urlTargetStore = "", string urlSubmit = "", int paramID = 0)
        {
            ViewData["UrlPlanStore"] = urlPlanStore;
            ViewData["UrlTargetStore"] = urlTargetStore;
            ViewData["UrlSubmit"] = urlSubmit;
            ViewData["ParamID"] = paramID;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
            };
        }
        public ActionResult PlanListView(string urlPlanStore = "", string urlTargetStore = "", string urlSubmit = "", int paramID = 0)
        {
            ViewData["UrlPlanStore"] = urlPlanStore;
            ViewData["UrlTargetStore"] = urlTargetStore;
            ViewData["UrlSubmit"] = urlSubmit;
            ViewData["ParamID"] = paramID;
            return PartialView(ViewData = ViewData);
        }
        public ActionResult PlanUpdateView(string urlPlanStore = "", string urlTargetStore = "", string urlSubmit = "", int paramID = 0, int ID = 0)
        {
            ViewData["UrlPlanStore"] = urlPlanStore;
            ViewData["UrlTargetStore"] = urlTargetStore;
            ViewData["UrlSubmit"] = urlSubmit;
            ViewData["ParamID"] = paramID;
            var data = new QualityPlanItem();
            if (ID == 0)
            {
                data.ActionForm = Form.Add;
            }
            else
            {
                data = planSV.GetById(ID);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult()
            {
                ViewName = "PlanUpdateView",
                Model = data,
                ViewData = ViewData
            };
        }
        /// <summary>
        /// Chi tiết danh sách kế hoạch
        /// </summary>
        /// <param name="urlPlanStore"></param>
        /// <param name="urlTargetStore"></param>
        /// <param name="paramID"></param>
        /// <returns></returns>
        public ActionResult PlanIndexView(string urlPlanStore = "", string urlTargetStore = "", string urlSubmit = "", int paramID = 0)
        {

            ViewData["UrlPlanStore"] = urlPlanStore;
            ViewData["UrlTargetStore"] = urlTargetStore;
            ViewData["UrlSubmit"] = urlSubmit;
            ViewData["ParamID"] = paramID;
            return PartialView(ViewData = ViewData);
        }
        #endregion
        #region Kết thúc kế hoạch
        [BaseSystem(Name = "Kết thúc", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult PlanEnd(int id)
        {
            var data = planSV.GetPlanEnd(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "PlanEnd", Model = data };
        }
        [HttpPost]
        public ActionResult PlanEnd(QualityPlanItem Plan)
        {
            try
            {
                planSV.End(Plan, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw ex;
            }
            return this.Direct();
        }
        #endregion
        #region Theo dõi kế hoạch
        public ActionResult Follow(int id)
        {
            var data = new QualityPlanItem() { ID = id };
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Follow", Model = data };
        }
        public ActionResult LoadPlanFollow(string node, int planId = 0)
        {
            NodeCollection nodes = new NodeCollection();
            bool isRoot = node == "root" ? true : false;
            var parentId = isRoot ? planId : System.Convert.ToInt32(node);
            if (!isRoot || planId != 0)
            {
                var Plans = planSV.GetChildPlan(parentId, isRoot);
                foreach (var Plan in Plans)
                {
                    nodes.Add(creatPlan(Plan));
                }
            }
            return this.Content(nodes.ToJson());
        }
        private Node creatPlan(QualityPlanItem dataNode)
        {
            Node nodeItem = new Node();
            nodeItem.NodeID = dataNode.ID.ToString();
            nodeItem.Text = !dataNode.Leaf ? dataNode.Name.ToUpper() : dataNode.Name;
            nodeItem.Icon = Icon.TagBlue;
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Status", Value = dataNode.Status.ToString(), Mode = ParameterMode.Value });
            nodeItem.Leaf = dataNode.Leaf;
            return nodeItem;
        }
        #endregion
    }
}

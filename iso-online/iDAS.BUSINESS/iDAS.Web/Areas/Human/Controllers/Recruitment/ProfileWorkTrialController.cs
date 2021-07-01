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
    [BaseSystem(Name = "Danh sách nhân sự thử việc", Icon = "TagGreen", IsActive = false, IsShow = true, Position = 1, Parent = GroupRecruitmentTrialController.Code)]
    public class ProfileWorkTrialController : BaseController
    {
        //
        // GET: /Human/ProfileWorkTrial/
        HumanProfileWorkTrialSV ptwSV = new HumanProfileWorkTrialSV();
        HumanProfileWorkTrialResultSV rsSV = new HumanProfileWorkTrialResultSV();
        TaskCommentSV cmSV = new TaskCommentSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Employee(int Id)
        {
            ViewBag.EmpID = Id;
            return View();
        }
        public ActionResult GetDataEmployee(StoreRequestParameters parameters, int empID)
        {
            
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ptwSV.GetByEmployee(filter, empID,0);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult LoadList(StoreRequestParameters parameters, string fromDate, string toDate)
        {
             DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = System.Convert.ToDateTime(fromDate);
            }
            else
            {
                searchFromDate = DateTime.Now;
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = System.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            else
            {
                searchToDate = DateTime.Now;
            }
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ptwSV.GetAll(filter,searchFromDate,searchToDate);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult GetQualityCriteria(StoreRequestParameters parameters, string categoryID)
        {
            List<QualityCriteriaItem> lstData = new List<QualityCriteriaItem>();
            int total;
            if (!string.IsNullOrEmpty(categoryID))
            {
                lstData = new QualityCriteriaSV().GetCriteriaUsedByCateIds(parameters.Page, parameters.Limit, out total, categoryID);
            }
            
            return this.Store(lstData,lstData.Count);

        }
        public ActionResult TaskForm(int id)
        {
            var data = new HumanProfileWorkTrialItem();
            data = ptwSV.getByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "TaskEmployee", Model = data };
        }
        public ActionResult ListComentForm(int id)
        {
            var data = new TaskItem();
            data.ID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "TaskComent", Model = data };
        }
        public ActionResult LoadListComent(StoreRequestParameters parameters, int ID)
        {            
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = cmSV.GetAll(filter, ID,User.ID);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult UpdateCommentForm(int ID, string action, int TaskId)
        {
            var data = new TaskCommentItem();
            if (ID != 0)
            {
                data = new TaskCommentSV().GetById(ID);
            }
            else
            {
                data.TaskID = TaskId;
                data.EmployeeID = User.EmployeeID;
                data.Employee.ID = User.EmployeeID;
            };
            data.ActionForm = action;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateTaskComent", Model = data };
        }
        public ActionResult UpdateComment(TaskCommentItem updateData)
        {
            try
            {
                if (updateData.ID != 0)
                {
                    cmSV.Update(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    cmSV.Insert(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                X.GetCmp<Store>("PlanInterviewStore").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winInterviewUpdate").Close();
            }
            return this.Direct();
        }
        public ActionResult DeleteComent(int id)
        {
            try
            {
                
                cmSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("PlanInterviewStore").Reload();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = true)]
        [SystemAuthorize(AllowAnonymous = false, CheckAuthorize = true)]
        public ActionResult DeleteProfile(int id)
        {
            try
            {
                ptwSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreProfile").Reload();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Cập nhật", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int ID, string action)
        {
            var data = new HumanProfileWorkTrialItem();
            if (ID != 0)
            {
                data = ptwSV.getByID(ID);
            }           
            data.ActionForm = action;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(HumanProfileWorkTrialItem updateData)
        {
            try
            {
                if (updateData.ID != 0)
                {
                    if (ptwSV.CheckEmpIDandRoleExits(updateData.Employee.ID, updateData.Role.ID))
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Cán bộ và vị trí đã tồn tại!!"
                        });
                    }
                    else
                    {
                        ptwSV.Update(updateData, User.ID);
                        Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    }
                }
                else
                {
                    if (ptwSV.CheckEmpIDandRoleExits(updateData.Employee.ID, updateData.Role.ID))
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Cán bộ và vị trí đã tồn tại!!"
                        });
                    }
                    else
                    {
                        ptwSV.Insert(updateData, User.EmployeeID);
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    }
                }
                X.GetCmp<Store>("StoreProfile").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winNewPlan").Close();
            }
            return this.Direct();
        }
        public ActionResult SendTickForm(int id)
        {
            var data = new HumanProfileWorkTrialItem();
            data.ID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "SelectCriteria", Model = data };
        }
        public ActionResult SaveCriteria(string catID, int humanProfileWorkTrialID)
        {
            try
            {
                rsSV.InsertListCri(catID, humanProfileWorkTrialID, User.ID);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                NotifyController notify = new NotifyController();
                var data = new HumanProfileWorkTrialItem();
                if (humanProfileWorkTrialID != 0)
                {
                    data = ptwSV.getByID(humanProfileWorkTrialID);
                }     
                notify.Notify(this.ModuleCode, "Bạn có một phiếu đánh giá cá nhân cần thực hiện", User.Name, data.HumanEmployeeID, User, Common.ProfileWorkTrialEmployee, "HumanProfileWorkTrialID:" + humanProfileWorkTrialID.ToString());
                notify.Notify(this.ModuleCode, "Bạn có một phiếu đánh giá nhân sự cần thực hiện", User.Name, data.ManagerID.Value, User, Common.ProfileWorkTrialManager, "HumanProfileWorkTrialID:" + humanProfileWorkTrialID.ToString());
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess, error: true);
            }
            finally
            {
                //X.GetCmp<Store>("StorePlanIndex").Reload();
                X.GetCmp<Window>("frmUpdate").Close();
            }
            return this.Direct();
        }
    }
}

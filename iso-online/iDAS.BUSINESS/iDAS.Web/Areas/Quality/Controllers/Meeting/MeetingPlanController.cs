using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Quality.Controllers
{

    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Kế hoạch", Icon = "ServerEdit", IsActive = true, IsShow = true, Position = 1, Parent = MeetingController.Code)]
    public class MeetingPlanController : BaseController
    {
        private QualityMeetingSV meetingSV = new QualityMeetingSV();
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult LoadMeetingPlan(StoreRequestParameters parameters, int departmentID = 0, int focusId = 0)
        {
            int totalCount;
            return this.Store(meetingSV.GetNotAppovedByDepartmentID(parameters.Page, parameters.Limit, out totalCount, departmentID, focusId), totalCount);
        }

        #region Thêm
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới và sửa")]
        public ActionResult UpdateForm(int id = 0)
        {
            var data = new QualityMeetingItem();
            if (id == 0)
            {
                data.ActionForm = Form.Add;
            }
            else
            {
                data = meetingSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public ActionResult Update(QualityMeetingItem data)
        {
            try
            {
                if (data.ID == 0)
                {
                    meetingSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    meetingSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }

            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            finally
            {

                X.GetCmp<Window>("winMeetingUpdate").Close();
                X.GetCmp<Store>("StoreMeetingPlan").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Chi tiết
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult DetailForm(int ID)
        {
            var data = meetingSV.GetById(ID);
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
        public ActionResult Delete(int id)
        {
            try
            {
                meetingSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreMeetingPlan").Reload();
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
            var data = new QualityMeetingItem();
            if (ID != 0)
            {
                data = meetingSV.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data };

        }
        /// <summary>
        /// Xử lý thông tin gửi duyệt
        /// </summary>
        /// <param name="updateData"></param>
        /// <param name="exit"></param>
        /// <returns></returns>
        public ActionResult SendApprove(QualityMeetingItem updateData)
        {
            try
            {
                meetingSV.SendApproval(updateData, User.ID);
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Yêu cầu phê duyệt kế hoạch cuộc họp", updateData.Name, updateData.Approval.ID, User, Common.MeetingPlan, "MeetingID:" + updateData.ID.ToString());
                Ultility.CreateNotification(message: RequestMessage.SendSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreMeetingPlan").Reload();
            }
            return this.Direct();
        }
        /// <summary>
        /// Thực hiện phê duyệt
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public ActionResult Appproved(QualityMeetingItem updateData)
        {
            try
            {
                if (updateData.ID != 0)
                {
                    updateData.IsAccept = updateData.IsResult == true;
                    meetingSV.Approve(updateData);
                    NotifyController notify = new NotifyController();
                    var data = meetingSV.GetById(updateData.ID);
                    List<int> notifyObj = new List<int>() { (int)data.CreateBy };
                    if (data.UpdateBy != null && data.UpdateBy != data.CreateBy)
                    {
                        notifyObj.Add((int)data.UpdateBy);
                    }
                    notify.NotifyUser(this.ModuleCode, "Có một kế hoạch cuộc họp đã được phê duyệt", updateData.Name, notifyObj, User, Common.MeetingPlan, "MeetingID:" + updateData.ID.ToString());
                    Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreMeetingPlan").Reload();

            }
            return this.Direct();
        }
        #endregion

        #region Thành phần tham dự

        public ActionResult ObjectForm(int ID)
        {
            var data = meetingSV.GetById(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Object", Model = data };
        }

        public ActionResult LoadMeetingPlanObject(StoreRequestParameters parameters, int meetingID = 0)
        {
            int totalCount;
            var result = meetingSV.GetMettingObject(parameters.Page, parameters.Limit, out totalCount, meetingID);
            foreach (QualityMeetingObjectItem emp in result)
            {
                string strChucDanh = "";
                foreach (HumanGroupPositionItem hgpI in emp.lstHumanGrPos)
                {
                    strChucDanh += hgpI.Name + " -- " + hgpI.GroupName + "</br>";
                }
                emp.RoleName = strChucDanh;
            }
            return this.Store(result, totalCount);
        }
        public ActionResult LoadMeetingPlanObjectHasMeeting(StoreRequestParameters parameters, int meetingID = 0)
        {
            int totalCount;
            var result = meetingSV.GetMettingObjectHasMeeting(parameters.Page, parameters.Limit, out totalCount, meetingID);
            foreach (QualityMeetingObjectItem emp in result)
            {
                string strChucDanh = "";
                foreach (HumanGroupPositionItem hgpI in emp.lstHumanGrPos)
                {
                    strChucDanh += hgpI.Name + " -- " + hgpI.GroupName + "</br>";
                }
                emp.RoleName = strChucDanh;
            }
            return this.Store(result, totalCount);
        }
        public ActionResult UpdateMeetingPlanObject(string stringId, int meetingId)
        {
            try
            {
                meetingSV.InsertMeetingObject(stringId, User.ID, meetingId);
                X.GetCmp<Store>("storeMeetingObject").Reload();
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();
        }
        public ActionResult DeleteObject(String listId)
        {
            try
            {
                meetingSV.DeleteRangeObject(listId);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("storeMeetingObject").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region
        public ActionResult ProblemForm(int ID)
        {
            var data = new QualityMeetingProblemItem();
            data.MeetingID = ID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Problem", Model = data };
        }
        #endregion
    }
}

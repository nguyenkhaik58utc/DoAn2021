using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Thực hiện", Icon = "Cog", IsActive = true, IsShow = true, Position = 2, Parent = MeetingController.Code)]
    public class MeetingPerformController : BaseController
    {
        private QualityMeetingSV MeetingService = new QualityMeetingSV();
        private QualityMeetingObjectSV meetingObjectService = new QualityMeetingObjectSV();
        private QualityMeetingResultSV MeetingResultService = new QualityMeetingResultSV();
        private QualityMeetingProblemSV MeetingProblemService = new QualityMeetingProblemSV();
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadMeetingPerfrom(StoreRequestParameters parameters, int departmentID = 0)
        {
            int totalCount;
            if (departmentID == 0 || departmentID == 1)
            {
                return this.Store(MeetingService.GetAllApproved(parameters.Page, parameters.Limit, out totalCount), totalCount);
            }
            else
            {
                return this.Store(MeetingService.GetAppovedByDepartmentID(parameters.Page, parameters.Limit, out totalCount, departmentID), totalCount);
            }

        }
        #region Đối tượng tham gia

        public ActionResult ObjectForm(int ID)
        {
            var data = MeetingService.GetById(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Object", Model = data };
        }

        public ActionResult LoadMeetingPlanObject(StoreRequestParameters parameters, int meetingID = 0)
        {
            int totalCount;
            var result = meetingObjectService.GetByMeeting(parameters.Page, parameters.Limit, out totalCount, meetingID);
            return this.Store(result, totalCount);
        }
        [ValidateInput(false)]        
        public ActionResult UpdateMeetingPerformObject(string data)
        {
            try
            {
                var objectItem = Ext.Net.JSON.Deserialize<QualityMeetingObjectItem>(data);
                meetingObjectService.Update(objectItem, User.ID);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();
        }
        #endregion

        #region Biên bản cuộc họp
        public ActionResult ResultForm(int ID)
        {
            var data = new QualityMeetingResultItem();
            var result = MeetingResultService.GetByMeetingId(ID);
            if (result != null)
            {
                data = result;
            }
            data.MeetingID = ID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Result", Model = data };
        }
        public ActionResult UpdateResult(QualityMeetingResultItem data)
        {
            try
            {
                if (data.ID == 0)
                {
                    // Lưu thông tin
                    MeetingResultService.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    // Lưu thông tin
                    MeetingResultService.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }

            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            finally
            {
                X.GetCmp<Store>("StoreMeetingPerform").Reload();
                X.GetCmp<Window>("winMeetingResult").Close();
            }
            return this.Direct();
        }
        public ActionResult UseMeetingInfo(int ID = 0, int meetingID = 0)
        {
            var data = new QualityMeetingResultItem();
            if (meetingID != 0)
            {
                data = MeetingResultService.UseMeetingInfo(meetingID);
            }
            if (ID != 0)
            {
                data.ID = ID;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Result", Model = data }; ;
        }
        #endregion

        #region Vấn đề cuộc họp
        public ActionResult ProblemForm(int ID)
        {
            var data = MeetingProblemService.GetById(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateContentProblem", Model = data };
        }

        public ActionResult UpdateProblem(QualityMeetingProblemItem data)
        {
            try
            {
                MeetingProblemService.UpdateContent(data, User.ID);
                X.GetCmp<Store>("storeMeetingProblem").GetById(data.ID).Commit();

            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            return this.Direct();
        }
        #endregion

        #region Kết thúc
        public ActionResult Finish(int ID)
        {
            try
            {
                if (ID != 0)
                {
                    MeetingService.Finish(ID, User.ID);
                    X.GetCmp<Store>("StoreMeetingPerform").Reload();
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();
        }
        #endregion
    }
}
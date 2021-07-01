using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
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
    [BaseSystem(Name = "Vấn đề cuộc họp")]
    public class MeetingProblemController : BaseController
    {
        private QualityMeetingProblemSV meetingProblemSV = new QualityMeetingProblemSV();
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult LoadIsoProblemPartialView()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "IsoProblemSelectWindow" };
        }
        public ActionResult LoadIso()
        {
            var result = meetingProblemSV.LoadISO();
            return this.Store(result);
        }
        public ActionResult LoadIsoProblem(StoreRequestParameters parameters, int isoID = 0)
        {
            int totalCount;
            var result = new List<CenterISOMeetingItem>();
            result = meetingProblemSV.GetIsoProblem(parameters.Page, parameters.Limit, out totalCount, isoID);
            return this.Store(result, totalCount);
        }
        public ActionResult LoadMeetingProblem(StoreRequestParameters parameters, int meetingID = 0)
        {
            int totalCount;
            var result = meetingProblemSV.GetByMeeting(parameters.Page, parameters.Limit, out totalCount, meetingID);
            return this.Store(result, totalCount);
        }
        public ActionResult UpdateMeetingProblem(string stringId, int meetingId)
        {
            try
            {
                meetingProblemSV.InsertObject(stringId, User.ID, meetingId);
                X.GetCmp<Store>("storeMeetingProblem").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();
        }

        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult DeleteProblem(String listId)
        {
            try
            {

                meetingProblemSV.DeleteRange(listId);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("storeMeetingProblem").Reload();
            }
            return this.Direct();
        }

        public ActionResult Update(string data)
        {
            try
            {
                var updateData = Ext.Net.JSON.Deserialize<QualityMeetingProblemItem>(data);
                meetingProblemSV.UpdateContent(updateData, User.ID);
                X.GetCmp<Store>("storeMeetingProblem").GetById(updateData.ID).Commit();

            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            return this.Direct();
        }
    }
}

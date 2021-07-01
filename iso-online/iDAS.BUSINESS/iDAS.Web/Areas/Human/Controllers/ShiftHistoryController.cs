using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Web.Areas.Human.Models;
using System;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Ca trực", IsActive = true, Icon = "GroupLink", IsShow = true, Position = 1)]
    public class ShiftHistoryController : BaseController
    {

        private ShiftHistoryAPI api = new ShiftHistoryAPI();

        public ActionResult Insert(DateTime? shiftTime, bool val)
        {
            if (shiftTime == null)
                shiftTime = DateTime.Now;

            try
            {
                ShiftHistoryDTO obj = new ShiftHistoryDTO();
                obj.ShiftTime = shiftTime.Value;
                obj.UserID = User.ID;

                if (ModelState.IsValid)
                {
                    api.Insert(obj, User.ID);
                    if (val)
                    {
                        X.GetCmp<Window>("winNewTitle").Close();
                    }
                    X.GetCmp<Store>("stTitleNew").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();
        }

        public ActionResult Update(ShiftHistoryDTO objEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objEdit.UserID = User.ID;
                    api.Update(objEdit);
                    X.GetCmp<Store>("stTitleNew").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.Direct();
        }

        public ActionResult HandOverShift(ShiftHistoryDTO objEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objEdit.UserID = User.ID;
                    api.HandOverShift(objEdit);
                    X.GetCmp<Store>("stTitleNew").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.Direct();
        }
    }
}

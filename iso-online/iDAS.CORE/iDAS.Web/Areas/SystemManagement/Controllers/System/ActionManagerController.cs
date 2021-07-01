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

namespace iDAS.Web.Areas.SystemManagement.Controllers
{
    public partial class SystemManagerController : BaseController
    {
        private SystemActionService actionService = new SystemActionService();
        private readonly string storeActionsID = "StoreActions";

        public ActionResult LoadActions(StoreRequestParameters parameters, string moduleCode)
        {
            int totalCount;
            var actions = actionService.GetAll(parameters.Page, parameters.Limit, out totalCount, moduleCode);
            Ultility.SetIconUrl(actions);
            Ultility.SetStatus(actions);
            return this.Store(actions, totalCount);
        }

        [ValidateInput(false)]
        public ActionResult UpdateAction(string data)
        {
            var action = Ext.Net.JSON.Deserialize<SystemActionItem>(data);
            try
            {
                Ultility.SetIconName(action);
                actionService.Update(action, User.ID);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeActionsID).Reload();
            }
            return this.Direct();
        }


        public ActionResult DeleteAction(int id)
        {
            try
            {
                actionService.Delete(id);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeActionsID).Reload();
            }
            return this.Direct();
        }
    }
}

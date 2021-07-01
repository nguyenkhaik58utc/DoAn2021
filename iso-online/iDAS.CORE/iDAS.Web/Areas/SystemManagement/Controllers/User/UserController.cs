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
    public partial class UserManagerController : BaseController
    {
        private SystemUserService userService = new SystemUserService();
        private readonly string storeUsersID = "StoreUsers";

        public ActionResult LoadUsers(StoreRequestParameters parameters)
        {
            int totalCount;
            return this.Store(userService.GetAll(parameters.Page, parameters.Limit, out totalCount), totalCount);
        }
        public ActionResult InsertUser(SystemUserItem user)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    userService.Insert(user, User.ID);
                    Ultility.CreateNotification(message: Message.InsertSuccess);
                    X.GetCmp<Store>(storeUsersID).Reload();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.InsertError, error: true);
            }
            return this.FormPanel(true);
        }
        public ActionResult UpdateUser(string data)
        {
            var user = Ext.Net.JSON.Deserialize<SystemUserItem>(data);
            try
            {
                userService.Update(user, User.ID);
                Ultility.CreateNotification(message:Message.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error:true);
            }
            finally
            {
                X.GetCmp<Store>(storeUsersID).Reload();
            }
            return this.Direct();
        }
        public ActionResult DeleteUser(int id)
        {
            try
            {
                userService.Delete(id);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.DeleteError, error:true).Show();
            }
            finally
            {
                X.GetCmp<Store>(storeUsersID).Reload();
            }
            return this.Direct();
        }
    }
}

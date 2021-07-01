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
        private SystemPermissionService permissionService = new SystemPermissionService();
        private SystemModuleService moduleService = new SystemModuleService();
        private SystemActionService actionService = new SystemActionService();
        private readonly string storePermissionsID = "StorePermissions";

        public ActionResult LoadPermissions(StoreRequestParameters parameters, int? roleId=null, string moduleId=null)
        {
            if (roleId == null || moduleId==null) return null;
            int totalCount;
            var actions = actionService.GetAll(parameters.Page, parameters.Limit, out totalCount, moduleId);
            foreach (var action in actions) {
                var permission = permissionService.GetPermission(roleId.Value,action.ID);
                action.IsEnable = permission != null;
            }

            return this.Store(actions, totalCount);
        }
        public ActionResult UpdatePermission(string data, int roleId)
        {
            var action = Ext.Net.JSON.Deserialize<SystemActionItem>(data);
            try
            {
                var permission = permissionService.GetPermission(roleId, action.ID);
                if (permission == null && action.IsEnable)
                {
                    permission = new SystemPermissionItem();
                    permission.ActionID = action.ID;
                    permission.RoleID = roleId;
                    permissionService.Insert(permission, User.ID);
                }
                if (permission != null && !action.IsEnable)
                {
                    permissionService.Delete(permission.ID);
                }
                Ultility.CreateNotification(message: Message.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storePermissionsID).Reload();
            }
            return this.Direct();
        }
        public ActionResult UpdatePermissions(string data, int roleId, bool isEnable)
        {
            try
            {
                var actionIds = Ext.Net.JSON.Deserialize<List<int>>(data);
                foreach (var actionId in actionIds)
                {
                    var permission = permissionService.GetPermission(roleId, actionId);
                    if (permission == null && isEnable)
                    {
                        permission = new SystemPermissionItem();
                        permission.ActionID = actionId;
                        permission.RoleID = roleId;
                        permissionService.Insert(permission, User.ID);
                    }
                    if (permission != null && !isEnable)
                    {
                        permissionService.Delete(permission.ID);
                    }
                }
                Ultility.ShowMessageBox(message: Message.UpdateSuccess, icon: MessageBox.Icon.INFO);
            }
            catch (Exception ex)
            {
                Ultility.ShowMessageBox(message: Message.UpdateError, icon: MessageBox.Icon.INFO);
            }
            finally
            {
                X.GetCmp<Store>(storePermissionsID).Reload();
            }

            return this.Direct();
        }
        public ActionResult LoadModules()
        {
            var modules = moduleService.GetAll();
            return this.Store(modules);
        }
    }
}

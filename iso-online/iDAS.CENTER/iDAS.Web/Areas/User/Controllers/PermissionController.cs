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

namespace iDAS.Web.Areas.User.Controllers
{
    [BaseSystem(Name = "Quản lý phân quyền", IsActive = true, IsShow = true, Position = 3, Icon = "GroupLink")]
    public partial class PermissionController : BaseController
    {
        private PermissionSV permissionService = new PermissionSV();
        private ModuleSV moduleService = new ModuleSV();
        private ActionSV actionService = new ActionSV();
        private readonly string storePermissionsID = "StorePermissions";
        //[BaseSystem(Name = "Xem Danh Sách")]
        //[SystemAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadPermissions(StoreRequestParameters parameters, int? roleId = null, string moduleCode = null, string funtionCode = null)
        {
            if (roleId == null || moduleCode == null || funtionCode == null) return null;
            int totalCount;
            var actions = actionService.GetAll(parameters.Page, parameters.Limit, out totalCount, moduleCode, funtionCode);
            foreach (var action in actions)
            {
                var permission = permissionService.GetPermission(roleId.Value, action.ID);
                action.IsEnable = permission != null;
            }
            return this.Store(actions, totalCount);
        }
        //[BaseSystem(Name = "Xem Chi Tiết")]
        //[SystemAuthorize]
        public ActionResult ShowFrmPermission(int roleId, string roleName)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "FrmPermission", ViewData = new ViewDataDictionary { { "roleId", roleId }, { "roleName", roleName } } };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        //[BaseSystem(Name = "Cập Nhật")]
        //[SystemAuthorize]
        public ActionResult UpdatePermission(string data, int roleId)
        {
            var action = Ext.Net.JSON.Deserialize<iDAS.Items.ActionItem>(data);
            try
            {
                var permission = permissionService.GetPermission(roleId, action.ID);
                if (permission == null)
                {
                    permission = new PermissionItem();
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
        //[BaseSystem(Name = "Cập Nhật Tất Cả")]
        //[SystemAuthorize]
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
                        permission = new PermissionItem();
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
            //var modules = moduleService.GetAll();
            //return this.Store(modules);
            return View();
        }
    }
}

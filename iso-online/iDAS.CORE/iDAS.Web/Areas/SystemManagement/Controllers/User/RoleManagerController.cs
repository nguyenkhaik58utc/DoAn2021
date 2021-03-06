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
        private SystemRoleService roleService = new SystemRoleService();
        private readonly string storeRolesID = "StoreRoles";

        public ActionResult LoadRole(int? id = null)
        {
            if (id == null&& id !=0) return null; 
            var role = roleService.GetByID(id.Value);
            return this.Store(role);
        }
        public ActionResult LoadRoles(int? groupId=null)
        {
            if (groupId == null)
            { 
                return null; 
            }
            var roles = roleService.GetAllByGroupID(groupId.Value);
            return this.Store(roles);
        }
        public ActionResult InsertRole(SystemRoleItem role)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    roleService.Insert(role, User.ID);
                    Ultility.CreateNotification(message: Message.InsertSuccess);
                    X.GetCmp<Store>(storeRolesID).Reload();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.InsertError, error: true);
            }
            return this.FormPanel(true);
        }
        public ActionResult UpdateRole(string data)
        {
            var role = Ext.Net.JSON.Deserialize<SystemRoleItem>(data);
            try
            {
                roleService.Update(role, User.ID);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeRolesID).Reload();
            }
            return this.Direct();
        }
        public ActionResult DeleteRole(int id)
        {
            try
            {
                roleService.Delete(id);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.DeleteError,error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeRolesID).Reload();
            }
            return this.Direct();
        }
    }
}

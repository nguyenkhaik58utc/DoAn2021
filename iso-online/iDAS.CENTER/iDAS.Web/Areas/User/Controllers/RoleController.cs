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
    public partial class RoleController : BaseController
    {
        private RoleSV roleService = new RoleSV();
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
        public ActionResult InsertRole(RoleItem role)
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
        public ActionResult CoppyRole(string lstrole, int groupId)
        {
            try
            {
                var ids = lstrole.Split(',').Select(n => (object)int.Parse(n)).ToList();
                List<RoleItem> lsR = new List<RoleItem>();
                foreach (int itemrole in ids)
                {                    
                    lsR.Add(roleService.GetByID(itemrole));
                }
                foreach (var item in lsR)
                {
                    item.GroupID = groupId;
                    roleService.Insert(item, User.ID);
                }
                X.GetCmp<Window>("winCoppy").Close();
                Ultility.CreateNotification(message: Message.InsertSuccess);
                X.GetCmp<Store>(storeRolesID).Reload();
                
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.InsertError, error: true);
            }
            return this.FormPanel(true);
        }
        public ActionResult UpdateRole(string data)
        {
            var role = Ext.Net.JSON.Deserialize<RoleItem>(data);
            try
            {
                roleService.Update(role, User.ID);               
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
        [BaseSystem(Name = "Xóa thông tin quy mô công ty", IsActive = true, IsShow = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
                foreach (int item in ids)
                {
                     roleService.Delete(item);
                }
                X.GetCmp<Store>(storeRolesID).Reload();
                Ultility.CreateNotification(message: Message.DeleteSuccess);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.DeleteError,error: true);
                return this.Direct("Error");
            }
        }
    }
}

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
using iDAS.Utilities;
namespace iDAS.Web.Areas.User.Controllers
{
    [BaseSystem(Name = "Quản lý thành viên", IsActive = true, IsShow = true, Position = 2)]
    public class UserController : BaseController
    {
        private UserSV userService = new UserSV();
        private readonly string storeUsersID = "StoreUsers";
        [BaseSystem(Name = "Xem Danh Sách")]
        [SystemAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadUsers(StoreRequestParameters parameters)
        {
            int totalCount;
            return this.Store(userService.GetAll(parameters.Page, parameters.Limit, out totalCount), totalCount);
        }
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize]
        public ActionResult InsertUser(UserItem user)
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
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize]
        public ActionResult UpdateUser(string data)
        {
            var user = Ext.Net.JSON.Deserialize<UserItem>(data);
            try
            {
                userService.Update(user, User.ID);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeUsersID).Reload();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                userService.Delete(id);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.DeleteError, error: true).Show();
            }
            finally
            {
                X.GetCmp<Store>(storeUsersID).Reload();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Thiết lập chức danh")]
        [SystemAuthorize]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult SettingForm(int Id)
        {
            // X.GetCmp<Store>("StoreOrganizations").St;
            var data = new UserItem();
            data.ID = Id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Setting", Model = data };
        }
        [BaseSystem(Name = "Xem Chi Tiết")]
        [SystemAuthorize]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DetailForm(int Id)
        {
            var data = userService.GetById(Id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult UpdateForm(int Id)
        {
            var data = userService.GetById(Id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateData"></param>
        /// <param name="exit"></param>
        /// <returns></returns>
        public ActionResult Update(UserItem updateData, bool exit)
        {
            try
            {
                userService.Update(updateData, User.ID);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
                if (exit == true)
                {
                    X.GetCmp<Window>("winUpdate").Close();
                    X.GetCmp<Store>("StoreUsers").Reload();
                }
                return this.Direct();

            }
            catch
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
                return RedirectToAction("Index");
            }
        }
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize]
        public ActionResult AddForm()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Add" };
        }
    }
}

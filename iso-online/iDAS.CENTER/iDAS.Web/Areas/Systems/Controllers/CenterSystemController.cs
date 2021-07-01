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


namespace iDAS.Web.Areas.Systems.Controllers
{
    [BaseSystem(Name = "Quản lý hệ thống", IsActive = true, IsShow = true, Position = 1, Icon = "ApplicationTileVertical")]
    public class CenterSystemController : BaseController
    {
        SystemSV systemSV = new SystemSV();
        string storeName = "stCenterSystem";

        [BaseSystem(Name = "Danh sách Hệ thống", IsActive = true, IsShow = true)]
        public ActionResult Index()
        {
            return View();
        }

        //
        public ActionResult ShowFrm(int id = 0, int type = 0, int status = 0)
        {

            Ext.Net.MVC.PartialViewResult partialView = new Ext.Net.MVC.PartialViewResult();
            SystemItem obj;
            if (type == (int)Common.FormName.Insert)//form them moi
            {
                partialView = new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
            }
            else if (type == (int)Common.FormName.Edit)
            {
                obj = systemSV.GetByID(id);

                return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
            }
            else if (type == (int)Common.FormName.Detail)
            {
                obj = systemSV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            return partialView;

        }
        //

        public ActionResult LoadSystems(StoreRequestParameters parameters)
        {
            int totalCount;
            var data = systemSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(data, totalCount);
        }


        public ActionResult InsertSystem(SystemItem obj, bool exit)
        {

            if (insert(obj))
            {
                X.GetCmp<FormPanel>("frmNewCenterSystem").Reset();

                if (exit) X.GetCmp<Window>("winNewCenterSystem").Close();
                Ultility.CreateNotification(message: Message.InsertSuccess);
                X.GetCmp<Store>(storeName).Reload();
            }
            return this.Direct();
        }
        public ActionResult EditSystem(SystemItem obj, string DBSource, string DBUserID, string DBPassword,
            string MailHost, string MailUserName, string MailUserPassword)
        {
            try
            {
                obj.DBSource = Encryptor.Encode(DBSource);
                obj.DBUserID = Encryptor.Encode(DBUserID);
                obj.DBPassword = Encryptor.Encode(DBPassword);
                obj.MailHost =string.IsNullOrEmpty(MailHost)?"": Encryptor.Encode(MailHost);
                obj.MailUserName = string.IsNullOrEmpty(MailUserName) ? "" : Encryptor.Encode(MailUserName);
                obj.MailUserPassword = string.IsNullOrEmpty(MailUserPassword) ? "" : Encryptor.Encode(MailUserPassword);
                var fileUploadAvatarField = X.GetCmp<FileUploadField>("FileUploadAvatarFieldId");
                if(fileUploadAvatarField.HasFile)
                {
                    obj.ImageUser = fileUploadAvatarField.FileBytes;
                }
                var fileUploadLogoField = X.GetCmp<FileUploadField>("FileUploadLogoFieldId");
                if (fileUploadLogoField.HasFile)
                {
                    obj.ImageLogo = fileUploadLogoField.FileBytes;
                }
                
                obj.UpdateBy = User.ID;
                systemSV.Update(obj);
                X.GetCmp<FormPanel>("frmEditCenterSystem").Reset();
                X.GetCmp<Window>("winEditCenterSystem").Close();
                Ultility.CreateNotification(message: Message.UpdateSuccess);
                X.GetCmp<Store>(storeName).Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            return this.Direct();
        }

        public ActionResult UpdateSystem(string url)
        {
            try
            {
                var success = systemSV.UpdateToCenter(url);
                if (success)
                {
                    Ultility.CreateNotification(message: Message.UpdateSuccess);
                }
                else
                {
                    Ultility.CreateNotification(message: Message.UpdateError, error: true);
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            return this.Direct();
        }

        public ActionResult UpdateStatus(int ID, bool Active)
        {
            //systemSV.UpdateStatus(ID, Active, User.ID);
            Ultility.CreateNotification(message: Message.UpdateSuccess);
            X.GetCmp<Store>(storeName).Reload();
            return this.Direct();
        }


        public ActionResult Delete(int ID)
        {
            if (ID > 0)
            {
                //systemSV.Delete(ID);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
                X.GetCmp<Store>(storeName).Reload();
            }

            return this.Direct();

        }

        private bool insert(SystemItem obj)
        {
            try
            {
                obj.CreateBy = User.ID;
                //systemSV.Insert(obj);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

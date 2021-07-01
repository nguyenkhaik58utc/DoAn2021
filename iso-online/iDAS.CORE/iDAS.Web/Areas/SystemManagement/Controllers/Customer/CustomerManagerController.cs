using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
namespace iDAS.Web.Areas.SystemManagement.Controllers
{
    [SystemAuthorize()]
    [BaseSystem(Name = "Quản trị khách hàng", IsActive = true, IsShow = true)]
    public partial class CustomerManagerController : BaseController
    {
        SystemCustomerService service = new SystemCustomerService();

        [BaseSystem(Name = "Thông tin khách hàng", IsActive = true, IsShow = true)]
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult LoadCustomers(StoreRequestParameters parameters)
        {
            int totalCount;
            var customers = service.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(customers, totalCount);
        }
        public ActionResult InsertCustomer(SystemCustomerItem customer)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var fileUploadField = X.GetCmp<FileUploadField>("FileUploadField2");
                    string path = PathUpload.Logo;
                    upload(ref path, fileUploadField, FormatFile.Image, customer.Code);
                    if (path != PathUpload.Logo)
                    {
                        customer.Logo = path;
                        service.Insert(customer, User.ID);
                        X.GetCmp<Store>("StoreCustomers").Reload();
                        X.GetCmp<Window>("WindowAdd").Close();
                        Ultility.CreateNotification(message: Message.InsertSuccess);
                    }
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.InsertError, error: true);
            }
            return this.FormPanel(true);
        }
        public ActionResult UpdateCustomer(string data)
        {
            var customer = Ext.Net.JSON.Deserialize<SystemCustomerItem>(data);
            try
            {
                service.Update(customer, User.ID);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomers").Reload();
            }
            return this.Direct();
        }
        public ActionResult DeleteCustomer(int id)
        {
            try
            {
                deleteFileLogo(id);
                service.Delete(id);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.DeleteError, error: true).Show();
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomers").Reload();
            }
            return this.Direct();
        }
        public ActionResult CreateDatabase(int id)
        {
            try
            {
                service.CreateDatabase(id, User.ID);
                Ultility.CreateNotification(message: Message.CreateDatabaseSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.CreateDatabaseError, error: true).Show();
            }
            return this.Direct();
        }
        public ActionResult DeleteDatabase(int id)
        {
            try
            {
                //Database database = SystemConfig.Config.Database;
                //service.DeleteDatabase(id, database, User.ID);
                //Ultility.CreateNotification(message: Message.DeleteDatabaseSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.DeleteDatabaseError, error: true).Show();
            }
            return this.Direct();
        }
        public ActionResult UploadLogo(int id, string code)
        {
            try
            {
                var fileUploadField = X.GetCmp<FileUploadField>("FileUploadField1");
                string path = PathUpload.Logo;
                upload(ref path, fileUploadField, FormatFile.Image, code);
                if (path != PathUpload.Logo)
                {
                    deleteFileLogo(id);
                    service.Update(id, path);
                    X.GetCmp<Store>("StoreCustomers").Reload();
                    X.GetCmp<Button>("btnUpload1").Disabled = true;
                    X.GetCmp<Window>("WindowLogoEdit").Close();
                    X.Msg.Hide();
                }
                return this.Direct();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        private void deleteFileLogo(int id) {
            var logo = service.GetLogoById(id);
            if (!string.IsNullOrEmpty(logo))
            {
                System.IO.File.Delete(Server.MapPath(logo));
            }
        }
        private void upload(ref string path, FileUploadField fileUploadField, FormatFile format, string newFileName = "")
        {
            //case file upload is correct
            if (fileUploadField.HasFile)
            {
                //get file name
                string fileName = fileUploadField.PostedFile.FileName;

                //get file size (KB)
                decimal fileSize = Convert.ToDecimal(fileUploadField.PostedFile.ContentLength) / 1024;

                //case format of file is incorrect
                if (!Ultility.CheckFormatFile(fileName, format))
                {
                    Ultility.ShowMessageBox(message: Message.NotFileImage, icon: MessageBox.Icon.ERROR);
                    return;
                }

                //check path to save file not or exist
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(Server.MapPath(path));
                }

                //get new file name
                newFileName = string.IsNullOrEmpty(newFileName) ? fileName : newFileName + "_" + DateTime.Now.ToFileTime().ToString() + '.' + fileName.Split('.').Last().ToLower();

                //get full path for file upload
                path = path + newFileName;

                //upload file
                fileUploadField.PostedFile.SaveAs(Server.MapPath(path));
            }

            //case file upload is incorrect
            else
            {
                Ultility.ShowMessageBox(message: Message.NotFileUpload, icon: MessageBox.Icon.ERROR);
            }
        }
    }
}

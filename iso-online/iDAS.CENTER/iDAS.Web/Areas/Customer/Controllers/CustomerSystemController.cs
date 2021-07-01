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
namespace iDAS.Web.Areas.Customer.Controllers
{
    [BaseSystem(Name = "Quản lý hệ thống", IsActive = true, IsShow = true, Position = 4, Icon = "ApplicationSideExpand")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class CustomerSystemController : BaseController
    {
        CustomerSystemSV CustomerSystemSV = new CustomerSystemSV();
        CustomerSV CustomerSV = new CustomerSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadSystem()
        {
            SystemSV service = new SystemSV();
            List<SystemItem> lst = service.GetAllCbo();
            return this.Store(lst);
        }
        public ActionResult LoadCustomers(StoreRequestParameters parameters, int? systemID = 0, int updateId = 0)
        {
            if (systemID > 0)
            {
                int totalCount;
                var customers = CustomerSystemSV.GetAll(parameters.Page, parameters.Limit, out totalCount, (int)systemID, updateId);
                return this.Store(customers, totalCount);
            }
            return this.Store(new List<CustomerSystemItem>());
        }

        #region Cập nhật khách hàng hệ thống
        [BaseSystem(Name = "Cập nhật khách hàng hệ thống", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateCustomer(string data)
        {
            var customerSystem = Ext.Net.JSON.Deserialize<CustomerSystemItem>(data);
            try
            {
                var Customer = new CustomerItem()
                {
                    ID = customerSystem.CustomerID,
                    Represent = customerSystem.Company,
                    Name = customerSystem.Name,
                    IsNew = customerSystem.IsNew,
                    IsActive = customerSystem.IsActive,
                };
                CustomerSystemSV.Update(customerSystem, User.ID);
                //CustomerSV.UpdateCustomer(Customer, User.ID);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomers").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Cập nhật Logo
        [BaseSystem(Name = "Cập nhật logo", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateLogo(int id)
        {
            try
            {

                CustomerSystemSV.UpdateLogo(id, User.ID);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
                X.GetCmp<Store>("StoreCustomers").Reload();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true).Show();
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Detail(int id, int systemID)
        {
            var data = CustomerSystemSV.GetCustomerByID(id, systemID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };

        }
        #endregion

        #region Thiết lập hệ thống
        [BaseSystem(Name = "Thiết lập hệ thống", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmCreateSystem(int id, int systemID)
        {
            try
            {
                var data = CustomerSystemSV.GetCustomerByID(id, systemID);//Lay thong tin KH
                data.SystemID = systemID;
                return new Ext.Net.MVC.PartialViewResult { ViewName = "CreateSystem", Model = data };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region Tạo hệ thống
        [BaseSystem(Name = "Tạo hệ thống", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult SetupSystem(int customerId, int systemId)
        {
            try
            {
                SystemSV systemSV = new SystemSV();
                systemSV.SetupSystem(customerId, systemId, User.ID);
                CustomerSystemSV.Active(customerId, systemId, User.ID);
                Ultility.CreateMessageBox(title: "Thông báo", message: Message.CreateDatabaseSuccess);
                X.GetCmp<Store>("StoreCustomers").Reload();
            }
            catch (Exception ex)
            {
                Ultility.CreateMessageBox(title: "Thông báo", message: Message.CreateDatabaseError, icon: MessageBox.Icon.ERROR);
            }
            return this.Direct();
        }
        public ActionResult CreateDatabase(CustomerSystemItem obj)
        {
            try
            {
                if (obj != null)
                {

                    System.Net.WebClient client = new System.Net.WebClient();
                    string url = "http://localhost:54400/CenterSystem/System/SetupSystem?databaseName={0}&&company={1}&&name={2}&&email={3}";
                    obj.Code = obj.Code.Trim() + "DB";
                    url = string.Format(url, obj.Code, obj.Company, obj.Name);
                    string result = client.DownloadString(url);
                    // Ngày 12/08/2014 Thanhnv Lỗi Cập nhật sau              

                    Ultility.CreateNotification(message: Message.CreateDatabaseSuccess);
                }
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
        #endregion

        #region Cập nhật hệ thống
        [BaseSystem(Name = "Cập nhật hệ thống", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateSystem(int customerId, int systemId)
        {
            try
            {
                //CustomerSystemSV.UpdateSystem(customerId, systemId);
                SystemSV SystemSV = new SystemSV();
                SystemSV.UpdateSystem(customerId, systemId);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Cập nhật thông tin hệ thống", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateDataBase(CustomerSystemItem obj)
        {
            try
            {
                //Cap nhat thong tin DB vao bang System
                if (CustomerSystemSV.UpdateDataBase(obj, User.ID))
                {
                    X.Msg.Notify("Thông báo", "Cập nhật thành công!").Show();

                    X.GetCmp<Window>("winCreateSys").Close();
                    X.GetCmp<Store>("StoreCustomers").Reload();
                }
                return this.Direct();
            }
            catch
            {
                X.Msg.Notify("Thông báo", "Có lỗi xảy ra trong quá trình cập nhật!").Show();
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Cập nhật dung lượng
        [BaseSystem(Name = "Cập nhật dung lượng", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateDataSize(int customerId, int systemId)
        {
            try
            {
                CustomerSystemSV.UpdateDataSize(customerId, systemId);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
                X.GetCmp<Store>("StoreCustomers").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            return this.Direct();
        }
        #endregion

        //#region Dịch vụ đăng ký
        //[BaseSystem(Name = "Xem danh sách dịch vụ đăng ký", IsActive = true, IsShow = false)]
        //[SystemAuthorize(CheckAuthorize = true)]
        //public ActionResult ServiceRegister(int id)
        //{
        //    ViewData["id"] = id;
        //    return new Ext.Net.MVC.PartialViewResult { ViewName = "ServiceRegister", ViewData = ViewData };
        //}
        //public ActionResult LoadServiceRegister(StoreRequestParameters parameters, int id = 0)
        //{
        //    if (id > 0)
        //    {
        //        int totalCount;
        //        var serviceRegister = CustomerSystemSV.GetServiceByCustomerSystemID(parameters.Page, parameters.Limit, out totalCount, id);
        //        return this.Store(serviceRegister, totalCount);
        //    }
        //    return this.Store(new List<CustomerRegisterServiceItem>());
        //}
        //[BaseSystem(Name = "Cập nhật hệ thống", IsActive = true, IsShow = false)]
        //[SystemAuthorize(CheckAuthorize = true)]
        //public ActionResult UpdateServiceSystem(int id, int customerSystemId, int systemId, int isoId)
        //{
        //    try
        //    {
        //        CustomerSystemSV.UpdateCustomerRegisterSystem(id, customerSystemId, isoId, systemId);
        //        X.GetCmp<Store>("StoreServiceRegister").Reload();
        //        Ultility.CreateNotification(message: Message.UpdateSuccess);
        //    }
        //    catch
        //    {
        //        Ultility.CreateNotification(message: Message.UpdateError, error: true);
        //    }
        //    return this.Direct();
        //}
        //[BaseSystem(Name = " Hủy Dịch Vụ")]
        //[SystemAuthorize]
        //public ActionResult CancelServiceSystem(int id, int customerSystemId, int systemId, int isoId)
        //{
        //    try
        //    {
        //        CustomerSystemSV.CancelCustomerRegisterSystem(id, customerSystemId, isoId, systemId);
        //        X.GetCmp<Store>("StoreServiceRegister").Reload();
        //        Ultility.CreateNotification(message: Message.UpdateSuccess);
        //    }
        //    catch
        //    {
        //        Ultility.CreateNotification(message: Message.UpdateError, error: true);
        //    }
        //    return this.Direct();
        //}
        //#endregion

        #region
        public ActionResult ModuleSetting(int id, int systemId)
        {
            try
            {
                var data = new CustomerSystemItem();
                data.SystemID = systemId;
                data.ID = id;
                return new Ext.Net.MVC.PartialViewResult { ViewName = "ModuleSetting", Model = data };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult LoadModule(StoreRequestParameters parameters, int customerSystemId, int systemId)
        {
            int totalCount;
            var customers = CustomerSystemSV.GetModule(parameters.Page, parameters.Limit, out totalCount, systemId, customerSystemId);
            return this.Store(customers, totalCount);
        }
        public ActionResult UpdateModule(string data, int customerId)
        {
            var moduleItem = Ext.Net.JSON.Deserialize<CustomerSystemModuleItem>(data);
            moduleItem.CustomerSystemID = customerId;
            try
            {
                CustomerSystemSV.UpdateModule(moduleItem, User.ID);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
            }
            catch(Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreModuleSetting").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Khác
        private void deleteFileLogo(string logo)
        {

            if (!string.IsNullOrEmpty(logo))
            {
                System.IO.File.Delete(Server.MapPath(logo));
            }
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Client, Duration = 120 * 60, VaryByParam = "IsUpdate")]
        public ActionResult LoadLogoByID(int customerId = 0, bool IsUpdate = false)
        {
            byte[] byedata = new byte[1];
            if (customerId != 0)
            {
                byedata = CustomerSystemSV.GetLogoFileById(customerId);
                if (byedata != null)
                {
                    return this.File(byedata, "image/jpeg");
                }
                else byedata = new byte[1];
            }
            return this.File(byedata, "image/jpeg");
        }
        #endregion
    }
}

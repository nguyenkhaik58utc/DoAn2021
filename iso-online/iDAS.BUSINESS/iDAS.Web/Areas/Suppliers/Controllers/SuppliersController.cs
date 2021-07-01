using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.Controllers;

namespace iDAS.Web.Areas.Suppliers.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Danh sách nhà cung cấp", Icon = "PagePortraitShot", IsActive = true, IsShow = true, Position = 1)]
    public class SuppliersController : BaseController
    {
        //
        // GET: /Suppliers/Suppliers/ VinhDQ 07/2015
        private SupplierSV suppSV = new SupplierSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int groupSupplierID = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<SupplierItem> lstData = new List<SupplierItem>();
            if (groupSupplierID != 0)
            {
                lstData = suppSV.GetAllByGroupID(filter, groupSupplierID);                
            }
            return this.Store(lstData, filter.PageTotal);
            
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowDetail(int id)
        {
            var obj = suppSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd(string groupID="1")
        {
            var data = new SupplierItem();
            int gID = Int16.Parse(groupID);
            data.SuppliersGroupId = gID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id = 0)
        {
            var data = suppSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(SupplierItem data, bool val)
        {
            try
            {
                if (data.ID == 0)
                {
                    if (suppSV.CheckCodeExits(data.Code))
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Mã NCC đã tồn tại!"
                        });
                    }
                    else
                    {
                        var fileUploadField = X.GetCmp<FileUploadField>("FileUploadFieldId");
                        if (fileUploadField.PostedFile != null && fileUploadField.PostedFile.ContentLength > 0)
                        {
                            data.ImageFile = new FileItem()
                            {
                                ID = Guid.NewGuid(),
                                Name = fileUploadField.FileName,
                                Size = fileUploadField.FileBytes.Length,
                                Type = fileUploadField.PostedFile.ContentType,
                                Extension = string.IsNullOrWhiteSpace(fileUploadField.FileName) ? "" : fileUploadField.FileName.Substring(fileUploadField.FileName.IndexOf('.') + 1),
                                Data = fileUploadField.FileBytes
                            };
                        }
                        suppSV.Insert(data, User.ID);
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                        if (val)
                            X.GetCmp<Window>("winUpdateSupplier").Close();
                    }
                }
                else
                {
                    var fileUploadField = X.GetCmp<FileUploadField>("FileUploadFieldId");
                    if (fileUploadField.PostedFile != null && fileUploadField.PostedFile.ContentLength > 0)
                    {
                        data.ImageFile = new FileItem()
                        {
                            ID = Guid.NewGuid(),
                            Name = fileUploadField.FileName,
                            Size = fileUploadField.FileBytes.Length,
                            Type = fileUploadField.PostedFile.ContentType,
                            Extension = string.IsNullOrWhiteSpace(fileUploadField.FileName) ? "" : fileUploadField.FileName.Substring(fileUploadField.FileName.IndexOf('.') + 1),
                            Data = fileUploadField.FileBytes
                        };
                    }
                    suppSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    if (val)
                        X.GetCmp<Window>("winUpdateSupplier").Close();
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("stSupplier").Reload();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = true)]
        [SystemAuthorize(AllowAnonymous = false, CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                int ids = int.Parse(stringId);
                suppSV.Delete(ids);                
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!").Show();
                X.GetCmp<Store>("stSupplier").Reload();
                return this.Direct();
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Nhà cung cấp đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        
       public ActionResult GetGroup()
        {
         
            var lst = new SuppliersGroupSV().getCombobox();
            return this.Store(lst);
        }
       public ActionResult GetCountry()
       {

           var lst = new CommonCountrySV().getCombobox();
           return this.Store(lst);
       }
       public ActionResult GetProvine(int countryid = 0)
       {

           if (countryid > 0)
           {
               string countryCode = new CommonCountrySV().getCombobox().FirstOrDefault(i => i.ID == countryid).FullName;
               var lst = new CommonCitySV().getCombobox(countryCode);
               return this.Store(lst);
           }
           return null;
       }
    }
}

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
    [BaseSystem(Name = "Quản lý hướng dẫn", IsActive = true, IsShow = true, Position = 4, Icon = "Book")]
    public class TutorialController : BaseController
    {
        private TutorialSV TutorialSV = new TutorialSV();
        [BaseSystem(Name = "Danh sách Hệ thống", IsActive = true, IsShow = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetDataSystem()
        {
            List<SystemItem> lst = TutorialSV.GetSystem();
            return this.Store(lst);
        }
        public ActionResult LoadTutorial(StoreRequestParameters parameters)
        {
            int totalCount;
            var data = TutorialSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(data, totalCount);
        }
        public ActionResult LoadTutorialBySystemCode(StoreRequestParameters parameters, string systemCode)
        {
            int totalCount;
            var data = TutorialSV.GetBySystemCode(parameters.Page, parameters.Limit, out totalCount, systemCode);
            return this.Store(data, totalCount);
        }
        public ActionResult GetModule(string systemCode)
        {
            List<ModuleItem> lst = TutorialSV.GetModule(systemCode);
            return this.Store(lst);
        }
        public ActionResult GetFunction(string systemCode, string moduleCode)
        {
            List<FunctionItem> lst = TutorialSV.GetFunction(systemCode: systemCode, moduleCode: moduleCode);
            return this.Store(lst);
        }
        public ActionResult FormInsert(string systemCode)
        {
            try
            {
                var data = new TutorialItem() { SystemCode = systemCode };
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = data };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Insert(TutorialItem addData, bool exit = false)
        {
            try
            {
                var fileUploadField = X.GetCmp<FileUploadField>("fileUploadFieldID");
                if (fileUploadField.HasFile)
                {
                    addData.AttachFile = new FileItem()
                    {
                        ID = Guid.NewGuid(),
                        Name = fileUploadField.FileName,
                        Size = fileUploadField.FileBytes.Length,
                        Type = fileUploadField.PostedFile.ContentType,
                        Extension = string.IsNullOrWhiteSpace(fileUploadField.FileName) ?
                                    "" : fileUploadField.FileName.Substring(fileUploadField.FileName.IndexOf('.') + 1),
                        Data = fileUploadField.FileBytes
                    };
                }
                bool insert = TutorialSV.Insert(addData, User.ID);
                if (insert)
                    X.Msg.Notify("Thông báo", "Thêm mới thành công!");
                else
                    X.Msg.Notify("Thông báo", "Có lỗi xảy ra trong quá trình thêm mới!");
                if (exit == true)
                {
                    X.GetCmp<Window>("winAdd").Close();

                }
                X.GetCmp<Store>("stTutorial").Reload();
                X.GetCmp<FormPanel>("frmAdd").Reset();
                return this.Direct();
            }
            catch
            {
                X.Msg.Notify("Thông báo", "Có lỗi xảy ra trong quá trình thêm mới!");
                return RedirectToAction("Index");
            }
        }
        public ActionResult FormUpdate(int id, string systemCode)
        {
            try
            {
                var data = TutorialSV.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Update(StoreRequestParameters parameters, TutorialItem addData, int? ID, bool exit, string imgProduct)
        {
            try
            {
                var fileUploadField = X.GetCmp<FileUploadField>("fileUploadFieldID");
                if (fileUploadField.HasFile)
                {
                    addData.AttachFile = new FileItem()
                    {
                        ID = Guid.NewGuid(),
                        Name = fileUploadField.FileName,
                        Size = fileUploadField.FileBytes.Length,
                        Type = fileUploadField.PostedFile.ContentType,
                        Extension = string.IsNullOrWhiteSpace(fileUploadField.FileName) ?
                                    "" : fileUploadField.FileName.Substring(fileUploadField.FileName.IndexOf('.') + 1),
                        Data = fileUploadField.FileBytes
                    };
                }
                bool update = TutorialSV.Update(addData, User.ID);
                if (update)
                    X.Msg.Notify("Thông báo", "Cập nhật thành công!");
                else
                    X.Msg.Notify("Thông báo", "Bạn chưa thay đổi thông tin nào!");
                if (exit == true)
                {
                    X.GetCmp<Window>("winUpdate").Close();
                    X.GetCmp<Store>("stTutorial").Reload();
                }
                return this.Direct();
            }
            catch
            {
                X.Msg.Notify("Thông báo", "Có lỗi xảy ra trong quá trình thêm mới!");
                return RedirectToAction("Index");
            }
        }
        public ActionResult FormDetail(int id)
        {
            try
            {
                var data = TutorialSV.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult FormDelete(string stringId)
        {
            try
            {
                bool Delete = TutorialSV.DeleteByListId(stringId);
                if (Delete)
                {
                    X.Msg.Notify("Thông báo", "Xóa thành công!");
                    X.GetCmp<Store>("stTutorial").Reload();
                }
                else
                    X.Msg.Notify("Thông báo", "Danh mục này đã được sử dụng không xóa được!");
                return this.Direct();
            }
            catch
            {
                X.Msg.Notify("Thông báo", "Có lỗi xảy ra trong quá trình thêm mới!");
                return RedirectToAction("Index");
            }
        }
    }
}
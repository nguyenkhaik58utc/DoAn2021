using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Web.API.Document;
using iDAS.Web.Areas.Document.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Document.Controllers
{
    public class DocumentFolderController : BaseController
    {
        private DocumentAPI DocumentApi = new DocumentAPI();
        DocumentFolderAPI api = new DocumentFolderAPI();
        // GET: Document/DocumentFolder
        public ActionResult Index()
        {
            return View();
        }

        public List<DocumentFolder> check(List<DocumentFolder> lstAll, int ID)
        {
            List<DocumentFolder> lstFolder = new List<DocumentFolder>();
            foreach (var item in lstAll)
            {
                if (item.ID == ID)
                    lstFolder.Add(item);
                if (item.ParentID == ID)
                {
                    lstFolder.Add(item);
                    check(lstAll, item.ID);
                }
            }
            return lstFolder;
        }

        public ActionResult LoadDataNotId(int ID)
        {
            try
            {
                var allFolder = DocumentApi.GetFolder(false).Result;
                var lstFolderParent = check(allFolder, ID);
                if (lstFolderParent.Count != 0)
                {
                    foreach (var item in lstFolderParent)
                    {
                        allFolder.Remove(item);
                    }
                }
                return this.Store(allFolder);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadData(bool folderOut = false)
        {
            try
            {
                var allFolder = DocumentApi.GetFolder(folderOut).Result;

                return this.Store(allFolder);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }

        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult ShowFrmDetail(int ID)
        {
            var obj = api.GetByID(ID).Result;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }


        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        public ActionResult Create(int ID)
        {
            try
            {
                var obj = api.GetByID(ID).Result;
                if (obj != null)
                    obj.ParentID = obj.ID;
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Create", Model = obj };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Create(DocumentFolder obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkName = api.GetByName(obj.Name, false,obj.ParentID);
                    if (checkName.Result == null)
                    {
                        obj.CreateBy = User.ID;
                        obj.DocumentOut = false;
                        api.Insert(obj);
                        X.GetCmp<Store>("stTreeFolder").Reload();
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                        return this.Direct();
                    }
                    else
                    {
                        Ultility.ShowMessageBox("Không thành công", "Tên thư mục đã tồn tại", MessageBox.Icon.ERROR);
                        return this.Direct();
                    }
                }
                else
                {
                    Ultility.ShowMessageRequest(RequestStatus.ValidError);
                }
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();

        }


        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Update(int ID)
        {
            try
            {
                var obj = api.GetByID(ID).Result;
                return new Ext.Net.MVC.PartialViewResult() { Model = obj };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }

        [HttpPost]
        public ActionResult Update(DocumentFolder objEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkName = api.GetByName(objEdit.Name, false, objEdit.ParentID);
                    if (checkName.Result == null)
                    {
                        objEdit.CreateBy = User.ID;
                        objEdit.DocumentOut = false;
                        api.Update(objEdit);
                        X.GetCmp<Store>("stTreeFolder").Reload();
                        Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                        return this.Direct();
                    }
                    else
                    {
                        Ultility.ShowMessageBox("Không thành công", "Tên thư mục đã tồn tại", MessageBox.Icon.ERROR);
                        return this.Direct();
                    }
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.Direct();
        }

        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Delete()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Delete(int ID)
        {
            try
            {
                var delete = api.Delete(ID);
                X.GetCmp<Store>("stTreeFolder").Reload();
                if (delete.Result != 0)
                    Ultility.ShowMessageRequest(RequestStatus.DeleteSuccess);
                else
                {
                    Ultility.ShowMessageBox("Không thành công", "Thư mục đang được sử dụng", MessageBox.Icon.ERROR);
                }
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct();
            }
        }
    }
}
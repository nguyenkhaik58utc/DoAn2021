using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using Ext.Net;
using Ext.Net.MVC;

namespace iDAS.Web.Areas.Document.Controllers
{
    [BaseSystem(Name = "Quản lý danh mục tài liệu", Icon = "PageLink", IsActive = true, IsShow = true, Position = 1)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class CategoryController : BaseController
    {
        DocumentCategorySV documentCategoryService = new DocumentCategorySV();
        DocumentSV docSV = new DocumentSV();
        private DepartmentSV departmentSV = new DepartmentSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int groupId)
        {
            int totalCount;
            var lst = documentCategoryService.GetAll(parameters.Page, parameters.Limit, out totalCount, groupId);
            return this.Store(lst, totalCount);
        }
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Insert(int id)
        {
            DocumentCategoryItem obj = new DocumentCategoryItem();
            if (id != 0)
            {
                obj.Department = new HumanDepartmentViewItem
                {
                    ID = id,
                    Name = departmentSV.GetByID(id).Name,
                };
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model=obj };
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(DocumentCategoryItem obj)
        {
            obj.CreateBy = User.ID;
            string ck = CheckExits(obj, 0 , obj.Department.ID);
            if (ck.Equals(""))
            {
                documentCategoryService.Insert(obj);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                X.GetCmp<Store>("StDocumentCate").Reload();
                X.GetCmp<Window>("winNewCate").Close();
            }
            else
                Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.WARNING, "Thông báo", ck);

            return this.Direct();
        }
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id)
        {
            var obj = documentCategoryService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(DocumentCategoryItem obj)
        {
            obj.UpdateBy = User.ID;
            string ck = CheckExits(obj, obj.ID, obj.Department.ID);
            if (ck.Equals(""))
            {
                documentCategoryService.Update(obj);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("StDocumentCate").Reload();
                X.GetCmp<Window>("winEditCate").Close();
            }
            else
                Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.WARNING, "Thông báo", ck);

            return this.Direct();
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                if (documentCategoryService.Delete(id))
                {
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("StDocumentCate").Reload();
                }
                else
                    Ultility.ShowMessageBox("Thông báo", "Đang có tài liệu thuộc danh mục tài liệu đã chọn nên không thể xóa!!", MessageBox.Icon.WARNING, MessageBox.Button.OK);

            }
            catch (Exception)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError);
            }

            return this.Direct();
        }
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var obj = documentCategoryService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }
        [BaseSystem(Name = "Xem danh sách tài liệu")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DocumentView(int id = 0)
        {
            if (id > 0)
            {
                var obj = documentCategoryService.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DocumentView", Model = obj };
            }
            return null;

        }
        public ActionResult LoadDocumentByCate(StoreRequestParameters parameters, int cateId)
        {
            int totalCount;
            var lst = docSV.GetAllByCateID(parameters.Page, parameters.Limit, out totalCount, cateId);
            return this.Store(lst, totalCount);
        }
        private string CheckExits(DocumentCategoryItem objDraft, int id = 0, int departmentId=0)
        {
            bool ckexit = false;
            objDraft.Name = objDraft.Name.Trim();
            var docItem = documentCategoryService.CheckExit(id, objDraft.Name, departmentId);
            if (docItem != null)
            {
                ckexit = true;
            }
            if (ckexit)
            {
                var deptName = documentCategoryService.GetByNameDepartment(objDraft.Department.ID);
                if (deptName != null)
                {
                    return "[" + objDraft.Name + "] đã tồn tại trong phòng [" + deptName.Name + "]" + ". Vui lòng nhập tên danh mục khác!";
                }
            }
            return "";
        }
        
    }
}

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
using System.IO;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Department.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Yêu cầu khác", IsActive = true, Icon = "FolderBrick", IsShow = true, Position = 4, Parent = DepartmentRequiredController.Code)]
    public class RequirmentController : BaseController
    {
        private DepartmentRequirmentSV RequirmentSV = new DepartmentRequirmentSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var result = RequirmentSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(result, totalCount);
        }

        #region Cập nhật
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0)
        {
            var data = new DepartmentRequirmentItem();
            if (id != 0)
            {
                data = RequirmentSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(DepartmentRequirmentItem data)
        {
            int id = data.ID;
            try
            {
                if (id != 0)
                {
                    RequirmentSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    id = RequirmentSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winRequirmentForm").Close();
                X.GetCmp<Store>("StoreRequirment").Reload();
            }
            return this.Direct(id);
        }
        #endregion

        #region Xóa
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            bool success = false;
            try
            {
                if (id != 0)
                {
                    if (RequirmentSV.Delete(id))
                    {
                        Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                        X.GetCmp<Store>("StoreRequirment").Reload();
                        success = true;
                    }
                    else
                    {
                        Ultility.CreateNotification(message: RequestMessage.DataUsing, error: true);
                    }
                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct(success);
        }
        #endregion

        #region Xem chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new DepartmentRequirmentItem();
            if (id != 0)
            {
                data = RequirmentSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Nội dung
        [HttpGet]
        public ActionResult Content(int id = 0)
        {
            var data = new DepartmentRequirmentItem();
            if (id != 0)
            {
                data.Content = RequirmentSV.GetContent(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Content", Model = data };
        }
        #endregion

        #region Lấy danh sách ISO
        public ActionResult GetISO()
        {
            return this.Store(RequirmentSV.GetISO());
        }
        #endregion

        #region Dữ liệu mẫu
        public ActionResult DataTemplate()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DataTemplate" };
        }
        public ActionResult LoadTemplateData(StoreRequestParameters parameters)
        {
            int totalCount;
            var result = new CenterDepartmentRequirmentSV().GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(result, totalCount);
        }
        [HttpGet]
        public ActionResult ContentTemplate(int id = 0)
        {
            var data = new DepartmentRequirmentItem();
            if (id != 0)
            {
                data.Content = new CenterDepartmentRequirmentSV().GetContent(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Content", Model = data };
        }
        public ActionResult InsertDataTemplate(int id)
        {
            try
            {
                RequirmentSV.InsertDataTemplate(id, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        #endregion
    }
}

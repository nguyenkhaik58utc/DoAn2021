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
    [BaseSystem(Name = "Quy định", IsActive = true, Icon = "Building", IsShow = true, Position = 2, Parent = DepartmentInfoController.Code)]
    public class RegulatoryController : BaseController
    {
        private DepartmentRegulatorySV RegulatorySV = new DepartmentRegulatorySV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var result = RegulatorySV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(result, totalCount);
        }
        public ActionResult Content(int id = 0)
        {
            string result = "";
            if (id != 0)
            {
                result = RegulatorySV.GetContent(id);
            }
            return this.Direct(result == null ? "" : result);
        }

        #region Cập nhật
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0)
        {
            var data = new DepartmentRegulatoryItem();
            if (id != 0)
            {
                data = RegulatorySV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(DepartmentRegulatoryItem data)
        {
            int id = data.ID;
            try
            {
                if (id != 0)
                {
                    RegulatorySV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    id = RegulatorySV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winRegulatoryForm").Close();
                X.GetCmp<Store>("StoreRegulatory").Reload();
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
                    if (RegulatorySV.Delete(id))
                    {
                        Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                        X.GetCmp<Store>("StoreRegulatory").Reload();
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
            var data = new DepartmentRegulatoryItem();
            if (id != 0)
            {
                data = RegulatorySV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Lấy danh sách ISO
        public ActionResult GetISO()
        {
            return this.Store(RegulatorySV.GetISO());
        }
        #endregion
    }
}

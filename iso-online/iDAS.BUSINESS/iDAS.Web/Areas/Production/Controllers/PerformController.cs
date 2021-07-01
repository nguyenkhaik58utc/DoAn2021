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
namespace iDAS.Web.Areas.Production.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Thực hiện", IsActive = true, IsShow = true, Position = 5, Icon = "WrenchOrange")]
    public class PerformController : BaseController
    {
        private ProductionPerformSV ProductionPerformSV = new ProductionPerformSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int commandId)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ProductionPerformSV.GetByCommand(filter, commandId);
            return this.Store(result, filter.PageTotal);
        }

        #region Cập nhật
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0, int commandId = 0, string commandCode = "", int departmentId = 0, string departmentName = "")
        {
            var data = new ProductionPerformItem();
            if (id != 0)
            {
                data = ProductionPerformSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ProductionCommandID = commandId;
                data.CommandCode = commandCode;
                data.HumanDepartment = new HumanDepartmentViewItem() { ID = departmentId, Name = departmentName };
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(ProductionPerformItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    ProductionPerformSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    ProductionPerformSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePerform").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xóa
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    ProductionPerformSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("StorePerform").Reload();
                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new ProductionPerformItem();
            if (id != 0)
            {
                data = ProductionPerformSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Giám sát sản xuất
        public ActionResult MonitorForm(int id = 0)
        {
            var data = new ProductionPerformItem();
            data.ID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Monitor", Model = data };
        }
        public ActionResult LoadPerformResult(StoreRequestParameters parameters, int id)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ProductionPerformSV.GetResult(filter, id);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult UpdateResult(string data)
        {
            var performResult = Ext.Net.JSON.Deserialize<ProductionPerformResultItem>(data);
            try
            {
                ProductionPerformSV.UpdateResult(performResult, User.ID);
                X.GetCmp<Store>("StoreMonitorResult").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        public ActionResult InsertProductEmployee(int id)
        {
            try
            {
                ProductionPerformSV.InsertProductEmployee(id, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreMonitorResult").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Dùng chung
        #endregion
    }
}


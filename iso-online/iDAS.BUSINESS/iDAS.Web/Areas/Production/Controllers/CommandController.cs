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
    [BaseSystem(Name = "Lệnh sản xuất", IsActive = true, IsShow = true, Position = 4, Icon = "IpodSound")]
    public class CommandController : BaseController
    {
        private ProductionCommandSV ProductionCommandSV = new ProductionCommandSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int departmentId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ProductionCommandSV.GetByDepartment(filter, departmentId);
            return this.Store(result, filter.PageTotal);
        }

        #region Cập nhật
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0, int departmentId = 0, string departmentName = "")
        {
            var data = new ProductionCommandItem();
            if (id != 0)
            {
                data = ProductionCommandSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.HumanDepartment = new HumanDepartmentViewItem()
                {
                    ID = departmentId,
                    Name = departmentName
                };
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(ProductionCommandItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    ProductionCommandSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    ProductionCommandSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCommand").Reload();
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
                    ProductionCommandSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("StoreCommand").Reload();
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
            var data = new ProductionCommandItem();
            if (id != 0)
            {
                data = ProductionCommandSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Điều khiển lệnh (Bao gồm: bắt đầu, tạm dừng, tiếp tục, kết thúc lệnh)
        public ActionResult StartCommand(int id)
        {
            try
            {
                ProductionCommandSV.Start(id);
                X.GetCmp<Store>("StoreCommand").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        public ActionResult PauseCommand(int id, bool isPause = true)
        {

            try
            {
                ProductionCommandSV.Pause(id, isPause);
                X.GetCmp<Store>("StoreCommand").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        [HttpGet]
        public ActionResult Finish(int id)
        {
            var data = new ProductionCommandItem() { ID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Finish", Model = data };
        }
        [HttpPost]
        public ActionResult Finish(ProductionCommandItem data)
        {
            ProductionCommandSV.Finish(data.ID, data.FinishTime);
            X.GetCmp<Store>("StoreCommand").Reload();
            return this.Direct();
        }
        #endregion

        #region Kế hoạch trong ngày
        public ActionResult LoadPlanInDay(StoreRequestParameters parameters, int departmentId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ProductionCommandSV.GetPlanByDepartment(filter, departmentId);
            return this.Store(result, filter.PageTotal);
        }
        #endregion

        #region Dùng chung
        public ActionResult GetAll(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ProductionCommandSV.GetAll(filter);
            return this.Store(result, filter.PageTotal);
        }
        #endregion
    }
}


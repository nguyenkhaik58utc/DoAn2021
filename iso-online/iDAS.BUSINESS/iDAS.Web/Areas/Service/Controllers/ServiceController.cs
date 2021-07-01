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

namespace iDAS.Web.Areas.Service.Controllers
{
    [BaseSystem(Name = "Danh mục dịch vụ", Icon = "PageGear", IsActive = true, IsShow = true, Position = 1)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ServiceController : BaseController
    {
        private ServiceSV serviceSV = new ServiceSV();
        private ServiceCategorySV serviceCategorySV = new ServiceCategorySV();
        private readonly string storeLevel = "stCategory";
        //
        // GET: /Service/Service/
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int cateId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = serviceSV.GetByCate(filter, cateId);
            return this.Store(lst, filter.PageTotal);

        }
        public ActionResult GetDataStage(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = serviceSV.GetAll(filter);
            return this.Store(lst, filter.PageTotal);
        }
        public ActionResult GetDataCombo()
        {
            var lst = serviceSV.GetForCombobox();
            return this.Store(lst);
        }
        [BaseSystem(Name = "Thêm nhóm dịch vụ", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAddCate()
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "CreateServiceCategory" };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult InsertCate(ServiceCategoryItem objNew, bool val)
        {
            try
            {
                if (!objNew.Name.Equals("") && !serviceCategorySV.CheckNameExits(objNew.Name.Trim()))
                {
                    serviceCategorySV.Insert(objNew, User.ID);
                    X.GetCmp<Store>(storeLevel).Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Tên nhóm dịch vụ đã tồn tại vui lòng nhập tên khác!"
                    });
                    return this.Direct("ErrorExited");
                }
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Sửa nhóm dịch vụ", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmUpdateCate(int id)
        {
            try
            {
                var obj = serviceCategorySV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateServiceCategory", Model = obj };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        [BaseSystem(Name = "Xem  chi tiết nhóm dịch vụ", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetailCate(int id)
        {
            try
            {
                var obj = serviceCategorySV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailServiceCategory", Model = obj };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult UpdateCate(ServiceCategoryItem objEdit)
        {
            try
            {
                if (!objEdit.Name.Equals("") && !serviceCategorySV.CheckNameExitEdits(objEdit.ID, objEdit.Name.Trim()))
                {
                    serviceCategorySV.Update(objEdit, User.ID);
                    X.GetCmp<Store>(storeLevel).Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Tên nhóm dịch vụ đã tồn tại vui lòng nhập tên khác!"
                    });
                    return this.Direct("Error");
                }
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xóa nhóm dịch vụ", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteCate(string stringId)
        {
            try
            {
                serviceCategorySV.Delete(stringId);
                X.GetCmp<Store>(storeLevel).Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct("Error");
            }
        }
        public ActionResult GetDataCate(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = serviceCategorySV.GetAll(filter);
            return this.Store(lst, filter.PageTotal);
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd(int cateId = 0)
        {
            try
            {
                var obj = new ServiceItem();
                obj.CategoryID = cateId;
                obj.IsUse = true;
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Create", Model = obj };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult GetDataCbo(StoreRequestParameters parameters)
        {
            List<ServiceCategoryItem> lst = new List<ServiceCategoryItem>();
            lst = serviceSV.GetCateAll();
            return this.Store(lst);
        }
        public ActionResult GetDataCboIsUse(StoreRequestParameters parameters)
        {
            List<ServiceCategoryItem> lst = new List<ServiceCategoryItem>();
            lst = serviceSV.GetCateAll();
            return this.Store(lst);
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmEdit(int id)
        {
            try
            {
                var obj = serviceSV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Edit", Model = obj };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Insert(ServiceItem objNew, bool val)
        {
            try
            {
                if (!objNew.Code.Trim().Equals("") && !serviceSV.CheckCodeExits((int)objNew.CategoryID, objNew.Code.Trim()))
                {
                    serviceSV.Insert(objNew, User.ID);
                    if (val == true)
                    {
                        X.GetCmp<Window>("winNewService").Close();
                    }
                    X.Msg.Notify("Thông báo", "Bạn đã thêm mới thành công!").Show();
                    X.GetCmp<Store>("stService").Reload();
                    X.GetCmp<GridPanel>("gpService").Refresh();
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Mã dịch vụ đã tồn tại vui lòng nhập mã khác!"
                    });
                    return this.Direct("ErrorExited");
                }
                return this.Direct();
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }
        }
        public ActionResult Update(ServiceItem objNew)
        {
            try
            {
                var obj = serviceSV.GetByID(objNew.ID);
                if (!objNew.Code.Trim().Equals("") && serviceSV.CheckCodeExitEdits(objNew.ID, (int)objNew.CategoryID, objNew.Code.Trim()))
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Mã dịch vụ đã tồn tại vui lòng nhập mã khác!"
                    });
                }
                else
                {
                    serviceSV.Update(objNew, User.ID);
                    X.GetCmp<Store>("stService").Reload();
                    X.GetCmp<Window>("winEditService").Close();
                    X.GetCmp<GridPanel>("gpService").Refresh();
                    X.Msg.Notify("Thông báo", "Bạn đã cập nhật thành công!").Show();
                }
                return this.Direct();
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                serviceSV.Delete(stringId);
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!").Show();
                X.GetCmp<Store>("stService").Reload();
                X.GetCmp<GridPanel>("gpService").Refresh();
                return this.Direct();
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Dịch vụ đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xem", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int id)
        {
            try
            {
                var obj = serviceSV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
    }
}

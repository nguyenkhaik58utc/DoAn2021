using System;
using System.Collections.Generic;
using iDAS.Core;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web.Areas.Stock;
using iDAS.Items;
using iDAS.Utilities;
using iDAS.Services;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Danh mục kho", Icon = "PageGear", IsActive = true, IsShow = true, Parent = GroupListController.Code, Position = 1)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class StocksController : BaseController
    {
        private StockSV listStockSV = new StockSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters); 
            var data = listStockSV.GetAll(filter);
            return this.Store(data, filter.PageTotal);
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowWdAddStock()
        {
            try
            {
                string CodeAuto = Common.NextID(listStockSV.GetMaxCode(), "KH");
                return new Ext.Net.MVC.PartialViewResult
                {
                    ViewName = "Create",
                    ViewData = new ViewDataDictionary { { "codeAuto", CodeAuto } }
                };
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }
        public ActionResult Insert(StockItem objNew, bool val)
        {
            try
            {
                string CodeAuto = "";
                if (listStockSV.CheckCodeExits(objNew.Code.Trim()))
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Mã kho đã tồn tại trên hệ thống vui lòng nhập mã khác!"
                    });

                }
                else if (listStockSV.CheckCodeExits(objNew.Name.Trim()))
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên kho đã tồn tại trên hệ thống vui lòng nhập tên khác!"
                    });
                    CodeAuto = Common.NextID(listStockSV.GetMaxCode(), "KH");
                }
                else
                {
                    var fileUploadAvatarField = X.GetCmp<FileUploadField>("FileUploadImageFieldId");
                    if (fileUploadAvatarField.HasFile)
                    {
                        objNew.ImageMap = fileUploadAvatarField.FileBytes;
                    }
                    objNew.Name = objNew.Name.Trim();
                    objNew.CreateAt = DateTime.Now;
                    objNew.CreateBy = User.ID;
                    listStockSV.Insert(objNew);
                    if (val == true)
                    {
                        X.GetCmp<Window>("winNewStock").Close();
                    }
                    X.GetCmp<Store>("stListStock").Reload();
                    X.GetCmp<GridPanel>("gpListStock").Refresh();
                    CodeAuto = Common.NextID(listStockSV.GetMaxCode(), "KH");
                    X.Msg.Notify("Thông báo", "Bạn đã thêm mới thành công!").Show();
                }
                return this.Direct(CodeAuto);
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        public ActionResult Update(StockItem objNew)
        {
            try
            {
                var objOld = listStockSV.GetById(objNew.ID);
                if (!objNew.Name.ToUpper().Equals(objOld.Name.ToUpper()))
                {
                    if (listStockSV.CheckNameExits(objNew.Name))
                    {

                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Tên kho đã tồn tại trên hệ thống vui lòng nhập tên khác!"
                        });
                    }
                    else
                    {
                        var fileUploadAvatarField = X.GetCmp<FileUploadField>("FileUploadImageFieldId");
                        if (fileUploadAvatarField.HasFile)
                        {
                            objNew.ImageMap = fileUploadAvatarField.FileBytes;
                        }
                        objNew.UpdateAt = DateTime.Now;
                        objNew.UpdateBy = User.ID;
                        listStockSV.Update(objNew);
                        X.GetCmp<Store>("stListStock").Reload();
                        X.GetCmp<GridPanel>("gpListStock").Refresh();
                        X.GetCmp<Window>("winEditStock").Close();
                        X.Msg.Notify("Thông báo", "Bạn đã sửa thành công!").Show();
                    }
                }
                else
                {
                    var fileUploadAvatarField = X.GetCmp<FileUploadField>("FileUploadImageFieldId");
                    if (fileUploadAvatarField.HasFile)
                    {
                        objNew.ImageMap = fileUploadAvatarField.FileBytes;
                    }
                    objNew.UpdateAt = DateTime.Now;
                    objNew.UpdateBy = User.ID;
                    listStockSV.Update(objNew);
                    X.GetCmp<Store>("stListStock").Reload();
                    X.GetCmp<GridPanel>("gpListStock").Refresh();
                    X.GetCmp<Window>("winEditStock").Close();
                    X.Msg.Notify("Thông báo", "Bạn đã sửa thành công!").Show();
                }
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowWdEditStock(int id)
        {
            try
            {
                var obj = listStockSV.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Edit", Model = obj };
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                listStockSV.DeleteRange(stringId);
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!").Show();
                X.GetCmp<Store>("stListStock").Reload();
                return this.Direct();
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Kho đã được sử dụng trong hệ thống không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowWdDetailStock(int id)
        {
            try
            {
                var obj = listStockSV.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }
    }
}

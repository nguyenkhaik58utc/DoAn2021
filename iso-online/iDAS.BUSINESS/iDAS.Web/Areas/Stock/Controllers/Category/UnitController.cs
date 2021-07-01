using Ext.Net;
using iDAS.Web.Areas.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using iDAS.Services;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Danh mục đơn vị tính", Icon = "PageGear", IsActive = true, IsShow = true, Parent = GroupListController.Code, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class UnitController : BaseController
    {
        private StockUnitSV stockUnitSV = new StockUnitSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            List<StockProductGroupItem> lst;
            int total;
            lst = stockUnitSV.GetAll(parameters.Page, parameters.Limit, out total);
            return this.Store(new Paging<StockProductGroupItem>(lst, total));
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Insert(string name, string description, bool isuse)
        {
            try
            {
                if (!name.Trim().Equals("") && !stockUnitSV.CheckNameExits(name.Trim()))
                {
                    stockUnitSV.Insert(name, description, isuse, User.ID);
                    X.GetCmp<Store>("stUnit").Reload();
                    X.Msg.Notify("Thông báo", "Bạn đã thêm mới thành công!");
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Tên đơn vị tính đã tồn tại vui lòng nhập tên khác!"
                    });
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
        public ActionResult Update(int id, string name, string description, Nullable<Boolean> isuse)
        {
            try
            {
                var objNew = stockUnitSV.GetById(id);
                if (objNew.Name.ToUpper().Trim().Equals(name.ToUpper().Trim()) != true && stockUnitSV.CheckNameExits(name.Trim()) == true)
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Tên đơn vị tính đã tồn tại vui lòng nhập tên khác!"
                    });
                }
                else
                {
                    objNew.Name = name.Trim();
                    objNew.Description = description;
                    objNew.Active = (bool)isuse;
                    objNew.UpdateAt = DateTime.Now;
                    stockUnitSV.Update(objNew);
                    X.GetCmp<Store>("stUnit").Reload();
                    X.Msg.Notify("Thông báo", "Bạn đã cập nhật thành công!");
                }
                return this.Direct();
            }
            catch
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
                stockUnitSV.DeleteRange(stringId);
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!");
                X.GetCmp<Store>("stUnit").Reload();
                return this.Direct();
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Đơn vị tính đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
    }
}

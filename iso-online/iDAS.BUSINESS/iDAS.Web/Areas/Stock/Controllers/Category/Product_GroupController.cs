using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web.Areas.Stock;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Items;
using iDAS.Services;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Danh mục nhóm vật tư hàng hóa", Icon = "PageGear", IsActive = true, IsShow = true, Parent = GroupListController.Code, Position = 3)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class Product_GroupController : BaseController
    {
        private StockProductGroupSV product_GroupService = new StockProductGroupSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            int total;
            var lst = product_GroupService.GetAll(parameters.Page, parameters.Limit, out total);
            return this.Store(new Paging<StockProductGroupItem>(lst, total));

        }
        /// <summary>
        /// Hàm insert dữ liệu
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="isuse"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Insert(string name, string description, Nullable<Boolean> isuse)
        {
            try
            {
                StockProductGroupItem obj = new StockProductGroupItem
                {
                    Name = name.Trim(),
                    Description = description,
                    IsUse = (bool)isuse,
                    CreateAt = DateTime.Now,
                    CreateBy = User.ID
                };
                if (!obj.Name.Trim().Equals("") && !product_GroupService.CheckNameExits(obj.Name.Trim()))
                {
                    product_GroupService.Insert(obj);
                    X.GetCmp<Store>("stProduct_Group").Reload();
                    X.Msg.Notify("Thông báo", "Bạn đã thêm mới thành công!");
                }
                else
                    return this.Direct("ErrorExited");
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        /// <summary>
        /// Hàm update dữ liệu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="isuse"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Update(int id, string name, string description, Nullable<Boolean> isuse)
        {
            try
            {
                var objNew = product_GroupService.GetById(id);
                if (objNew.Name.ToUpper().Trim().Equals(name.ToUpper().Trim()) != true && product_GroupService.CheckNameExits(name.Trim()) == true)
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Tên nhóm vật tư hàng hóa đã tồn tại vui lòng nhập tên khác!"
                    });

                }
                else
                {
                    objNew.Name = name.Trim();
                    objNew.Description = description;
                    objNew.IsUse = (bool)isuse;
                    objNew.UpdateAt = DateTime.Now;
                    product_GroupService.Update(objNew);
                    X.GetCmp<Store>("stProduct_Group").Reload();
                    X.Msg.Notify("Thông báo", "Bạn đã cập nhật thành công!");
                }
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        /// <summary>
        ///  Action xóa dữ liệu
        /// </summary>
        /// <param name="stringId"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                product_GroupService.DeleteRange(stringId);
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!");
                X.GetCmp<Store>("stProduct_Group").Reload();
                return this.Direct();
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Nhóm vật tư hàng hóa đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
    }
}

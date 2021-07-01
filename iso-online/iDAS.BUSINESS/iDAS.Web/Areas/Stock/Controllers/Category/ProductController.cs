using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web.Areas.Stock;
using iDAS.Items;
using iDAS.Utilities;
using iDAS.Services;
using iDAS.DA;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Danh mục vật tư hàng hóa", Icon = "PageGear", IsActive = true, IsShow = true, Parent = GroupListController.Code, Position = 4)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProductController : BaseController
    {
        private StockProductSV productService = new StockProductSV();
        private StockSV stockSV = new StockSV();
        private StockProductGroupSV productGroupSV = new StockProductGroupSV();
        private StockProductBuildSV product_BuildService = new StockProductBuildSV();
        private StockUnitSV unitService = new StockUnitSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters); 
            var lst = productService.GetAll(filter);
            return this.Store(lst, filter.PageTotal);
        }
        public ActionResult GetDataOfStructure(StoreRequestParameters parameters, int build_Id)
        {
            int total;
            var lst = product_BuildService.GetAll(parameters.Page, parameters.Limit, out total, build_Id);
            return this.Store(new Paging<StockProductBuildItem>(lst, total));
        }
        public ActionResult LoadCboProduct(int start, int limit, int page, string query, int prodcuctCreate_id)
        {
            if (string.IsNullOrEmpty(query))
                query = "";
            List<StockProductItem> lstProduct = productService.GetProductsForCreate(start, limit, page, query, prodcuctCreate_id);
            Paging<StockProductItem> plants = new Paging<StockProductItem>(lstProduct, lstProduct.Count);
            return this.Store(plants.Data, plants.TotalRecords);
        }
        public ActionResult LoadGroup(int start, int limit, int page, string query)
        {
            List<ComboboxItem> lstGroup_Product = productGroupSV.Combobox(query);
            Paging<ComboboxItem> plants = new Paging<ComboboxItem>(lstGroup_Product, lstGroup_Product.Count);
            return this.Store(plants.Data, plants.TotalRecords);
        }
        public ActionResult LoadStock(int start, int limit, int page, string query)
        {
            if (string.IsNullOrEmpty(query))
                query = "";
            List<ComboboxItem> lstStock = stockSV.Combobox(query);
            Paging<ComboboxItem> plants = new Paging<ComboboxItem>(lstStock, lstStock.Count);
            return this.Store(plants.Data, plants.TotalRecords);
        }
        public ActionResult LoadUnit()
        {
            var lst = unitService.GetListAll();
            return this.Store(lst);
        }
        public ActionResult LoadUnitAdd()
        {
            var lst = unitService.GetListAllIsUse();
            return this.Store(lst);
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowWdAddProduct()
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Create"};
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowWdEditProduct(int id)
        {
            try
            {
                var obj = productService.GetById(id);
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
                productService.DeleteRange(stringId);
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!");
                X.GetCmp<Store>("stProduct").Reload();
                return this.Direct();
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Vật tư hàng hóa đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowWdDetailProduct(int id)
        {
            try
            {
                var obj = productService.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [BaseSystem(Name = "Xem danh sách vật tư cấu thành sản phẩm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowWdCreateStruct(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "CreateStructure", ViewData = new ViewDataDictionary { { "build_id", id } } };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [BaseSystem(Name = "Thêm mới vật tư cấu thành sản phẩm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowWdAddProductStructure(int build_id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "CreateBuild", ViewData = new ViewDataDictionary { { "build_id", build_id } } };
            }
            catch
            {
                return this.Direct();
            }

        }
        [BaseSystem(Name = "Sửa vật tư cấu thành sản phẩm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowWdEditProductStructure(int id, int build_id)
        {
            try
            {
                var obj = product_BuildService.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "EditBuild", Model = obj, ViewData = new ViewDataDictionary { { "build_id", build_id } } };
            }
            catch
            {
                return this.Direct();
            }

        }
        public ActionResult Insert(StockProductItem objNew, bool val)
        {
            try
            {
                var fileUploadAvatarField = X.GetCmp<FileUploadField>("FileUploadImageFieldId");
                if (fileUploadAvatarField.HasFile)
                {
                    objNew.ImageProduct = fileUploadAvatarField.FileBytes;
                }
                objNew.Name = objNew.Name.Trim();
                objNew.CreateAt = DateTime.Now;
                objNew.CreateBy = User.ID;
                if (productService.CheckCodeExits(objNew.Code.Trim()))
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Mã vật tư hàng hóa đã tồn tại trên hệ thống vui lòng nhập mã khác!"
                    });
                }
                else if (productService.CheckNameExits(objNew.Name.Trim()))
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên vật tư hàng hóa đã tồn tại trên hệ thống vui lòng nhập tên khác!"
                    });
                }
                else if (!productService.CheckNameExits(objNew.Name.Trim()) && !productService.CheckCodeExits(objNew.Code.Trim()))
                {
                    productService.Insert(objNew);
                    if (val == true)
                    {
                        X.GetCmp<Window>("winNewProduct").Close();
                    }
                    X.GetCmp<FormPanel>("frProduct").Reset();
                    X.GetCmp<Store>("stProduct").Reload();
                    X.GetCmp<GridPanel>("gpProduct").Refresh();
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
        public ActionResult InsertBuild(StockProductBuildItem objNew, bool val, int build_id)
        {
            try
            {
                objNew.Build_ID = build_id;
                product_BuildService.Insert(objNew);
                if (val == true)
                {
                    X.GetCmp<Window>("winNewProductStructure").Close();
                }
                X.GetCmp<Store>("stStructure").Reload();
                X.GetCmp<GridPanel>("gpStructure").Refresh();
                X.Msg.Notify("Thông báo", "Bạn đã thêm mới thành công!");
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        public ActionResult GetRecordProduct(int id)
        {

            var obj = productService.GetById(id);
            return this.Direct(obj);
        }
        public ActionResult UpdateBuild(StockProductBuildItem objNew, int build_id)
        {
            try
            {
                var objOld = product_BuildService.GetById(objNew.ID);
                objOld.Amount = objNew.Amount;
                objOld.Unit = objNew.Unit;
                objOld.StockProductID = objNew.StockProductID;
                objOld.Price = objNew.Price;
                objOld.Quantity = objNew.Quantity;
                product_BuildService.Update(objOld);
                X.GetCmp<Store>("stStructure").Reload();
                X.GetCmp<GridPanel>("gpStructure").Refresh();
                X.GetCmp<Window>("winEditProductStructure").Close();
                X.Msg.Notify("Thông báo", "Bạn đã sửa thành công!");
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        public ActionResult Update(StockProductItem objNew)
        {
            try
            {
                var objOld = productService.GetById(objNew.ID);
                if (productService.CheckCodeEditExits(objNew.ID, objNew.Code.Trim()))
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Mã vật tư hàng hóa đã tồn tại trên hệ thống vui lòng nhập mã khác!"
                    });
                    return this.Direct("Error");
                }
                else
                {
                    if (!objNew.Name.ToUpper().Equals(objOld.Name.ToUpper()))
                    {
                        if (productService.CheckNameExits(objNew.Name))
                        {
                            X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.ERROR,
                                Title = "Thông báo",
                                Message = "Tên vật tư hàng hóa đã tồn tại trên hệ thống vui lòng nhập tên khác!"
                            });
                        }
                        else
                        {
                            var fileUploadAvatarField = X.GetCmp<FileUploadField>("FileUploadImageFieldId");
                            if (fileUploadAvatarField.HasFile)
                            {
                                objNew.ImageProduct = fileUploadAvatarField.FileBytes;
                            }
                            objOld.UpdateAt = DateTime.Now;
                            objOld.UpdateBy = User.ID;
                            objOld.Active = objNew.Active;
                            objOld.Type_ID = objNew.Type_ID;
                            objOld.StockID = objNew.StockID;
                            objOld.Description = objNew.Description;
                            objOld.Group_ID = objNew.Group_ID;
                            objOld.Code = objNew.Code;
                            objOld.Barcode = objNew.Barcode;
                            objOld.Name = objNew.Name.Trim();
                            objOld.Unit_ID = objNew.Unit_ID;
                            objOld.Origin = objNew.Origin;
                            objOld.VAT_ID = objNew.VAT_ID;
                            objOld.MinStock = objNew.MinStock;
                            objOld.MaxStock = objNew.MaxStock;
                            objOld.CurrentCost = objNew.CurrentCost;
                            objOld.Org_Price = objNew.Org_Price;
                            objOld.Sale_Price = objNew.Sale_Price;
                            objOld.Retail_Price = objNew.Retail_Price;
                            productService.Update(objOld);
                            X.GetCmp<Store>("stProduct").Reload();
                            X.GetCmp<GridPanel>("gpProduct").Refresh();
                            X.GetCmp<Window>("winEditProduct").Close();
                            X.Msg.Notify("Thông báo", "Bạn đã sửa thành công!");
                        }
                    }
                    else
                    {
                        var fileUploadAvatarField = X.GetCmp<FileUploadField>("FileUploadImageFieldId");
                        if (fileUploadAvatarField.HasFile)
                        {
                            objOld.ImageProduct = fileUploadAvatarField.FileBytes;
                        }
                        objOld.UpdateAt = DateTime.Now;
                        objOld.UpdateBy = User.ID;
                        objOld.Active = objNew.Active;
                        objOld.Type_ID = objNew.Type_ID;
                        objOld.StockID = objNew.StockID;
                        objOld.Description = objNew.Description;
                        objOld.Group_ID = objNew.Group_ID;
                        objOld.Code = objNew.Code;
                        objOld.Barcode = objNew.Barcode;
                        objOld.Name = objNew.Name.Trim();
                        objOld.Unit_ID = objNew.Unit_ID;
                        objOld.Origin = objNew.Origin;
                        objOld.VAT_ID = objNew.VAT_ID;
                        objOld.MinStock = objNew.MinStock;
                        objOld.MaxStock = objNew.MaxStock;
                        objOld.CurrentCost = objNew.CurrentCost;
                        objOld.Org_Price = objNew.Org_Price;
                        objOld.Sale_Price = objNew.Sale_Price;
                        objOld.Retail_Price = objNew.Retail_Price;
                        productService.Update(objOld);
                        X.GetCmp<Store>("stProduct").Reload();
                        X.GetCmp<GridPanel>("gpProduct").Refresh();
                        X.GetCmp<Window>("winEditProduct").Close();
                        X.Msg.Notify("Thông báo", "Bạn đã sửa thành công!");
                    }
                    return this.Direct();
                }
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xóa vật tư cấu thành sản phẩm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteBuild(string stringId)
        {
            try
            {
                product_BuildService.DeleteRange(stringId);
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!");
                X.GetCmp<Store>("stStructure").Reload();
                return this.Direct();
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
    }
}

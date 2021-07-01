using iDAS.Core;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Base;
using iDAS.Services;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Nhập kho đầu kỳ", Icon = "DoorIn", IsActive = true, IsShow = true, Parent = GroupManageStockController.Code, Position = 1)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class InwardStartController : BaseController
    {
        private StockInwardSV stock_InwardSV = new StockInwardSV();
        private StockSV listStockSV = new StockSV();
        private StockProductGroupSV product_GroupSV = new StockProductGroupSV();
        private InventorySV inventorySV = new InventorySV();
        private InventoryDetailSV inventory_DetailSV = new InventoryDetailSV();
        private StockProductSV productService = new StockProductSV();
        private StockInwardDetailSV stock_Inward_DetailSV = new StockInwardDetailSV();
        private TransRefSV trans_RefSV = new TransRefSV();
        //
        // GET: /Stock/InwardStart/
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            var obj = stock_InwardSV.GetMaxCodeStart();
            StockInwardDetailSV.ClearInward_Start();
            string CodeAutoStart = Common.NextID(stock_InwardSV.GetMaxCodeStart(), "DK");
            ViewBag.CodeAutoStart = CodeAutoStart;
            return View();
        }
        public ActionResult GetDataOfProducts(StoreRequestParameters parameters, string stringId = "0")
        {
            string[] arrayId = stringId.Split(',');
           ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
           List<StockProductItem> lstData;
            lstData = productService.GetProductsByGroup(filter,arrayId);
            return this.Store(lstData, filter.PageTotal);
       
        }       
        public ActionResult GetProductsStart(string stringId = "0")
        {
            try
            {
                string[] arrayId = stringId.Split(',');
                var data = stock_Inward_DetailSV.GetListProductStartByStringId(arrayId);
                List<StockInwardDetailItem> lst = new List<StockInwardDetailItem>();
                lst.AddRange(data);
                StockInwardDetailSV.AddProductInward_Start(lst);
                X.GetCmp<Store>("stProductChoice").LoadData(StockInwardDetailSV.StorageInward_Start);
                X.GetCmp<Store>("stProductChoice").Reload();
                X.GetCmp<Window>("frmProducts").Close();
                X.GetCmp<GridPanel>("gpProductChoise").Refresh();
                return this.Direct();
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }

        }
        public ActionResult DeleteProductInStorage(string stringId = "0")
        {
            string[] arrayId = stringId.Split(',');
            foreach (string id in arrayId)
            {
                StockInwardDetailSV.DeleteProductStart(int.Parse(id));
            }
            X.GetCmp<Store>("stProductChoice").Reload();
            return this.Direct();
        }
        public ActionResult GetDataStart()
        {
            var data = StockInwardDetailSV.StorageInward_Start;
            return this.Store(data);
        }
        public ActionResult ResetGridStart()
        {
            StockInwardDetailSV.ClearInward_Start();
            X.GetCmp<Store>("stProductChoice").Reload();
            X.GetCmp<NumberField>("txtDiscount").Reset();
            X.GetCmp<NumberField>("txtFAmount").Reset();
            string CodeAutoStart = Common.NextID(stock_InwardSV.GetMaxCodeStart(), "DK");
            X.GetCmp<TextField>("txtCode").SetValue(CodeAutoStart);
            return this.Direct();
        }
        public ActionResult LoadStock(int start, int limit, int page, string query)
        {
            if (string.IsNullOrEmpty(query))
                query = "";
            List<ComboboxItem> lstStock = listStockSV.Combobox(query);
            Paging<ComboboxItem> plants = new Paging<ComboboxItem>(lstStock, lstStock.Count);
            return this.Store(plants.Data, plants.TotalRecords);
        }
        public ActionResult ShowFrmFindProductStart()
        {
            var data = product_GroupSV.GetListAll();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "FrmFindProductsForStart", Model = data };
        }
        public DirectResult HandleChangesInward_Start(int id, string field, string oldValue, string newValue, string stock_Name, Nullable<decimal> quantity, Nullable<decimal> unitPrice, Nullable<decimal> amount, object product)
        {
            StockInwardDetailItem obj = new StockInwardDetailItem();
            obj.ID = id;
            obj.Amount = amount;
            obj.StockID = listStockSV.GetIdByName(stock_Name);
            obj.Stock_Name = stock_Name;
            if (field.Equals("Quantity"))
            {
                obj.Quantity = quantity;
            }
            if (field.Equals("UnitPrice"))
            {
                obj.UnitPrice = unitPrice;
            }
            StockInwardDetailSV.UpdateProductStart(obj);
            X.GetCmp<Store>("stProductChoice").GetById(id).Commit();
            return this.Direct();
        }
        public ActionResult InsertStart(StockInwardItem obj, Nullable<decimal> discount, Nullable<decimal> fAmount, Nullable<decimal> amount)
        {
            try
            {
                var data = StockInwardDetailSV.StorageInward_Start;
                if (data.Count <= 0)
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Phiếu này rỗng không thể lưu được!"
                    });
                    return this.Direct();
                }
                else
                {
                    var objss = data.Where(t => t.Stock_Name.ToString().Trim().Equals("") == true).ToList();
                    if (objss.Count > 0)
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Kho nhập không được để trống!"
                        });
                        return this.Direct();
                    }
                    else if (!obj.Barcode.Equals("") && !stock_InwardSV.CheckBarcodeExits(obj.Barcode))
                    {

                        obj.CreateBy = User.ID;
                        obj.Amount = amount;
                        obj.CustomerName = obj.CustomerName;
                        obj.Employee_ID = User.ID;
                        int stock_InwardID = stock_InwardSV.InsertStart(obj, discount, fAmount);
                        List<StockInwardDetail> lst = new List<StockInwardDetail>();
                        foreach (var item in data)
                        {
                            lst.Add(new StockInwardDetail()
                            {
                                Active = true,
                                StockInwardID = stock_InwardID,
                                ProductName = item.ProductName,
                                StockProductID = item.StockProductID,
                                RefType = (int)Common.RefType.inward_Start,
                                Stock_ID = item.StockID,
                                Quantity = item.Quantity != null ? item.Quantity : 0,
                                UnitPrice = item.UnitPrice != null ? item.UnitPrice : 0,
                                Limit = DateTime.Now,
                                Description = obj.CustomerName,
                                StoreID = 0,
                                Sorted = 0,
                                Lev1 = 0,
                                Lev2 = 0,
                                Lev3 = 0,
                                Lev4 = 0,
                                Unit = item.Unit,
                                Amount = item.Quantity == 0 ? 0 : item.UnitPrice * item.Quantity,
                                Vat = 0,
                                CurrentQty = 0,
                                Charge = 0,
                                Width = 0,
                                Height = 0,
                                CreateAt = DateTime.Now,
                                CreateBy = User.ID
                            });
                        }
                        inventorySV.InsertStart(lst, obj.Barcode);
                        inventory_DetailSV.InsertStart(lst, obj.Barcode);
                        stock_Inward_DetailSV.Insert(lst);
                        trans_RefSV.InsertInward(stock_InwardID);
                        StockInwardDetailSV.ClearInward_Start();
                        X.GetCmp<FormPanel>("frDetailStock_InwardStart").Reset();
                        X.GetCmp<Store>("stProductChoice").Reload();
                        X.GetCmp<NumberField>("txtDiscount").Reset();
                        X.GetCmp<NumberField>("txtFAmount").Reset();
                        string CodeAutoStart = Common.NextID(stock_InwardSV.GetMaxCodeStart(), "DK");
                        X.GetCmp<TextField>("txtCode").SetValue(CodeAutoStart);
                        X.Msg.Notify("Thông báo", "Bạn đã nhập thành công một phiếu nhập!").Show();
                        return this.Direct();
                    }

                    else
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Mã phiếu nhập đã tồn tại!"
                        });
                        return this.Direct();
                    }
                }
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }
        }
        public ActionResult FrmPrintInwardStart(string code, string namecreate, DateTime date, string des, decimal? amount)
        {
            var data = StockInwardDetailSV.StorageInward_Start;
            CenterCustomerSV systemCustomerService = new CenterCustomerSV();
            var objcus = systemCustomerService.GetCustomerByCode(User.Code);
            var datareport = stock_Inward_DetailSV.GetForReportInwardStart(code, namecreate, date, des, amount, objcus.Company, objcus.FileID, data);
            return View(datareport);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web.Areas.Stock;
using iDAS.Utilities;
using iDAS.Items;
using iDAS.Services;
namespace iDAS.Web.Areas.Stock.Controllers
{

    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Tháo dỡ", Icon = "BuildCancel", IsActive = true, IsShow = true, Parent = AddinController.Code, Position = 5)]
     [SystemAuthorize(CheckAuthorize = false)]
    public class Stock_BuildDismantleController : BaseController
    {
        private StockBuildSV stock_BuildService  = new StockBuildSV();
        private StockBuildDetailSV stock_Build_DetailService = new StockBuildDetailSV();
        private StockProductSV productService = new StockProductSV();
        private StockSV listStockService = new StockSV();
        private TransRefSV trans_RefService = new TransRefSV();
        private InventoryDetailSV inventory_DetailService = new InventoryDetailSV();
        private StockProductBuildSV product_BuildService = new StockProductBuildSV();
        private InventorySV inventoryService = new InventorySV();
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            StockBuildDetailSV.Clear();
            string CodeAuto = Common.NextID(stock_BuildService.GetMaxCode(), "TD");
            ViewBag.CodeAuto = CodeAuto;
            return View();
        }
        public DirectResult HandleChanges(int id, string field, string oldValue, string newValue, string stock_Name, Nullable<decimal> quantity, Nullable<decimal> unitPrice, Nullable<decimal> amount, object product)
        {
            StockBuildDetailItem obj = new StockBuildDetailItem();
            obj.ID = id;
            obj.Amount = amount;
            obj.StockID = listStockService.GetIdByName(stock_Name);
            obj.Stock_Name = stock_Name;
            if (field.Equals("Quantity"))
            {
                obj.Quantity = quantity;
            }
            if (field.Equals("UnitPrice"))
            {
                obj.UnitPrice = unitPrice;
            }
            StockBuildDetailSV.UpdateProduct(obj);
            X.GetCmp<Store>("stProductChoiceBuildDismantle").GetById(id).Commit();
            return this.Direct();
        }
        public ActionResult GetDataBuildDismantleByVouchers(StoreRequestParameters parameters, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year = "")
        {
            if (fromdate == null && todate == null && year.Equals(""))
            {
                year = DateTime.Now.Year.ToString();
                fromdate = new DateTime(DateTime.Now.Year, 1, 1);
                todate = new DateTime(DateTime.Now.Year, 12, 31);
            }
            List<StockBuildItem> lst = new List<StockBuildItem>();
            int total;
            lst = stock_BuildService.GetAllDismantle(parameters.Page, parameters.Limit, out total, fromdate, todate, year);
            return this.Store(lst, total);
        }
        public ActionResult GetDataBuildDismantleByProduct(StoreRequestParameters parameters, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year = "")
        {
            if (fromdate == null && todate == null && year.Equals(""))
            {
                year = DateTime.Now.Year.ToString();
                fromdate = new DateTime(DateTime.Now.Year, 1, 1);
                todate = new DateTime(DateTime.Now.Year, 12, 31);
            }
            List<StockBuildDetailItem> lst = new List<StockBuildDetailItem>();
            int total;
            lst = stock_Build_DetailService.GetAllByBuildDismantle(parameters.Page, parameters.Limit, out total, fromdate, todate, year);
            return this.Store(lst, total);
        }
        public ActionResult GetDataStock()
        {
            List<StockItem> lst;
            lst = listStockService.GetAll();
            return this.Store(lst);
        }        
        public ActionResult GetRecordProduct(int id)
        {
            try
            {
                var obj = productService.GetById(id);
                return this.Direct(obj);
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        public ActionResult LoadCboProduct()
        {
            
            var lstProduct = productService.GetListProductByType();
            return this.Store(lstProduct);
        }
        public ActionResult Insert(StockBuildItem obj, Nullable<decimal> discount, Nullable<decimal> fAmount, Nullable<decimal> amount, int quantity, int store_Stock, int product_build, string unit, Nullable<decimal> unitprice)
        {
            try
            {
                
                var data = StockBuildDetailSV.Storage;
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
                else if (!obj.Barcode.Equals("") && !stock_BuildService.CheckBarcodeExits(obj.Barcode))
                {
                    foreach (var items in data)
                    {
                        if (items.Stock_Name == null)
                        {
                            X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.WARNING,
                                Title = "Thông báo",
                                Message = "Vật tư hàng hóa : " + items.ProductName + "<br/> Chưa biết lấy từ kho nào!"
                            });
                            return this.Direct();
                        }                       
                        decimal curQuatity = inventoryService.GetCurrentQuatity(product_build, store_Stock);
                        if (quantity > curQuatity)
                        {

                            X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.WARNING,
                                Title = "Thông báo",
                                Message = "Hàng hóa : <b>" + productService.GetNameByProductId(product_build) + "</b><br/> Trong kho <b>" + listStockService.GetNameStockById(store_Stock) + "</b> còn: <b>" + float.Parse(curQuatity.ToString()) + "</b><br/>Không đủ để tháo dỡ!"
                            });
                            return this.Direct();
                        }
                        
                    }
                    StockBuildItem objNew = new StockBuildItem();
                    objNew.Amount = amount;
                    objNew.FAmount = fAmount;
                    objNew.Discount = discount;
                    objNew.Barcode = obj.Barcode;
                    objNew.CreateAt = DateTime.Now;
                    objNew.CreateBy = User.ID;
                    objNew.Description = obj.Description;
                    objNew.RefDate = obj.RefDate;
                    objNew.RefType = (int)Common.RefType.build_Dismantle;
                    objNew.ExchangeRate = 1;
                    objNew.Vat = 0;
                    objNew.Employee_ID = User.EmployeeID;
                    objNew.Discount = discount;
                    objNew.Charge = 0;
                    int stock_BuildID = stock_BuildService.Insert(objNew);
                    List<StockBuildDetailItem> lst = new List<StockBuildDetailItem>();
                    foreach (var item in data)
                    {
                        lst.Add(new StockBuildDetailItem()
                        {
                            RefType = (int)Common.RefType.build_Dismantle,
                            StockProductID = item.StockProductID,
                            Unit = item.Unit,
                            UnitConvert = 1,
                            Quantity = item.Quantity != null ? item.Quantity : 0,
                            UnitPrice = item.UnitPrice != null ? item.UnitPrice : 0,
                            Amount = item.Quantity == 0 ? 0 : item.UnitPrice * item.Quantity,
                            QtyConvert = item.Amount,
                            Description = User.Name,
                            CreateAt = DateTime.Now,
                            CreateBy = User.ID,
                            Build_ID = stock_BuildID,
                            Charge = 0,
                            CurrentQty = 1,
                            RefTypeSub = (int)Common.RefType.inward,
                            Discount = 0,
                            Limit = DateTime.Now,
                            ProductName = item.ProductName,
                            QuantityDefault = item.QuantityDefault,
                            StockID = item.StockID,
                            Vat = 0
                       });
                    }
                    lst.Add(new StockBuildDetailItem()
                    {
                    RefType = (int)Common.RefType.build_Dismantle,
                        StockProductID = product_build,
                        Unit = unit,
                        UnitConvert = 1,
                        UnitPrice = unitprice,
                        Quantity = quantity,
                        Amount = quantity * unitprice,
                        QtyConvert = quantity * unitprice,
                        Description = User.Name,
                        CreateAt = DateTime.Now,
                        CreateBy = User.ID,
                        Build_ID = stock_BuildID,
                        Charge = 0,
                        CurrentQty = 1,
                        RefTypeSub = (int)Common.RefType.outward,
                        Discount = 0,
                        Limit = DateTime.Now,
                        ProductName = productService.GetNameByProductId(product_build),
                        QuantityDefault = 1,
                        StockID = store_Stock,
                        Vat = 0

                    });
                    inventoryService.Insert(lst, obj.Barcode);
                    inventory_DetailService.Insert(lst, obj.Barcode);
                    stock_Build_DetailService.Insert(lst);
                    trans_RefService.InsertBuild(stock_BuildID);
                    StockBuildDetailSV.Clear();
                    X.GetCmp<FormPanel>("frStock_BuildDismantle").Reset();
                    X.GetCmp<FormPanel>("FrProductBuildDismantle").Reset();
                    X.GetCmp<Store>("stProductChoiceBuildDismantle").Reload();
                    X.GetCmp<NumberField>("txtDiscount").Reset();
                    X.GetCmp<NumberField>("txtFAmount").Reset();
                    string CodeAuto = Common.NextID(stock_BuildService.GetMaxCode(), "TD");
                    X.GetCmp<TextField>("txtCode").SetValue(CodeAuto);
                    X.Msg.Notify("Thông báo", "Bạn đã tạo một phiếu tháo dỡ thành công!").Show();
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
            catch
            {
                return this.Direct("Error");
            }
        }
        public ActionResult ClearData()
        {
            StockBuildDetailSV.Clear();
            X.GetCmp<Store>("stProductChoiceBuildDismantle").Reload();
            return this.Direct();
        }
        public ActionResult GetData(Nullable<int> StockProductID=0, Nullable<int> quantity=1)
        {
            var datas = StockBuildDetailSV.Storage;
            if (datas.Count() == 0)
            {
                
                List<StockBuildDetailItem> lstdata = new List<StockBuildDetailItem>();
                var data = product_BuildService.GetAll(StockProductID);
                foreach (var item in data)
                {
                    lstdata.Add(new StockBuildDetailItem
                    {
                        ID = item.ID,
                        StockProductID = item.StockProductID,
                        Product_Code = item.Code,
                        ProductName = item.Product_Name,
                        QuantityDefault = item.Quantity,
                        Group_Name = item.Group_Name,
                        UnitPrice = 0,
                        Unit = item.Unit,
                        Quantity = item.Quantity * quantity,
                        Amount = item.Amount * quantity,
                        Build_ID = item.Build_ID

                    });
                }
                List<StockBuildDetailItem> lst = new List<StockBuildDetailItem>();
                lst.AddRange(lstdata);
                StockBuildDetailSV.AddProduct(lst);
            }
            if (datas.Count() > 0)
            {
                StockBuildDetailItem obj = new StockBuildDetailItem();
                foreach (var item in datas)
                {
                    obj.ID = item.ID;
                    obj.Amount = item.Amount * quantity;
                    obj.Quantity = item.QuantityDefault * quantity;
                    obj.UnitPrice = item.UnitPrice;
                    obj.StockID = item.StockID;
                    obj.Stock_Name = listStockService.GetNameStockById(item.StockID.HasValue ? item.StockID.Value : 0);
                    StockBuildDetailSV.UpdateProduct(obj);
                    X.GetCmp<Store>("stProductChoiceBuildDismantle").GetById(item.ID).Commit();
                }
            }
            return this.Store(datas);
        }
        public ActionResult LoadUser(StoreRequestParameters parameters, string query)
        {
            int totalCount;
            var data = new HumanEmployeeSV().Combobox(parameters.Page, parameters.Limit, out totalCount, query);
            return this.Store(data, totalCount);
        }
        public ActionResult ResetGrid()
        {
            StockBuildDetailSV.Clear();
            X.GetCmp<Store>("stProductChoiceBuildDismantle").Reload();
            X.GetCmp<NumberField>("txtDiscount").Reset();
            X.GetCmp<NumberField>("txtFAmount").Reset();
            string CodeAuto = Common.NextID(stock_BuildService.GetMaxCode(), "TD");
            X.GetCmp<TextField>("txtCode").SetValue(CodeAuto);
            return this.Direct();
        }
        public ActionResult DeleteProductInStorage(string stringId = "0")
        {
            string[] arrayId = stringId.Split(',');
            foreach (string id in arrayId)
            {
                StockBuildDetailSV.DeleteProduct(int.Parse(id));
            }
            X.GetCmp<Store>("stProductChoiceBuildDismantle").Reload();
            return this.Direct();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web.Areas.Stock;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.Services;
using iDAS.Items;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Chuyển kho", Icon = "LorryGo", IsActive = true, IsShow = true, Parent = AddinController.Code, Position = 1)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class Stock_TransferController : BaseController
    {
        private StockTransferSV stock_TransferService = new StockTransferSV();
        private StockProductGroupSV product_GroupService = new StockProductGroupSV();
        private StockTransferDetailSV stock_Transfer_DetailService = new StockTransferDetailSV();
        private StockSV listStockService = new StockSV();
        private InventoryDetailSV inventory_DetailService = new InventoryDetailSV();
        private InventorySV inventoryService = new InventorySV();
        private TransRefSV trans_RefService = new TransRefSV();
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            StockTransferDetailSV.Clear();
            string CodeAuto = Common.NextID(stock_TransferService.GetMaxCode(), "CK");
            ViewBag.CodeAuto = CodeAuto;
            return View();
        }
        public ActionResult FrmPrintTransfer(int stock_TransferID)
        {
            CenterCustomerSV systemCustomerService = new CenterCustomerSV();
            var objcus = systemCustomerService.GetCustomerByCode(User.Code);
            var datareport = stock_Transfer_DetailService.GetForReportTransfer((int)stock_TransferID, systemCustomerService.GetCustomerNameByCode(User.Code), objcus.FileID);
            return View(datareport);
        }
        public DirectResult HandleChanges(int id, string field, string oldValue, string newValue, string outStock_Name, string inStock_Name, Nullable<decimal> quantity, Nullable<decimal> unitPrice, Nullable<decimal> amount, object product)
        {
           
            StockTransferDetailItem obj = new StockTransferDetailItem();            
            if (inStock_Name.Equals(outStock_Name))
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Kho đi phải khác kho tới!"
                });
                X.GetCmp<Store>("stProductChoiceTransfer").Reload();
            }
            else
            {
                obj.ID = id;
                obj.Amount = amount;
                if (field.Equals("Quantity"))
                {
                    obj.Quantity = quantity;
                }
                if (field.Equals("UnitPrice"))
                {
                    obj.UnitPrice = unitPrice;
                }
                obj.InStock = listStockService.GetIdByName(inStock_Name);
                obj.InStock_Name = inStock_Name;
                StockTransferDetailSV.UpdateProduct(obj);
                X.GetCmp<Store>("stProductChoiceTransfer").GetById(id).Commit();
            }
           
          
            return this.Direct();
        }
        public ActionResult GetDataTransferByVouchers(StoreRequestParameters parameters, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year = "")
        {
            if (fromdate == null && todate == null && year.Equals(""))
            {
                year = DateTime.Now.Year.ToString();
                fromdate = new DateTime(DateTime.Now.Year, 1, 1);
                todate = new DateTime(DateTime.Now.Year, 12, 31);
            }
            
            List<StockTransferItem> lst = new List<StockTransferItem>();
            int total;
            lst = stock_TransferService.GetAll(parameters.Page, parameters.Limit, out total, fromdate, todate, year);
            return this.Store(lst, total);
        }
        public ActionResult GetDataTransferByProduct(StoreRequestParameters parameters, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year = "")
        {
            if (fromdate == null && todate == null && year.Equals(""))
            {
                year = DateTime.Now.Year.ToString();
                fromdate = new DateTime(DateTime.Now.Year, 1, 1);
                todate = new DateTime(DateTime.Now.Year, 12, 31);
            }
            int total;
            var lst = stock_Transfer_DetailService.GetAllByTransfer(parameters.Page, parameters.Limit, out total, fromdate, todate, year);
            return this.Store(lst, total);
        }
        public ActionResult GetDataStock()
        {
           
            List<StockItem> lst;
            lst = listStockService.GetAll();
            return this.Store(lst);

        }
        public ActionResult GetDataOfProducts(string stringId = "0")
        {
            string[] arrayId = stringId.Split(',');
            var data2 = inventoryService.GetProductsByStockID(arrayId);
            X.GetCmp<Store>("stProducts").LoadData(data2);
            X.GetCmp<GridPanel>("gpProducts").Refresh();
            return this.Direct();
        }
        public ActionResult GetProductsByStock(StoreRequestParameters parameters, int stockId=0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<StockProductItem> lstData;
            lstData = inventoryService.GetProductsByStockID(filter, stockId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetProducts(Nullable<int> outStockID, string stringId = "0")
        {
            try
            {
               
                string[] arrayId = stringId.Split(',');
                var data1 = stock_Transfer_DetailService.GetListProductByStringId(arrayId, outStockID);
                List<StockTransferDetailItem> lst = new List<StockTransferDetailItem>();
                lst.AddRange(data1);
                StockTransferDetailSV.AddProduct(lst);
                X.GetCmp<Store>("stProductChoiceTransfer").LoadData(StockTransferDetailSV.Storage);
                X.GetCmp<Store>("stProductChoiceTransfer").Reload();
                X.GetCmp<Window>("frmProducts").Close();
                X.GetCmp<GridPanel>("gpProductChoiseTransfer").Refresh();
                return this.Direct();
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }

        }
        public ActionResult Insert(StockTransferItem obj, Nullable<decimal> discount, Nullable<decimal> fAmount, Nullable<decimal> amount, Nullable<Boolean> print)
        {
            try
            {
                var data = StockTransferDetailSV.Storage;
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
                else if (!obj.Barcode.Equals("") && !stock_TransferService.CheckBarcodeExits(obj.Barcode))
                {
                    foreach (var items in data)
                    {
                        decimal curQuatity = inventoryService.GetCurrentQuatity((int)items.StockProductID, (int)items.OutStock);
                        if (items.Quantity > curQuatity)
                        {

                            X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.WARNING,
                                Title = "Thông báo",
                                Message = "Vật tư : <b>" + items.Product_Name + "</b><br/> Trong kho <b>" + items.OutStock_Name + "</b> còn: <b>" + float.Parse(curQuatity.ToString()) + " " + items.Unit + "</b><br/>Bạn không được chuyển quá số lượng đó!"
                            });
                            return this.Direct();
                        }
                        else if (items.InStock==null || items.InStock==0)
                        {
                            X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.ERROR,
                                Title = "Thông báo",
                                Message = "Bạn phải chọn kho đến cho vật tư hàng hóa!"
                            });
                            return this.Direct();
                        }

                    }
                    var objNew = new StockTransferItem();
                    objNew.Active = true;
                    objNew.Amount = amount;
                    objNew.Barcode = obj.Barcode;
                    objNew.CreateAt = DateTime.Now;
                    objNew.CreateBy = User.ID;
                    objNew.Description = obj.Description;
                    objNew.RefDate = obj.RefDate;
                    obj.IsClose = false;
                    objNew.RefType = (int)Common.RefType.transfer;                    
                    objNew.Employee_ID = obj.Employee_ID;
                    objNew.FromStockID = "All";
                    objNew.ToStockID = "All";
                    objNew.Sender_ID = User.ID;
                    objNew.IsReview = true;
                    int stock_TransferID = stock_TransferService.Insert(objNew);
                    List<StockTransferDetailItem> lst = new List<StockTransferDetailItem>();
                    foreach (var item in data)
                    {
                        lst.Add(new StockTransferDetailItem()
                        {
                            Transfer_ID = stock_TransferID,
                            RefType = (int)Common.RefType.transfer,
                            StockProductID = item.StockProductID,
                            Product_Name = item.Product_Name,
                            OutStock = item.OutStock,
                            InStock = item.InStock,
                            Lev1 = 0,
                            Lev2 = 0,
                            Lev3 = 0,
                            Lev4 = 0,                           
                            Unit = item.Unit,
                            UnitConvert = 1,
                            Quantity = item.Quantity != null ? item.Quantity : 0,
                            UnitPrice = item.UnitPrice != null ? item.UnitPrice : 0,
                            Amount = item.Quantity == 0 ? 0 : item.UnitPrice * item.Quantity,
                            QtyConvert = item.Amount,
                            StoreID = 0,
                            Description = User.Name,
                            CreateAt = DateTime.Now,
                            CreateBy = User.ID
                        });
                    }
                    inventoryService.Insert(lst, obj.Barcode);
                    inventory_DetailService.Insert(lst, obj.Barcode);
                    stock_Transfer_DetailService.Insert(lst);
                    trans_RefService.InsertTransfer(stock_TransferID);
                    StockTransferDetailSV.Clear();
                    X.GetCmp<FormPanel>("frStock_Transfer").Reset();
                    X.GetCmp<Store>("stProductChoiceTransfer").Reload();
                    X.GetCmp<NumberField>("txtDiscount").Reset();
                    X.GetCmp<NumberField>("txtFAmount").Reset();
                    string CodeAuto = Common.NextID(stock_TransferService.GetMaxCode(), "CK");
                    X.GetCmp<TextField>("txtCode").SetValue(CodeAuto);
                    X.Msg.Notify("Thông báo", "Bạn đã tạo một phiếu chuyển kho thành công!").Show();
                    ResultInsert xxx = new ResultInsert();
                    xxx.Order_ID = stock_TransferID;
                    xxx.PrintActive = print;
                    return this.Direct(xxx);
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
            catch (Exception ex)
            {
                return this.Direct("Error");
            }
        }       
        public ActionResult GetData()
        {
            var data = StockTransferDetailSV.Storage;
            return this.Store(data);
        }
        public ActionResult ShowFrmFindProduct()
        {
            var data = product_GroupService.GetListAll();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "FrmFindProducts", Model = data };
        }
        public ActionResult LoadUser(StoreRequestParameters parameters, string query)
        {
            int totalCount ;
            var data = new HumanEmployeeSV().Combobox(parameters.Page, parameters.Limit, out totalCount, query);
            return this.Store(data, totalCount);
        }
        public ActionResult ResetGrid()
        {
            
            StockTransferDetailSV.Clear();
            X.GetCmp<Store>("stProductChoiceTransfer").Reload();
            X.GetCmp<NumberField>("txtDiscount").Reset();
            X.GetCmp<NumberField>("txtFAmount").Reset();
            string CodeAuto = Common.NextID(stock_TransferService.GetMaxCode(), "CK");
            X.GetCmp<TextField>("txtCode").SetValue(CodeAuto);
            return this.Direct();
        }
        public ActionResult DeleteProductInStorage(string stringId = "0")
        {
            string[] arrayId = stringId.Split(',');
            foreach (string id in arrayId)
            {
                StockTransferDetailSV.DeleteProduct(int.Parse(id));
            }
            X.GetCmp<Store>("stProductChoiceTransfer").Reload();
            return this.Direct();
        }

    }
}

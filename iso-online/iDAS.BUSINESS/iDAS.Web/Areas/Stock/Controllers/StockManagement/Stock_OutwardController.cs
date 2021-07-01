
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Web.Areas.Stock;
using System.Reflection;
using iDAS.Services;
using iDAS.DA;
using iDAS.Utilities;
using iDAS.Items;
using iDAS.Base;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Xuất kho", Icon = "DoorOut", IsActive = true, IsShow = true, Parent = GroupManageStockController.Code, Position = 3)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class Stock_OutwardController : BaseController
    {
        public const string Code = "Stock_Outward";
        //
        // GET: /Stock/Stock_Outward/      
        private CustomerSV customerSV = new CustomerSV();
        private StockSV listStockService = new StockSV();
        private StockProductGroupSV product_GroupService = new StockProductGroupSV();
        private StockOutwardSV stock_OutwardService = new StockOutwardSV();
        private StockProductSV productService = new StockProductSV();
        private InventorySV inventoryService = new InventorySV();
        private InventoryDetailSV inventory_DetailService = new InventoryDetailSV();
        private StockOutwardDetailSV stock_Outward_DetailService = new StockOutwardDetailSV();
        private TransRefSV trans_RefService = new TransRefSV();
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            StockOutwardDetailSV.Clear();
            string CodeAuto = Common.NextID(stock_OutwardService.GetMaxCode(), "XK");
            ViewBag.CodeAuto = CodeAuto;
            return View();
        }
        public ActionResult FrmPrintOutward(int stock_OutwardID)
        {
            CenterCustomerSV systemCustomerService = new CenterCustomerSV();
            var objcus = systemCustomerService.GetCustomerByCode(User.Code);
            stock_Outward_DetailService = new StockOutwardDetailSV();
            var datareport = stock_Outward_DetailService.GetForReportOutward((int)stock_OutwardID, systemCustomerService.GetCustomerNameByCode(User.Code));
            return View(datareport);
        }
        public ActionResult GetDataOutwardByVouchers(StoreRequestParameters parameters, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year = "")
        {
            if (fromdate == null && todate == null && year.Equals(""))
            {
                year = DateTime.Now.Year.ToString();
                fromdate = new DateTime(DateTime.Now.Year, 1, 1);
                todate = new DateTime(DateTime.Now.Year, 12, 31);
            }
            List<StockOutwardItem> lst = new List<StockOutwardItem>();
            int total;
            lst = stock_OutwardService.GetAll(parameters.Page, parameters.Limit, out total, fromdate, todate, year);
            return this.Store(lst, total);
        }
        public ActionResult GetDataOutwardByProduct(StoreRequestParameters parameters, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year = "")
        {
            if (fromdate == null && todate == null && year.Equals(""))
            {
                year = DateTime.Now.Year.ToString();
                fromdate = new DateTime(DateTime.Now.Year, 1, 1);
                todate = new DateTime(DateTime.Now.Year, 12, 31);
            }
            List<StockOutwardDetailItem> lst = new List<StockOutwardDetailItem>();
            int total;
            lst = stock_Outward_DetailService.GetAll(parameters.Page, parameters.Limit, out total, fromdate, todate, year);
            return this.Store(lst, total);
        }
        public DirectResult HandleChanges(int id, string field, string oldValue, string newValue, string stock_Name, Nullable<decimal> quantity, Nullable<decimal> unitPrice, Nullable<decimal> amount, object product)
        {
            StockOutwardDetailItem obj = new StockOutwardDetailItem();
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
            StockOutwardDetailSV.UpdateProduct(obj);
            X.GetCmp<Store>("stProductChoice").GetById(id).Commit();
            return this.Direct();
        }
        public ActionResult DeleteProductInStorage(string stringId = "0")
        {
            string[] arrayId = stringId.Split(',');
            foreach (string id in arrayId)
            {
                StockOutwardDetailSV.DeleteProduct(int.Parse(id));
            }
            X.GetCmp<Store>("stProductChoice").Reload();
            return this.Direct();
        }
        public ActionResult LoadCustomer(StoreRequestParameters parameters, string query, bool isInside)
        {
            int total;
            var data = new List<ComboboxItem>();
            if (!isInside)
            {
                data = customerSV.Combobox(parameters.Page, parameters.Limit, out total, query);
            }
            else
            {
                data = customerSV.ComboboxDepartment(parameters.Page, parameters.Limit, out total, query);
            }
            return this.Store(data, total);
        }
        public ActionResult LoadStock(int start, int limit, int page, string query)
        {
            if (string.IsNullOrEmpty(query))
                query = "";
            List<ComboboxItem> lstStock = listStockService.Combobox(query);
            Paging<ComboboxItem> plants = new Paging<ComboboxItem>(lstStock, lstStock.Count);
            return this.Store(plants.Data, plants.TotalRecords);
        }
        public ActionResult GetData()
        {
            var data = StockOutwardDetailSV.Storage;
            return this.Store(data);
        }
        public ActionResult ResetGrid()
        {
            StockOutwardDetailSV.Clear();
            X.GetCmp<Store>("stProductChoice").Reload();
            X.GetCmp<NumberField>("txtDiscount").Reset();
            X.GetCmp<NumberField>("txtFAmount").Reset();
            string CodeAuto = Common.NextID(stock_OutwardService.GetMaxCode(), "XK");
            X.GetCmp<TextField>("txtCode").SetValue(CodeAuto);
            return this.Direct();
        }
        public ActionResult GetDataProductGroup()
        {
            List<StockProductGroupItem> lst;
            lst = product_GroupService.GetListAll();
            return this.Store(lst);

        }
        public ActionResult GetDataOfProducts(StoreRequestParameters parameters, string stringId = "0")
        {
            string[] arrayId = stringId.Split(',');
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<StockProductItem> lstData;
            lstData = productService.GetProductsByGroup(filter, arrayId);
            return this.Store(lstData, filter.PageTotal);
        }      
        public ActionResult GetProducts(string stringId = "0")
        {
            try
            {
                string[] arrayId = stringId.Split(',');
                var data = stock_Outward_DetailService.GetListProductByStringId(arrayId);
                List<StockOutwardDetailItem> lst = new List<StockOutwardDetailItem>();
                lst.AddRange(data);
                StockOutwardDetailSV.AddProduct(lst);
                X.GetCmp<Store>("stProductChoice").LoadData(StockOutwardDetailSV.Storage);
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
        public ActionResult ShowFrmFindProduct()
        {
            var data = product_GroupService.GetListAll();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "FrmFindProducts", Model = data };
        }
        public ActionResult GetRecordCustomer(int id, bool isInside)
        {
            try
            {
                var obj = new CustomerItem();
                if (!isInside)
                {
                    obj = customerSV.GetById(id);
                }
                else
                {
                    obj = customerSV.GetDepartmentById(id);
                }
                return this.Direct(obj);
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }
        }
        public ActionResult Insert(StockOutwardItem obj, Nullable<decimal> discount, Nullable<decimal> fAmount, Nullable<decimal> amount, Nullable<Boolean> print)
        {
            try
            {
                var data = StockOutwardDetailSV.Storage;
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
                            Message = "Kho xuất không được để trống!"
                        });
                        return this.Direct();
                    }
                    else if (!obj.Barcode.Equals("") && !stock_OutwardService.CheckBarcodeExits(obj.Barcode))
                    {
                        foreach (var items in data)
                        {
                            decimal curQuatity = inventoryService.GetCurrentQuatity((int)items.StockProductID, (int)items.StockID);
                            if (items.Quantity > curQuatity)
                            {

                                X.Msg.Show(new MessageBoxConfig
                                {
                                    Buttons = MessageBox.Button.OK,
                                    Icon = MessageBox.Icon.WARNING,
                                    Title = "Thông báo",
                                    Message = "Vật tư : <b>" + items.ProductName + "</b><br/> Trong kho <b>" + items.Stock_Name + "</b> còn: <b>" + float.Parse(curQuatity.ToString()) + " " + items.Unit + "</b><br/>Bạn không được xuất quá số lượng đó!"
                                });
                                return this.Direct();
                            }
                        }
                        StockOutward objNew = new StockOutward();
                        objNew.Active = true;
                        objNew.Amount = amount;
                        objNew.Barcode = obj.Barcode;
                        objNew.Contact = obj.Contact;
                        objNew.Discount = discount;
                        objNew.FAmount = fAmount;
                        objNew.CreateAt = DateTime.Now;
                        objNew.CreateBy = User.ID;
                        if (obj.IsInside)
                        {
                            objNew.Department_ID = obj.Customer_ID;
                        }
                        else
                        {
                            objNew.Customer_ID = obj.Customer_ID;
                        }
                        objNew.CustomerAddress = obj.CustomerAddress;
                        objNew.CustomerName = obj.CustomerName;
                        objNew.Description = obj.Description;
                        objNew.Reason = obj.Reason;
                        objNew.IsInside = obj.IsInside;
                        objNew.Employee_ID = User.ID;
                        objNew.RefDate = obj.RefDate;
                        obj.IsClose = false;
                        objNew.Sorted = 2;
                        objNew.Profit = 0;
                        objNew.Charge = 0;
                        objNew.RefType = (int)Common.RefType.outward;
                        objNew.Cost = obj.Cost;
                        int stock_OutwardID = stock_OutwardService.Insert(objNew);
                        List<StockOutwardDetail> lst = new List<StockOutwardDetail>();
                        foreach (var item in data)
                        {
                            lst.Add(new StockOutwardDetail()
                            {
                                Active = true,
                                StockOutwardID = stock_OutwardID,
                                ProductName = item.ProductName,
                                StockProductID = item.StockProductID,
                                RefType = (int)Common.RefType.outward,
                                Stock_ID = item.StockID,
                                Quantity = item.Quantity != null ? item.Quantity : 0,
                                UnitPrice = item.UnitPrice != null ? item.UnitPrice : 0,
                                Profit = item.Profit,
                                Cost = item.UnitPrice * item.Quantity,
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
                        inventoryService.Insert(lst, obj.Barcode);
                        inventory_DetailService.Insert(lst, obj.Barcode, (int)obj.Customer_ID);
                        stock_Outward_DetailService.Insert(lst);
                        trans_RefService.InsertOutward(stock_OutwardID);
                        StockOutwardDetailSV.Clear();
                        X.GetCmp<FormPanel>("frStock_Outward").Reset();
                        X.GetCmp<Store>("stProductChoice").Reload();
                        X.GetCmp<NumberField>("txtDiscount").Reset();
                        X.GetCmp<NumberField>("txtFAmount").Reset();
                        string CodeAuto = Common.NextID(stock_OutwardService.GetMaxCode(), "XK");
                        X.GetCmp<TextField>("txtCode").SetValue(CodeAuto);
                        X.Msg.Notify("Thông báo", "Bạn đã nhập thành công một phiếu nhập!").Show();
                        ResultInsert xxx = new ResultInsert();
                        xxx.Order_ID = stock_OutwardID;
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

            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }
        }
        public ActionResult GetDataReportOutwardBySupplier(StoreRequestParameters parameters, bool isInside, Nullable<int> provider_Id, Nullable<DateTime> fromDate, Nullable<DateTime> toDate)
        {
            if (provider_Id == null)
            {
                provider_Id = 1;
            }
            if (fromDate == null && toDate == null)
            {
                fromDate = DateTime.Now;
                toDate = DateTime.Now;
            }
            List<StockOutwardDetailItem> lst = new List<StockOutwardDetailItem>();
            int total;
            lst = stock_OutwardService.GetForReport(parameters.Page, parameters.Limit, out total, isInside, provider_Id, fromDate, toDate);
            return this.Store(lst, total);
        }
        public ActionResult GetDataReportOutwardByProduct(StoreRequestParameters parameters, Nullable<int> StockProductID, Nullable<DateTime> fromDate, Nullable<DateTime> toDate)
        {
            if (StockProductID == null)
            {
                StockProductID = 1;
            }
            if (fromDate == null && toDate == null)
            {
                fromDate = DateTime.Now;
                toDate = DateTime.Now;
            }
            List<StockOutwardDetailItem> lst = new List<StockOutwardDetailItem>();
            int total;
            lst = stock_OutwardService.GetForReportForProduct(parameters.Page, parameters.Limit, out total, StockProductID, fromDate, toDate);
            return this.Store(lst, total);
        }
        public ActionResult LoadProduct(int start, int limit, int page, string query)
        {

            if (string.IsNullOrEmpty(query))
                query = "";
            List<ComboboxItem> lstProduct = productService.Combobox(query);
            Paging<ComboboxItem> plants = new Paging<ComboboxItem>(lstProduct, lstProduct.Count);
            return this.Store(plants.Data, plants.TotalRecords);
        }
        
    }
}

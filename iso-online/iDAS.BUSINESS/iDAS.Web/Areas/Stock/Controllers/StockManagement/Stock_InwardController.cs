using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web.Areas.Stock;
using System.Reflection;
using iDAS.DA;
using iDAS.Utilities;
using iDAS.Services;
using iDAS.Items;
using iDAS.Base;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Nhập kho", Icon = "DoorIn", IsActive = true, IsShow = true, Parent = GroupManageStockController.Code, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class Stock_InwardController : BaseController
    {
        public const string Code = "Stock_Inward";
        //
        // GET: /Stock/Stock_Inward/
        private SupplierSV supplierService = new SupplierSV();
        private TransRefSV trans_RefService = new TransRefSV();
        private StockSV listStockService = new StockSV();
        private StockProductGroupSV product_GroupService = new StockProductGroupSV();
        private StockInwardSV stock_InwardService = new StockInwardSV();
        private StockProductSV productService = new StockProductSV();
        private StockInwardDetailSV stock_Inward_DetailService = new StockInwardDetailSV();
        private InventorySV inventoryService = new InventorySV();
        private InventoryDetailSV inventory_DetailService = new InventoryDetailSV();
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            StockInwardDetailSV.Clear();
            string CodeAuto = Common.NextID(stock_InwardService.GetMaxCode(), "NK");
            ViewBag.CodeAuto = CodeAuto;
            return View();
        }
        public ActionResult FrmPrintInward(int stock_InwardID)
        {
            CenterCustomerSV systemCustomerService = new CenterCustomerSV();
            var objcus = systemCustomerService.GetCustomerByCode(User.Code);
            var datareport = stock_Inward_DetailService.GetForReportInward((int)stock_InwardID, objcus.Company, objcus.FileID);
            return View(datareport);
        }
        public ActionResult GetDataInwardByVouchers(StoreRequestParameters parameters, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year = "")
        {
            if (fromdate == null && todate == null && year.Equals(""))
            {
                year = DateTime.Now.Year.ToString();
                fromdate = new DateTime(DateTime.Now.Year, 1, 1);
                todate = new DateTime(DateTime.Now.Year, 12, 31);
            }
            stock_InwardService = new StockInwardSV();
            List<StockInwardItem> lst = new List<StockInwardItem>();
            int total;
            lst = stock_InwardService.GetAll(parameters.Page, parameters.Limit, out total, fromdate, todate, year);
            return this.Store(lst, total);
        }
        public ActionResult GetDataInwardByProduct(StoreRequestParameters parameters, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year = "")
        {
            if (fromdate == null && todate == null && year.Equals(""))
            {
                year = DateTime.Now.Year.ToString();
                fromdate = new DateTime(DateTime.Now.Year, 1, 1);
                todate = new DateTime(DateTime.Now.Year, 12, 31);
            }
            List<StockInwardDetailItem> lst = new List<StockInwardDetailItem>();
            int total;
            lst = stock_Inward_DetailService.GetAll(parameters.Page, parameters.Limit, out total, fromdate, todate, year);
            return this.Store(lst, total);
        }
        public DirectResult HandleChanges(int id, string field, string oldValue, string newValue, string stock_Name, Nullable<decimal> quantity, Nullable<decimal> unitPrice, Nullable<decimal> amount, object product)
        {
            StockInwardDetailItem obj = new StockInwardDetailItem();
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
            StockInwardDetailSV.UpdateProduct(obj);
            X.GetCmp<Store>("stProductChoice").GetById(id).Commit();
            return this.Direct();
        }
        public ActionResult DeleteProductInStorage(string stringId = "0")
        {
            string[] arrayId = stringId.Split(',');
            foreach (string id in arrayId)
            {
                StockInwardDetailSV.DeleteProduct(int.Parse(id));
            }
            X.GetCmp<Store>("stProductChoice").Reload();
            return this.Direct();
        }
        public ActionResult LoadSupplier(StoreRequestParameters parameters, string query, bool isInside)
        {
            int total;
            if (string.IsNullOrEmpty(query))
                query = "";
            List<ComboboxItem> lstSupplier = new List<ComboboxItem>();
            if (!isInside)
            {
                lstSupplier = supplierService.Combobox(parameters.Page, parameters.Limit, out total, query);
            }
            else
            {
                lstSupplier = supplierService.ComboboxDepartment(parameters.Page, parameters.Limit, out total, query);
            }
            return this.Store(lstSupplier, total);
        }
        public ActionResult LoadBill(int start, int limit, int page, string query, int supplierid = 0)
        {
            SupplierDA supplierDA = new SupplierDA();
            SuppliersOrderSV ordersService = new SuppliersOrderSV();
            if (string.IsNullOrEmpty(query))
                query = "";
            List<SuppliersOrderItem> lstOrder = ordersService.GetOrderListBySuppID(supplierid, query);
            Paging<SuppliersOrderItem> plants = new Paging<SuppliersOrderItem>(lstOrder, lstOrder.Count);
            return this.Store(plants.Data, plants.TotalRecords);

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
            var data = StockInwardDetailSV.Storage;
            return this.Store(data);
        }
        public ActionResult ResetGrid()
        {
            StockInwardDetailSV.Clear();
            X.GetCmp<Store>("stProductChoice").Reload();
            X.GetCmp<NumberField>("txtDiscount").Reset();
            X.GetCmp<NumberField>("txtFAmount").Reset();
            string CodeAuto = Common.NextID(stock_InwardService.GetMaxCode(), "NK");
            X.GetCmp<TextField>("txtCode").SetValue(CodeAuto);
            return this.Direct();
        }
        public ActionResult ResetGridStart()
        {
            StockInwardDetailSV.Clear();
            X.GetCmp<Store>("stProductChoice").Reload();
            X.GetCmp<NumberField>("txtDiscount").Reset();
            X.GetCmp<NumberField>("txtFAmount").Reset();
            string CodeAutoStart = Common.NextID(stock_InwardService.GetMaxCodeStart(), "DK");
            X.GetCmp<TextField>("txtCode").SetValue(CodeAutoStart);
            return this.Direct();
        }
        public ActionResult GetDataProductGroup()
        {
            List<StockProductGroupItem> lst;
            lst = product_GroupService.GetListAll();
            return this.Store(lst);

        }
        public ActionResult GetDataOfProductsStart(string stringId = "0")
        {
            string[] arrayId = stringId.Split(',');
            List<int> idpro = new List<int>();
            idpro = inventoryService.GetListProductIdInvalid();
            var data2 = productService.GetProductsByGroup_IdForStart(arrayId, idpro);
            X.GetCmp<Store>("stProducts").LoadData(data2);
            X.GetCmp<GridPanel>("gpProducts").Refresh();
            return this.Direct();
        }
        public ActionResult GetDataOfProducts(StoreRequestParameters parameters, string stringId = "0")
        {
            string[] arrayId = stringId.Split(',');
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<StockProductItem> lstData;
            lstData = productService.GetProductsByGroup(filter, arrayId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetDataOfProducts_supplier(string stringId = "0")
        {
            string[] arrayId = stringId.Split(',');
            var data2 = productService.GetProductsByGroup_Id(arrayId);
            X.GetCmp<Store>("stProducts").LoadData(data2);
            X.GetCmp<GridPanel>("gpProducts").Refresh();
            return this.Direct();
        }
        public ActionResult GetProducts(string stringId = "0")
        {
            try
            {
                string[] arrayId = stringId.Split(',');
                var data = stock_Inward_DetailService.GetListProductByStringId(arrayId);
                List<StockInwardDetailItem> lst = new List<StockInwardDetailItem>();
                lst.AddRange(data);
                StockInwardDetailSV.AddProduct(lst);
                X.GetCmp<Store>("stProductChoice").LoadData(StockInwardDetailSV.Storage);
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
        public ActionResult GetProductByBills(int id = 0)
        {
            try
            {
                StockInwardDetailSV.Clear();
                var data = stock_Inward_DetailService.GetListProductByBill(id);
                List<StockInwardDetailItem> lst = new List<StockInwardDetailItem>();
                lst.AddRange(data);
                StockInwardDetailSV.AddProduct(lst);
                X.GetCmp<Store>("stProductChoice").LoadData(StockInwardDetailSV.Storage);
                X.GetCmp<Store>("stProductChoice").Reload();
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
        public ActionResult GetRecordSupplier(int id, bool isInside)
        {
            try
            {
                var obj = new SupplierItem();
                if (!isInside)
                {
                    obj = supplierService.GetById(id);
                }
                else
                {
                    obj = supplierService.GetDepartmentById(id);
                }
                return this.Direct(obj);
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }
        }
        public ActionResult Insert(StockInwardItem obj, Nullable<decimal> discount, Nullable<decimal> fAmount, Nullable<decimal> amount, Nullable<Boolean> print)
        {
            try
            {
                var data = StockInwardDetailSV.Storage;
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
                    else if (!obj.Barcode.Equals("") && !stock_InwardService.CheckBarcodeExits(obj.Barcode))
                    {

                        obj.CreateBy = User.ID;
                        obj.Amount = amount;
                        obj.CustomerName = obj.CustomerName;
                        obj.Employee_ID = User.ID;                       
                        int stock_InwardID = stock_InwardService.Insert(obj, discount, fAmount);
                        if (!obj.IsInside)
                        {
                            supplierService.updateTotalImportValue((int)obj.Customer_ID, (double)fAmount);
                            supplierService.updateAddIsOwedOn((int)obj.Customer_ID, (double)fAmount);
                        }
                        List<StockInwardDetail> lst = new List<StockInwardDetail>();
                        foreach (var item in data)
                        {
                            lst.Add(new StockInwardDetail()
                            {
                                Active = true,
                                StockInwardID = stock_InwardID,
                                ProductName = item.ProductName,
                                StockProductID = item.StockProductID,
                                RefType = (int)Common.RefType.inward,
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
                        inventoryService.Insert(lst, obj.Barcode);
                        inventory_DetailService.Insert(lst, obj.Barcode, (int)obj.Customer_ID);
                        stock_Inward_DetailService.Insert(lst);
                        trans_RefService.InsertInward(stock_InwardID);
                        StockInwardDetailSV.Clear();
                        X.GetCmp<FormPanel>("frDetailStock_Inward").Reset();
                        X.GetCmp<Store>("stProductChoice").Reload();
                        X.GetCmp<NumberField>("txtDiscount").Reset();
                        X.GetCmp<NumberField>("txtFAmount").Reset();
                        string CodeAuto = Common.NextID(stock_InwardService.GetMaxCode(), "NK");
                        X.GetCmp<TextField>("txtCode").SetValue(CodeAuto);
                        X.Msg.Notify("Thông báo", "Bạn đã nhập thành công một phiếu nhập!").Show();
                        ResultInsert xxx = new ResultInsert();
                        xxx.Order_ID = stock_InwardID;
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

        // HungNM. Add Importing function for stock_inward management. 20200924.
        public ActionResult SelectImportFile()
        {
            var fileUploadField = X.GetCmp<FileUploadField>("FileImportField");
            var direction = Common.FileImportCustomer + Guid.NewGuid().ToString() + ".xlsx";
            if (fileUploadField.HasFile)
            {
                if (fileUploadField.PostedFile.ContentLength < 300 * 1024 + 1)
                {
                    if (!System.IO.File.Exists(Server.MapPath(direction)))
                    {
                        fileUploadField.PostedFile.SaveAs(Server.MapPath(direction));
                    }
                }
                else
                {
                    Ultility.CreateMessageBox(title: RequestMessage.Notify, message: "Chỉ cho phép dung lượng import tối đa là 300KB");
                    return this.Direct();
                }
            }
            else
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.NotFileUpload);
            }
            return this.Direct(direction);
        }
        // End.

    }
}

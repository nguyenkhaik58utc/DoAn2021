using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class StockInwardDetailSV
    {
        private StockInwardDetailDA stock_Inward_DetailDA = new StockInwardDetailDA();
        private SuppliersOrderSV ordersService = new SuppliersOrderSV();
        public List<StockInwardDetailItem> GetByStock_Inward_Id(int stock_Inward_Id)
        {
            var dbo = stock_Inward_DetailDA.Repository;
            List<StockInwardDetailItem> lst = new List<StockInwardDetailItem>();
            var data = stock_Inward_DetailDA.GetQuery(t => t.StockInwardID == stock_Inward_Id).OrderByDescending(t => t.ID).ToList();
            if (data != null && data.Count > 0)
            {
                int n = 1;
                foreach (var item in data)
                {
                    lst.Add(new StockInwardDetailItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        NumberOrder = n,
                        CreateAt = item.CreateAt,
                        Group_Name = item.StockProduct.StockProductGroup.Name,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Stock_Name = dbo.Stocks.FirstOrDefault(t => t.ID == (int)item.Stock_ID).Name,
                        Unit = item.Unit,
                        UnitPrice = item.UnitPrice,
                        Discount = item.Discount,
                        Vat = item.Vat
                    });
                    n++;
                }

            }
            return lst;
        }
        public List<InwardInfo> GetForReportInwardStart(string code, string namecreate, DateTime date, string des, decimal? amount, string name, Guid? fileId, List<StockInwardDetailItem> items)
        {
            var dbo = stock_Inward_DetailDA.Repository;
            byte[] logo = new CenterCustomerDA().Repository.Files.Where(i => i.ID == fileId).Select(i => i.Data).FirstOrDefault();
            string datere = String.Format("{0:dd/MM/yyyy}", DateTime.Now.ToShortDateString());
            var lst = new List<InwardInfo>();
                lst.Add(new InwardInfo
                {
                    Amount = amount,
                    CreateName = name,
                    Barcode = code,
                    Description = des,
                    Reason = des,
                    RefDate = String.Format("{0:dd/MM/yyyy}", date.ToShortDateString()),
                    CustomerLogo = logo,
                    CustomerName = name,
                    DateReport = datere

                });
                List<StockInwardDetailItem> lstDetail = new List<StockInwardDetailItem>();
                if (items != null && items.Count > 0)
                {
                    int n = 1;
                    foreach (var item in items)
                    {
                        lstDetail.Add(new StockInwardDetailItem
                        {
                            ID = item.ID,
                            Amount = item.Amount,
                            NumberOrder = n,
                            CreateAt = item.CreateAt,
                            Group_Name = item.Group_Name,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            Stock_Name = item.Stock_Name,
                            Unit = item.Unit,
                            UnitPrice = item.UnitPrice,
                            Discount = item.Discount,
                            Vat = item.Vat
                        });
                        n++;
                    }
                }
            if(lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    item.Items = lstDetail;
                }
            }
            return lst;
        }
        public List<InwardInfo> GetForReportInward(int stock_Inward_Id, string name, Guid? fileId)
        {
            var dbo = stock_Inward_DetailDA.Repository;
            byte[] logo = new CenterCustomerDA().Repository.Files.Where(i => i.ID == fileId).Select(i => i.Data).FirstOrDefault();
            string datere = String.Format("{0:dd/MM/yyyy}", DateTime.Now.ToShortDateString());
            var lst = new List<InwardInfo>();
            var lst1 = dbo.StockInwards.Where(t => t.ID == stock_Inward_Id).Select(t => new { t.ID, t.Amount, t.Barcode, t.FAmount, t.Discount, t.CustomerAddress, SupplierName = t.CustomerName, t.Description, t.Reason, t.Contact, t.Contract_ID, t.RefDate }).FirstOrDefault();
            if (lst1 != null)
            {
                lst.Add(new InwardInfo
                {
                    ID = lst1.ID,
                    Amount = lst1.Amount,
                    Discount = lst1.Discount,
                    FAmount = lst1.FAmount,
                    SupplierName = lst1.SupplierName,
                    SupplierAddress = lst1.CustomerAddress,
                    Barcode = lst1.Barcode,
                    Contact = lst1.Contact,
                    Description = lst1.Description,
                    Reason = lst1.Reason,
                    RefDate = String.Format("{0:dd/MM/yyyy}", lst1.RefDate.Value.ToShortDateString()),
                    ContractCode = ordersService.GetCodeById(lst1.Contract_ID.HasValue ? (int)lst1.Contract_ID : 0),
                    CustomerLogo = logo,
                    CustomerName = name,
                    DateReport = datere

                });

            }
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    item.Items = GetByStock_Inward_Id((int)stock_Inward_Id);
                }
            }
            return lst;
        }
        public void Insert(List<StockInwardDetail> lst)
        {
            stock_Inward_DetailDA.InsertRange(lst);
            stock_Inward_DetailDA.Save();
            foreach (var item in lst)
            {
                Trigger(item.Stock_ID, item.StockProductID, item.RefType.Value, item.Quantity, item.Amount);
            }
        }
        private void Trigger(int? stock_Id, int? product_ID, int reftype, decimal? quantity, decimal? amount)
        {
            var dbo = stock_Inward_DetailDA.Repository;
            decimal? quantityInventory = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == stock_Id).Select(t => t.Quantity).FirstOrDefault();
            decimal? amountInventory = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == stock_Id).Select(t => t.Amount).FirstOrDefault();
            var s = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == stock_Id).FirstOrDefault();
            if (reftype == 0)
            {
                s.Amount = amount;
                s.Quantity = quantity;
            }
            else
            {
                s.Amount = amount + amountInventory;
                s.Quantity = quantity + quantityInventory;
            }
            dbo.SaveChanges();
        }
        public List<StockInwardDetailItem> GetListProductByStringId(string[] strwhere)
        {

            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            var dbo = stock_Inward_DetailDA.Repository;
            List<StockInwardDetailItem> lst = new List<StockInwardDetailItem>();
            var lstProducts = dbo.StockProducts.Where(t => output.Contains((int)t.ID)).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                int n = 1;
                int rs = 0;
                if (StockInwardDetailSV.Storage.Count() > 0)
                {
                    rs = StockInwardDetailSV.Storage.LastOrDefault().ID;
                }
                foreach (var item in lstProducts)
                {
                    lst.Add(new StockInwardDetailItem
                    {
                        ID = n + rs,
                        StockProductID = item.ID,
                        ProductCode = item.Code,
                        ProductName = item.Name,
                        Quantity = 0,
                        StockID = item.StockID,
                        Group_Name = dbo.StockProductGroups.FirstOrDefault(t => t.ID == (int)item.StockProductGroupID) != null ? dbo.StockProductGroups.FirstOrDefault(t => t.ID == (int)item.StockProductGroupID).Name : string.Empty,
                        Unit = dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.Unit_ID) != null ? dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.Unit_ID).Name : string.Empty,
                        Stock_Name = dbo.Stocks.FirstOrDefault(t => t.ID == (int)item.StockID) != null ? dbo.Stocks.FirstOrDefault(t => t.ID == (int)item.StockID).Name : string.Empty,
                        UnitPrice = item.Org_Price
                    });
                    n++;
                }
            }
            return lst;
        }
        public List<StockInwardDetailItem> GetListProductByBill(int id=0)
        {

            var dbo = stock_Inward_DetailDA.Repository;
            List<StockInwardDetailItem> lst = new List<StockInwardDetailItem>();
            var lstProducts = dbo.SuppliersOrderDetails.Where(t =>t.SuppliersOrderID==id).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                int n = 1;
                int rs = 0;
                foreach (var item in lstProducts)
                {
                    lst.Add(new StockInwardDetailItem
                    {
                        ID = n + rs,
                        StockProductID = item.StocksProductID.Value,
                        ProductCode = item.StockProduct.Code,
                        ProductName = item.StockProduct.Name,
                        Quantity = item.ReciveQuantity,
                        StockID = item.StockProduct.StockID,
                        Group_Name = item.StockProduct.StockProductGroup.Name,
                        Unit = dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.StockProduct.Unit_ID) != null ? dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.StockProduct.Unit_ID).Name : string.Empty,
                        Stock_Name = item.StockProduct.Stock!=null?item.StockProduct.Stock.Name:string.Empty,
                        UnitPrice = item.StockProduct.Org_Price
                    });
                    n++;
                }
            }
            return lst;
        }
        public List<StockInwardDetailItem> GetListProductStartByStringId(string[] strwhere)
        {

            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            var dbo = stock_Inward_DetailDA.Repository;
            List<StockInwardDetailItem> lst = new List<StockInwardDetailItem>();
            var lstProducts = dbo.StockProducts.Where(t => output.Contains((int)t.ID)).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                int n = 1;
                int rs = 0;
                if (StockInwardDetailSV.StorageInward_Start.Count() > 0)
                {
                    rs = StockInwardDetailSV.StorageInward_Start.LastOrDefault().ID;
                }
                foreach (var item in lstProducts)
                {
                    lst.Add(new StockInwardDetailItem
                    {
                        ID = n + rs,
                        StockProductID = item.ID,
                        ProductCode = item.Code,
                        ProductName = item.Name,
                        Quantity = 0,
                        StockID = item.StockID,
                        Group_Name = dbo.StockProductGroups.FirstOrDefault(t => t.ID == (int)item.StockProductGroupID) != null ? dbo.StockProductGroups.FirstOrDefault(t => t.ID == (int)item.StockProductGroupID).Name : string.Empty,
                        Unit = dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.Unit_ID) != null ? dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.Unit_ID).Name : string.Empty,
                        Stock_Name = dbo.Stocks.FirstOrDefault(t => t.ID == (int)item.StockID) != null ? dbo.Stocks.FirstOrDefault(t => t.ID == (int)item.StockID).Name : string.Empty,
                        UnitPrice = item.Org_Price
                    });
                    n++;
                }
            }
            return lst;
        }
        public static void UpdateProductStart(StockInwardDetailItem product)
        {

            var products = StockInwardDetailSV.StorageInward_Start;
            StockInwardDetailItem updatingproduct = null;

            foreach (StockInwardDetailItem p in products)
            {
                if (p.ID == product.ID)
                {
                    updatingproduct = p;
                    break;
                }
            }

            if (updatingproduct == null)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm của phiếu nhập kho");
            }
            if (product.Quantity != null)
            {
                updatingproduct.Quantity = product.Quantity;
            }
            if (product.UnitPrice != null)
            {
                updatingproduct.UnitPrice = product.UnitPrice;
            }
            updatingproduct.StockID = product.StockID;
            updatingproduct.Stock_Name = product.Stock_Name;
            updatingproduct.Amount = product.Amount;
            StockInwardDetailSV.StorageInward_Start = products;

        }
        public static List<StockInwardDetailItem> StorageInward_Start
        {
            get
            {
                var productchoices = HttpContext.Current.Session["StockInwardDetailItemStart"];
                if (productchoices == null)
                {
                    productchoices = new List<StockInwardDetailItem>();
                    HttpContext.Current.Session["StockInwardDetailItemStart"] = productchoices;
                }

                return (List<StockInwardDetailItem>)productchoices;
            }
            set
            {
                HttpContext.Current.Session["StockInwardDetailItemStart"] = value;
            }
        }
        public static void ClearInward_Start()
        {
            StockInwardDetailSV.StorageInward_Start = null;
        }
        public static void AddProductInward_Start(List<StockInwardDetailItem> product)
        {
            var products = StockInwardDetailSV.StorageInward_Start;
            products.AddRange(product);
            StockInwardDetailSV.StorageInward_Start = products;
        }
        public static void DeleteProductInward_Start(int id)
        {

            var products = StockInwardDetailSV.StorageInward_Start;
            StockInwardDetailItem produt = null;

            foreach (StockInwardDetailItem p in products)
            {
                if (p.ID == id)
                {
                    produt = p;
                    break;
                }
            }

            if (produt == null)
            {
                throw new Exception("Không tìm thấy sản phẩm");
            }

            products.Remove(produt);

            StockInwardDetailSV.StorageInward_Start = products;

        }
        public static List<StockInwardDetailItem> Storage
        {
            get
            {
                var productchoices = HttpContext.Current.Session["StockInwardDetailItem"];
                if (productchoices == null)
                {
                    productchoices = new List<StockInwardDetailItem>();
                    HttpContext.Current.Session["StockInwardDetailItem"] = productchoices;
                }

                return (List<StockInwardDetailItem>)productchoices;
            }
            set
            {
                HttpContext.Current.Session["StockInwardDetailItem"] = value;
            }
        }
        public static void Clear()
        {
            StockInwardDetailSV.Storage = null;
        }
        public static void AddProduct(List<StockInwardDetailItem> product)
        {
            var products = StockInwardDetailSV.Storage;
            products.AddRange(product);
            StockInwardDetailSV.Storage = products;
        }
        public static void DeleteProduct(int id)
        {

            var products = StockInwardDetailSV.Storage;
            StockInwardDetailItem produt = null;
            foreach (StockInwardDetailItem p in products)
            {
                if (p.ID == id)
                {
                    produt = p;
                    break;
                }
            }
            if (produt == null)
            {
                throw new Exception("Không tìm thấy sản phẩm");
            }
            products.Remove(produt);
            StockInwardDetailSV.Storage = products;

        }
        public static void DeleteProductStart(int id)
        {

            var products = StockInwardDetailSV.StorageInward_Start;
            StockInwardDetailItem produt = null;
            foreach (StockInwardDetailItem p in products)
            {
                if (p.ID == id)
                {
                    produt = p;
                    break;
                }
            }
            if (produt == null)
            {
                throw new Exception("Không tìm thấy sản phẩm");
            }
            products.Remove(produt);
            StockInwardDetailSV.StorageInward_Start = products;

        }
        public List<StockInwardDetailItem> GetAll(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year)
        {
            var dbo = stock_Inward_DetailDA.Repository;
            if (year.Trim().Equals(""))
                year = DateTime.Now.Year.ToString();
            int y1 = int.Parse(year);
            List<StockInwardDetailItem> lst = new List<StockInwardDetailItem>();
            var fromDateForQr = Convert.ToDateTime(fromdate);
            var toDateQr = Convert.ToDateTime(todate).AddDays(1);
            var data = stock_Inward_DetailDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.StockInward.RefDate.Value.Year == y1
                && (t.StockInward.RefDate > fromDateForQr && t.StockInward.RefDate <= toDateQr)).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockInwardDetailItem
                    {
                        ID = item.ID,
                        Active = item.Active,
                        Amount = item.Amount,
                        Charge = item.Charge,
                        CreateAt = item.CreateAt,
                        StockProductID = item.StockProductID,
                        ProductCode = item.StockProduct.Code,
                        Batch = item.Batch,
                        ChassyNo = item.ChassyNo,
                        Color = item.Color,
                        CurrentQty = item.CurrentQty,
                        Inward_Code = item.StockInward.Barcode,
                        Group_Name = item.StockProduct.StockProductGroup.Name,
                        Height = item.Height,
                        IME = item.IME,
                        Inward_ID = item.StockInwardID,
                        Lev1 = item.Lev1,
                        Lev2 = item.Lev2,
                        Lev3 = item.Lev3,
                        Lev4 = item.Lev4,
                        Limit = item.Limit,
                        Orgin = item.Orgin,
                        ProductName = item.ProductName,
                        QtyConvert = item.QtyConvert,
                        Quantity = item.Quantity,
                        Serial = item.Serial,
                        Size = item.Size,
                        Stock_Name = dbo.Stocks.FirstOrDefault(t => t.ID == (int)item.Stock_ID).Name,
                        StoreID = item.Stock_ID,
                        Unit = item.Unit,
                        UnitConvert = item.UnitConvert,
                        UnitPrice = item.UnitPrice,
                        Width = item.Width,
                        CreateBy = item.CreateBy,
                        Description = item.Description,
                        Discount = item.Discount,
                        RefType = item.RefType,
                        Sorted = item.Sorted,
                        StockID = item.Stock_ID,
                        Vat = item.Vat,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy
                    });
                }

            }
            return lst;
        }
        public static void UpdateProduct(StockInwardDetailItem product)
        {
            var products = StockInwardDetailSV.Storage;
            StockInwardDetailItem updatingproduct = null;
            foreach (StockInwardDetailItem p in products)
            {
                if (p.ID == product.ID)
                {
                    updatingproduct = p;
                    break;
                }
            }
            if (updatingproduct == null)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm của phiếu nhập kho");
            }
            if (product.Quantity != null)
            {
                updatingproduct.Quantity = product.Quantity;
            }
            if (product.UnitPrice != null)
            {
                updatingproduct.UnitPrice = product.UnitPrice;
            }
            updatingproduct.StockID = product.StockID;
            updatingproduct.Stock_Name = product.Stock_Name;
            updatingproduct.Amount = product.Amount;
            StockInwardDetailSV.Storage = products;

        }
    }
}

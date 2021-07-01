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
    public class StockTransferDetailSV
    {
        private StockSV listStockService = new StockSV();
        private StockProductDA productDA = new StockProductDA(); 
        private StockTransferDA stock_TransferDA = new StockTransferDA();
        private StockDA listStockDA = new StockDA();
        private StockTransferDetailDA stock_Transfer_DetailDA = new StockTransferDetailDA();
        public decimal GetUnitPrice(int StockProductID, int StockID)
        {
            var dbo = stock_Transfer_DetailDA.Repository;
            var rs = dbo.StockInventories.Where(p => p.StockProductID == StockProductID && p.Stock_ID == StockID).FirstOrDefault();
            if (rs != null)
            {
                decimal unitprice = 0;
                if (rs.Quantity != 0 && rs.Quantity != null && rs.Amount != null)
                {
                    unitprice = Math.Round((decimal)(rs.Amount / rs.Quantity), 2);
                }
                return unitprice;
            }
            return 0;
        }
        public List<StockTransferDetailItem> GetByStock_Transfer_Id(int stock_Transfer_Id)
        {
            List<StockTransferDetailItem> lst = new List<StockTransferDetailItem>();
            var data = stock_Transfer_DetailDA.GetQuery(t => t.StockTransferID == stock_Transfer_Id).OrderByDescending(t => t.ID).ToList();
            if (data != null && data.Count > 0)
            {
                int n = 1;
                foreach (var item in data)
                {
                    lst.Add(new StockTransferDetailItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        NumberOrder = n,
                        CreateAt = item.CreateAt,
                        // Group_Name = item.stock_Product.genericCategory.Name,
                        Product_Name = item.Product_Name,
                        Quantity = item.Quantity,
                        InStock_Name = listStockService.GetNameStockById((int)item.InStock),
                        OutStock_Name = listStockService.GetNameStockById((int)item.OutStock),
                        Unit = item.Unit,
                        UnitPrice = item.UnitPrice
                    });
                    n++;
                }

            }
            return lst;
        }
        public List<TransferInfo> GetForReportTransfer(int stock_Transfer_Id, string name, Guid? fileId)
        {
            byte[] logo = new CenterCustomerDA().Repository.Files.Where(i => i.ID == fileId).Select(i => i.Data).FirstOrDefault();
            string datere = String.Format("{0:dd/MM/yyyy}", DateTime.Now.ToShortDateString());
            var lst = new List<TransferInfo>();
            var lst1 = stock_TransferDA.GetQuery(t => t.ID == stock_Transfer_Id).Select(t => new { t.ID, t.Amount, t.Barcode, t.Description, t.RefDate, t.Employee_ID }).FirstOrDefault();
            if (lst1 != null)
            {
                lst.Add(new TransferInfo
                {
                    ID = lst1.ID,
                    Amount = lst1.Amount,
                    Barcode = lst1.Barcode,
                    Description = lst1.Description,
                    FAmount = lst1.Amount,
                    ContractCode = "",
                    //   EmployeeName = userClientSV.GetFullNameById((int)lst1.Employee_ID),   
                    RefDate = String.Format("{0:dd/MM/yyyy}", lst1.RefDate.Value.ToShortDateString()),
                    CustomerLogo = logo,
                    CustomerName = name,
                    DateReport = datere

                });

            }
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    item.Items = GetByStock_Transfer_Id((int)stock_Transfer_Id);
                }
            }
            return lst;
        }       
        private string GetStockNameById(int id)
        {
            var rs = listStockDA.GetById(id);
            if (rs != null)
                return rs.Name;
            return "";
        }
        public void Insert(List<StockTransferDetail> lst)
        {
            stock_Transfer_DetailDA = new StockTransferDetailDA();
            stock_Transfer_DetailDA.InsertRange(lst);
            stock_Transfer_DetailDA.Save();
        }
        public void Insert(List<StockTransferDetailItem> lst)
        {
            List<StockTransferDetail> lstInsert = new List<StockTransferDetail>();
            foreach (var item in lst)
            {
                lstInsert.Add(new StockTransferDetail
                {
                    Amount = item.Amount,
                    Batch = item.Batch,
                    CreateAt = DateTime.Now,
                    CreateBy = item.CreateBy,
                    Description = item.Description,
                    InStock = item.InStock,
                    Lev1 = item.Lev1,
                    Lev2 = item.Lev2,
                    Lev4 = item.Lev4,
                    Lev3 = item.Lev3,
                    OutStock = item.OutStock,
                    StockProductID = item.StockProductID,
                    Product_Name = item.Product_Name,
                    QtyConvert = item.QtyConvert,
                    Quantity = item.Quantity,
                    RefType = item.RefType,
                    Serial = item.Serial,
                    Sorted = item.Sorted,
                    StoreID = item.StoreID,
                    StockTransferID = item.Transfer_ID,
                    Unit = item.Unit,
                    UnitConvert = item.UnitConvert,
                    UnitPrice = item.UnitPrice,
                });

            }
            stock_Transfer_DetailDA.InsertRange(lstInsert);
            stock_Transfer_DetailDA.Save();
            foreach (var item in lst)
            {
                Trigger(item.InStock, item.OutStock, item.StockProductID, item.Quantity, item.Amount);
            }
        }
        private void Trigger(int? instock_Id,int ? outStock_Id, int? product_ID,decimal? quantity, decimal? amount)
        {
            var dbo = stock_Transfer_DetailDA.Repository;
            decimal? quantityInventory = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == outStock_Id).Select(t => t.Quantity).FirstOrDefault();
            decimal? amountInventory = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == outStock_Id).Select(t => t.Amount).FirstOrDefault();
            decimal? quantityInventoryIn = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == instock_Id).Select(t => t.Quantity).FirstOrDefault();
            decimal? amountInventoryIn = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == instock_Id).Select(t => t.Amount).FirstOrDefault();
            var stockOut = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == outStock_Id).FirstOrDefault();
            stockOut.Amount = amountInventory - amount;
            stockOut.Quantity = quantityInventory - quantity;
            var stockIn = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == instock_Id).FirstOrDefault();
            stockIn.Amount = amountInventoryIn + amount;
            stockIn.Quantity = quantityInventoryIn + quantity;
            dbo.SaveChanges();
        }
        public List<StockTransferDetailItem> GetListProductByStringId(string[] strwhere, Nullable<int> outStockID=0)
        {
            var dbo = productDA.Repository;
            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            List<StockTransferDetailItem> lst = new List<StockTransferDetailItem>();
            var lstProducts = productDA.GetQuery().Where(t => output.Contains((int)t.ID)).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                int n = 1;
                int rs = 0;
                if (StockTransferDetailSV.Storage.Count() > 0)
                {
                    rs = StockTransferDetailSV.Storage.LastOrDefault().ID;
                }
                foreach (var item in lstProducts)
                {
                    lst.Add(new StockTransferDetailItem
                    {
                        ID = n + rs,
                        StockProductID = item.ID,
                        Product_Code = item.Code,
                        Product_Name = item.Name,
                        OutStock = outStockID,
                        OutStock_Name = listStockService.GetNameStockById((int)outStockID),
                        Group_Name = item.StockProductGroup.Name,
                        Quantity = 0,
                        Unit = dbo.StockUnits.FirstOrDefault(t => t.ID == item.Unit_ID).Name,
                        UnitPrice = GetUnitPrice((int)item.ID, outStockID.Value),
                    });
                    n++;
                }
            }
            return lst;
        }
        public static List<StockTransferDetailItem> Storage
        {
            get
            {
                var productchoicestransfer = HttpContext.Current.Session["StockTransferDetailItem"];
                if (productchoicestransfer == null)
                {
                    productchoicestransfer = new List<StockTransferDetailItem>();
                    HttpContext.Current.Session["StockTransferDetailItem"] = productchoicestransfer;
                }

                return (List<StockTransferDetailItem>)productchoicestransfer;
            }
            set
            {
                HttpContext.Current.Session["StockTransferDetailItem"] = value;
            }
        }
        public static void Clear()
        {
            StockTransferDetailSV.Storage = null;
        }
        public static void AddProduct(List<StockTransferDetailItem> product)
        {
            var products = StockTransferDetailSV.Storage;
            products.AddRange(product);
            StockTransferDetailSV.Storage = products;
        }
        public static void DeleteProduct(int id)
        {

            var products = StockTransferDetailSV.Storage;
            StockTransferDetailItem produt = null;

            foreach (StockTransferDetailItem p in products)
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

            StockTransferDetailSV.Storage = products;

        }
        public List<StockTransferDetailItem> GetAll(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year)
        {
            var dbo = stock_Transfer_DetailDA.Repository;
            if (year.Trim().Equals(""))
                year = DateTime.Now.Year.ToString();
            stock_Transfer_DetailDA = new StockTransferDetailDA();
            listStockService = new StockSV();
            int y1 = int.Parse(year);
            List<StockTransferDetailItem> lst = new List<StockTransferDetailItem>();
            var fromDateForQr = Convert.ToDateTime(fromdate);
            var toDateQr = Convert.ToDateTime(todate).AddDays(1);
            var data = stock_Transfer_DetailDA.GetQuery().OrderByDescending(t => t.ID).Where(t => dbo.StockTransfers.FirstOrDefault(s => s.ID == t.StockTransferID).RefDate.Value.Year == y1 && (dbo.StockTransfers.FirstOrDefault(s => s.ID == t.StockTransferID).RefDate > fromDateForQr && dbo.StockTransfers.FirstOrDefault(s => s.ID == t.StockTransferID).RefDate <= toDateQr)).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockTransferDetailItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        CreateAt = item.CreateAt,
                        StockProductID = item.StockProductID,
                        Batch = item.Batch,
                        Lev1 = item.Lev1,
                        Lev2 = item.Lev2,
                        Lev3 = item.Lev3,
                        Lev4 = item.Lev4,
                        QtyConvert = item.QtyConvert,
                        Quantity = item.Quantity,
                        Serial = item.Serial,
                        Unit = item.Unit,
                        UnitConvert = item.UnitConvert,
                        UnitPrice = item.UnitPrice,
                        CreateBy = item.CreateBy,
                        Description = item.Description,
                        RefType = item.RefType,
                        Sorted = item.Sorted,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        InStock = item.InStock,
                        OutStock = item.InStock,
                        Product_Name = item.Product_Name,
                        StoreID = item.StoreID,
                        Transfer_ID = item.StockTransferID.Value
                    });
                }
            }
            return lst;
        }
        public static void UpdateProduct(StockTransferDetailItem product)
        {
            var products = StockTransferDetailSV.Storage;
            StockTransferDetailItem updatingproduct = null;
            foreach (StockTransferDetailItem p in products)
            {
                if (p.ID == product.ID)
                {
                    updatingproduct = p;
                    break;
                }
            }
            if (updatingproduct == null)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm của phiếu chuyển kho");
            }
            if (product.Quantity != null)
            {
                updatingproduct.Quantity = product.Quantity;
            }
            if (product.UnitPrice != null)
            {
                updatingproduct.UnitPrice = product.UnitPrice;
            }
            updatingproduct.InStock = product.InStock;
            updatingproduct.Amount = product.Amount;
            updatingproduct.InStock_Name = product.InStock_Name;
            StockTransferDetailSV.Storage = products;
        }
        public List<StockTransferDetailItem> GetAllByTransfer(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year)
        {
            var dbo = stock_Transfer_DetailDA.Repository;
            if (year.Trim().Equals(""))
                year = DateTime.Now.Year.ToString();
            stock_Transfer_DetailDA = new StockTransferDetailDA();
            listStockService = new StockSV();
            int y1 = int.Parse(year);
            List<StockTransferDetailItem> lst = new List<StockTransferDetailItem>();
            var fromDateForQr = Convert.ToDateTime(fromdate);
            var toDateQr = Convert.ToDateTime(todate).AddDays(1);
            var data = stock_Transfer_DetailDA.GetQuery().OrderByDescending(t => t.ID)
                .Where(t => t.StockTransfer.RefDate.Value.Year == y1 && (t.StockTransfer.RefDate > fromDateForQr && t.StockTransfer.RefDate <= toDateQr)).Page(page, pageSize, out total).ToList(); 
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockTransferDetailItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        CreateAt = item.CreateAt,
                        StockProductID = item.StockProductID,
                        Product_Code = item.StockProduct.Code,
                        Batch = item.Batch,
                        Transfer_Code = dbo.StockTransfers.FirstOrDefault(s => s.ID == item.StockTransferID).Barcode,
                        Group_Name = item.StockProduct.StockProductGroup.Name,
                        Transfer_ID = item.StockTransferID.Value,
                        Lev1 = item.Lev1,
                        Lev2 = item.Lev2,
                        Lev3 = item.Lev3,
                        Lev4 = item.Lev4,
                        InStock = item.InStock,
                        OutStock = item.OutStock,
                        StoreID = item.StoreID,
                        Product_Name = item.Product_Name,
                        QtyConvert = item.QtyConvert,
                        Quantity = item.Quantity,
                        Serial = item.Serial,
                        OutStock_Name = listStockService.GetNameStockById((int)item.OutStock),
                        InStock_Name = listStockService.GetNameStockById((int)item.InStock),
                        Unit = item.Unit,
                        UnitConvert = item.UnitConvert,
                        UnitPrice = item.UnitPrice,
                        CreateBy = item.CreateBy,
                        Description = item.Description,
                        RefType = item.RefType,
                        Sorted = item.Sorted,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy
                    });
                }

            }
            return lst;
        }
    }
}

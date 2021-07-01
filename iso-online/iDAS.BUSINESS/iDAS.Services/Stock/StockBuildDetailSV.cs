using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.DA;
using iDAS.Items;
namespace iDAS.Services
{
    public class StockBuildDetailSV
    {
        private StockSV listStockService = new StockSV();
        private StockProductDA productDA = new StockProductDA();
        private StockDA listStockDA = new StockDA();
        private StockBuildDetailDA stock_Build_DetailDA = new StockBuildDetailDA();
        public static decimal GetUnitPrice(int StockProductID, int StockID)
        {
            StockBuildDetailDA stockBuildDetailDA = new StockBuildDetailDA();
            var dbo = stockBuildDetailDA.Repository;
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
        private string GetStockNameById(int id)
        {
            var rs = listStockDA.GetById(id);
            return rs.Name;
        }
        public void Insert(List<StockBuildDetailItem> lst)
        {
            var lstInsert = new List<StockBuildDetail>();
            foreach (var item in lst)
            {
                lstInsert.Add(new StockBuildDetail()
                    {
                        RefType = item.RefType,
                        StockProductID = item.StockProductID,
                        Unit = item.Unit,
                        UnitConvert = item.UnitConvert,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Amount = item.Amount,
                        QtyConvert = item.QtyConvert,
                        Description = item.Description,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        StockBuildID = item.Build_ID,
                        Charge = item.Charge,
                        CurrentQty = item.CurrentQty,
                        RefTypeSub = item.RefTypeSub,
                        Discount = item.Discount,
                        Limit = item.Limit,
                        ProductName = item.ProductName,
                        QuantityDefault = item.QuantityDefault,
                        Stock_ID = item.StockID,
                        Vat = item.Vat
                    });
            }
            Insert(lstInsert);
        }
        public void Insert(List<StockBuildDetail> lstInsert)
        {
            stock_Build_DetailDA.InsertRange(lstInsert);
            stock_Build_DetailDA.Save();
            foreach (var item in lstInsert)
            {
                Trigger(item.Stock_ID, item.StockProductID, item.RefType.Value, item.RefTypeSub, item.Quantity, item.Amount);
            }
        }
        private void Trigger(int? stock_Id, int? product_ID, int reftype, int? refTypeSub, decimal? quantity, decimal? amount)
        {
            var dbo = stock_Build_DetailDA.Repository;
            decimal? quantityInventory = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == stock_Id).Select(t => t.Quantity).FirstOrDefault();
            decimal? amountInventory = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == stock_Id).Select(t => t.Amount).FirstOrDefault();
            var s = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == stock_Id).FirstOrDefault();
            if (reftype == 7 && refTypeSub == 2)
            {
                s.Quantity = quantityInventory - quantity;
                s.Amount = amountInventory - amount;
            }
            else if (reftype == 7 && refTypeSub == 1)
            {
                s.Quantity = quantityInventory + quantity;
                s.Amount = amountInventory + amount;
            }
            else if (reftype == 8 && refTypeSub == 1)
            {
                s.Quantity = quantityInventory + quantity;
                s.Amount = amountInventory + amount;
            }

            else if (reftype == 8 && refTypeSub == 2)
            {
                s.Quantity = quantityInventory - quantity;
                s.Amount = amountInventory - amount;
            }
            dbo.SaveChanges();
        }
        public List<StockBuildDetailItem> GetListProductByStringId(string[] strwhere, Nullable<int> outStockID)
        {


            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            List<StockBuildDetailItem> lst = new List<StockBuildDetailItem>();
            var lstProducts = productDA.GetQuery().Where(t => output.Contains((int)t.ID)).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                int n = 1;
                int rs = 0;
                if (Storage.Count() > 0)
                {
                    rs = Storage.LastOrDefault().ID;
                }
                foreach (var item in lstProducts)
                {
                    lst.Add(new StockBuildDetailItem
                    {
                        ID = n + rs,
                        StockProductID = item.ID,
                        //Product_Code = item.Code,
                        //Product_Name = item.Name,
                        //OutStock = outStockID,
                        //OutStock_Name = listStockService.GetNameStockById((int)outStockID),
                        //Group_Name = item.genericCategory.Name,
                        Quantity = 0,
                        //   Unit = GetUnitNameById((int)item.Unit_ID),
                        UnitPrice = item.Sale_Price
                    });
                    n++;
                }
            }
            return lst;
        }
        public static List<StockBuildDetailItem> Storage
        {
            get
            {
                var productchoicestransfer = HttpContext.Current.Session["StockBuildDetailItem"];
                if (productchoicestransfer == null)
                {
                    productchoicestransfer = new List<StockBuildDetailItem>();
                    HttpContext.Current.Session["StockBuildDetailItem"] = productchoicestransfer;
                }

                return (List<StockBuildDetailItem>)productchoicestransfer;
            }
            set
            {
                HttpContext.Current.Session["StockBuildDetailItem"] = value;
            }
        }
        public static void Clear()
        {
            Storage = null;
        }
        public static void AddProduct(List<StockBuildDetailItem> product)
        {
            var products = Storage;
            products.AddRange(product);
            Storage = products;
        }
        public static void DeleteProduct(int id)
        {

            var products = Storage;
            StockBuildDetailItem produt = null;

            foreach (StockBuildDetailItem p in products)
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

            Storage = products;

        }
        //public List<StockBuildDetailItem> GetAll(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year)
        //{
        //    if (year.Trim().Equals(""))
        //        year = DateTime.Now.Year.ToString();
        //    
        //    
        //    int y1 = int.Parse(year);
        //    List<StockBuildDetailItem> lst = new List<StockBuildDetailItem>();
        //    var data = stock_Build_DetailDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.MnStockStock_Transfer.RefDate.Value.Year == y1 && (t.MnStockStock_Transfer.RefDate >= fromdate && t.MnStockStock_Transfer.RefDate <= todate)).Page(page, pageSize, out total).ToList();
        //    if (data != null && data.Count > 0)
        //    {
        //        foreach (var item in data)
        //        {
        //            lst.Add(new StockBuildDetailItem
        //            {
        //                ID = item.ID,
        //                Amount = item.Amount,
        //                CreateAt = item.CreateAt,
        //                StockProductID = item.StockProductID,

        //                QtyConvert = item.QtyConvert,
        //                Quantity = item.Quantity,
        //                Serial = item.Serial,
        //                Unit = item.Unit,
        //                UnitConvert = item.UnitConvert,
        //                UnitPrice = item.UnitPrice,
        //                CreateBy = item.CreateBy,
        //                Description = item.Description,
        //                RefType = item.RefType,
        //                Sorted = item.Sorted,
        //                UpdateAt = item.UpdateAt,
        //                UpdateBy = item.UpdateBy

        //            });
        //        }
        //    }
        //    return lst;
        //}
        public static void UpdateProduct(StockBuildDetailItem product)
        {
            int productId = 0;
            var products = Storage;
             StockBuildDetailItem updatingproduct = null;
            foreach (StockBuildDetailItem p in products)
            {
                if (p.ID == product.ID)
                {
                    updatingproduct = p;
                    productId = p.StockProductID;
                    break;
                }
            }
            if (updatingproduct == null)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm của phiếu đóng gói");
            }
            if (product.Quantity != null)
            {
                updatingproduct.Quantity = product.Quantity;
            }
            if (product.UnitPrice != null)
            {
                updatingproduct.UnitPrice = product.UnitPrice;
            }
            else
            {
                updatingproduct.UnitPrice = GetUnitPrice(productId, product.StockID.Value);
            }
            updatingproduct.StockID = product.StockID;
            updatingproduct.Stock_Name = product.Stock_Name;
            updatingproduct.Amount = product.Amount;
            Storage = products;
        }
        public List<StockBuildDetailItem> GetAllByBuild(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year)
        {
            var dbo = stock_Build_DetailDA.Repository;
            if (year.Trim().Equals(""))
                year = DateTime.Now.Year.ToString();
            int y1 = int.Parse(year);
            List<StockBuildDetailItem> lst = new List<StockBuildDetailItem>();
            var fromDateForQr = System.Convert.ToDateTime(fromdate);
            var toDateQr = System.Convert.ToDateTime(todate).AddDays(1);
            var data = stock_Build_DetailDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.StockBuild.RefDate.Value.Year == y1 && (t.StockBuild.RefDate > fromDateForQr && t.StockBuild.RefDate <= toDateQr) && t.RefType == (int)Common.RefType.build).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockBuildDetailItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        CreateAt = item.CreateAt,
                        StockProductID = item.StockProductID,
                        Build_ID = item.StockBuildID,
                        Charge = item.Charge,
                        CurrentQty = item.CurrentQty,
                        Discount = item.Discount,
                        Limit = item.Limit,
                        ProductName = item.ProductName,
                        QuantityDefault = item.QuantityDefault,
                        RefTypeSub = item.RefTypeSub,
                        StockID = item.Stock_ID,
                        Vat = item.Vat,
                        QtyConvert = item.QtyConvert,
                        Quantity = item.Quantity,
                        Serial = item.Serial,
                        Unit = item.Unit,
                        RefTypeSubName = item.RefTypeSub == (int)Common.RefType.inward ? "Nhập" : "Xuất",
                        UnitConvert = item.UnitConvert,
                        UnitPrice = item.UnitPrice,
                        CreateBy = item.CreateBy,
                        Description = item.Description,
                        RefType = item.RefType,
                        Sorted = item.Sorted,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        Build_Code = item.StockBuild.Barcode,
                        //  Group_Name = item.stock_Product.genericCategory.Name,
                        Product_Code = dbo.StockProducts.FirstOrDefault(t => t.ID == item.StockProductID).Code,
                        Stock_Name = listStockService.GetNameStockById((int)item.Stock_ID),
                    });
                }

            }
            return lst;
        }
        public List<StockBuildDetailItem> GetAllByBuildDismantle(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year)
        {
            var dbo = stock_Build_DetailDA.Repository;
            if (year.Trim().Equals(""))
                year = DateTime.Now.Year.ToString();
            int y1 = int.Parse(year);
            List<StockBuildDetailItem> lst = new List<StockBuildDetailItem>();
            var fromDateForQr = System.Convert.ToDateTime(fromdate);
            var toDateQr = System.Convert.ToDateTime(todate).AddDays(1);
            var data = stock_Build_DetailDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.StockBuild.RefDate.Value.Year == y1 && (t.StockBuild.RefDate > fromDateForQr && t.StockBuild.RefDate <= toDateQr) && t.RefType == (int)Common.RefType.build_Dismantle).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockBuildDetailItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        CreateAt = item.CreateAt,
                        StockProductID = item.StockProductID,
                        Build_ID = item.StockBuildID,
                        Charge = item.Charge,
                        CurrentQty = item.CurrentQty,
                        Discount = item.Discount,
                        Limit = item.Limit,
                        ProductName = item.ProductName,
                        QuantityDefault = item.QuantityDefault,
                        RefTypeSub = item.RefTypeSub,
                        StockID = item.Stock_ID,
                        Vat = item.Vat,
                        QtyConvert = item.QtyConvert,
                        Quantity = item.Quantity,
                        Serial = item.Serial,
                        Unit = item.Unit,
                        RefTypeSubName = item.RefTypeSub == (int)Common.RefType.inward ? "Nhập" : "Xuất",
                        UnitConvert = item.UnitConvert,
                        UnitPrice = item.UnitPrice,
                        CreateBy = item.CreateBy,
                        Description = item.Description,
                        RefType = item.RefType,
                        Sorted = item.Sorted,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        Build_Code = item.StockBuild.Barcode,
                        // Group_Name = item.stock_Product.genericCategory.Name,
                        Product_Code = dbo.StockProducts.FirstOrDefault(t => t.ID == item.StockProductID).Code,
                        Stock_Name = listStockService.GetNameStockById((int)item.Stock_ID),
                    });
                }

            }
            return lst;
        }
    }
}

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
    public class StockOutwardDetailSV
    {
        private StockOutwardDetailDA stock_Outward_DetailDA = new StockOutwardDetailDA();
        public static decimal GetUnitPrice(int StockProductID, int StockID)
        {
             StockOutwardDetailDA stock_OutwardDetailDA = new StockOutwardDetailDA();
             var dbo = stock_OutwardDetailDA.Repository;
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
        public List<StockOutwardDetailItem> GetByStock_Outward_Id(int stock_Outward_Id)
        {
            var dbo = stock_Outward_DetailDA.Repository;
            List<StockOutwardDetailItem> lst = new List<StockOutwardDetailItem>();
            var data = stock_Outward_DetailDA.GetQuery(t => t.StockOutwardID == stock_Outward_Id).OrderByDescending(t => t.ID).ToList();
            if (data != null && data.Count > 0)
            {
                int n = 1;
                foreach (var item in data)
                {
                    lst.Add(new StockOutwardDetailItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        NumberOrder = n,
                        CreateAt = item.CreateAt,
                        Group_Name = item.StockProduct.StockProductGroup.Name,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Stock_Name = dbo.Stocks.FirstOrDefault(t => t.ID == item.Stock_ID).Name,
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
        public List<OutwardInfo> GetForReportOutward(int stock_Outward_Id, string name)
        {
            var dbo = stock_Outward_DetailDA.Repository;
            string datere = String.Format("{0:dd/MM/yyyy}", DateTime.Now.ToShortDateString());
            var lst = new List<OutwardInfo>();
            var lst1 = dbo.StockOutwards.Where(t => t.ID == stock_Outward_Id).Select(t => new { t.ID, t.Amount, t.Barcode, t.FAmount, t.Discount, t.CustomerAddress, SupplierName = t.CustomerName, t.Description, t.Reason, t.Contact, t.Contract_ID, t.RefDate }).FirstOrDefault();
            if (lst1 != null)
            {
                lst.Add(new OutwardInfo
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
                    CustomerName = name,
                    DateReport = datere

                });

            }
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    item.Items = GetByStock_Outward_Id((int)stock_Outward_Id);
                }
            }
            return lst;
        }
        public void Insert(List<StockOutwardDetail> lst)
        {
            stock_Outward_DetailDA.InsertRange(lst);
            stock_Outward_DetailDA.Save();
            foreach (var item in lst)
            {
                Trigger(item.Stock_ID, item.StockProductID, item.RefType.Value, item.Quantity, item.Amount);
            }
        }
        private void Trigger(int? stock_Id, int? product_ID, int reftype, decimal? quantity, decimal? amount)
        {
            var dbo = stock_Outward_DetailDA.Repository;
            decimal? quantityInventory = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == stock_Id).Select(t => t.Quantity).FirstOrDefault();
            decimal? amountInventory = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == stock_Id).Select(t => t.Amount).FirstOrDefault();
            var s = dbo.StockInventories.Where(t => t.StockProductID == product_ID && t.Stock_ID == stock_Id).FirstOrDefault();
            s.Amount = amountInventory - amount;
            s.Quantity = quantityInventory - quantity;
            dbo.SaveChanges();
        }

        public List<StockOutwardDetailItem> GetListProductByStringId(string[] strwhere)
        {

            var dbo = stock_Outward_DetailDA.Repository;
            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            List<StockOutwardDetailItem> lst = new List<StockOutwardDetailItem>();
            var lstProducts = dbo.StockProducts.Where(t => output.Contains((int)t.ID)).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                int n = 1;
                int rs = 0;
                if (StockOutwardDetailSV.Storage.Count() > 0)
                {
                    rs = StockOutwardDetailSV.Storage.LastOrDefault().ID;
                }
                foreach (var item in lstProducts)
                {
                    lst.Add(new StockOutwardDetailItem
                    {
                        ID = n + rs,
                        StockProductID = item.ID,
                        ProductCode = item.Code,
                        ProductName = item.Name,
                        Quantity = 0,
                        StockID = item.StockID,
                        Group_Name = item.StockProductGroup.Name,
                        Unit = dbo.StockUnits.FirstOrDefault(t => t.ID == item.Unit_ID) != null ? dbo.StockUnits.FirstOrDefault(t => t.ID == item.Unit_ID).Name : string.Empty,
                        Stock_Name = item.Stock.Name,
                        UnitPrice = GetUnitPrice((int)item.ID, (int)item.StockID),
                    });
                    n++;
                }
            }
            return lst;
        }
        public static List<StockOutwardDetailItem> Storage
        {
            get
            {
                var productchoices = HttpContext.Current.Session["StockOutwardDetailItem"];
                if (productchoices == null)
                {
                    productchoices = new List<StockOutwardDetailItem>();
                    HttpContext.Current.Session["StockOutwardDetailItem"] = productchoices;
                }

                return (List<StockOutwardDetailItem>)productchoices;
            }
            set
            {
                HttpContext.Current.Session["StockOutwardDetailItem"] = value;
            }
        }
        public static void Clear()
        {
            StockOutwardDetailSV.Storage = null;
        }
        public static void AddProduct(List<StockOutwardDetailItem> product)
        {
            var products = StockOutwardDetailSV.Storage;
            products.AddRange(product);
            StockOutwardDetailSV.Storage = products;
        }
        public static void DeleteProduct(int id)
        {

            var products = StockOutwardDetailSV.Storage;
            StockOutwardDetailItem produt = null;

            foreach (StockOutwardDetailItem p in products)
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

            StockOutwardDetailSV.Storage = products;

        }
        public List<StockOutwardDetailItem> GetAll(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year)
        {
            if (year.Trim().Equals(""))
                year = DateTime.Now.Year.ToString();
            var dbo = stock_Outward_DetailDA.Repository;
            int y1 = int.Parse(year);
            List<StockOutwardDetailItem> lst = new List<StockOutwardDetailItem>();
            var fromDateForQr = Convert.ToDateTime(fromdate);
            var toDateQr = Convert.ToDateTime(todate).AddDays(1);
            var data = stock_Outward_DetailDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.StockOutward.RefDate.Value.Year == y1
                    && (t.StockOutward.RefDate > fromDateForQr && t.StockOutward.RefDate <= toDateQr)).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockOutwardDetailItem
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
                        CurrentQty = item.CurrentQty,
                        Outward_Code = item.StockOutward.Barcode,
                        Group_Name = item.StockProduct.StockProductGroup.Name,
                        Height = item.Height,
                        IME = item.IME,
                        Outward_ID = item.StockOutwardID,
                        Cost = item.Cost,
                        Profit = item.Profit,
                        Lev1 = item.Lev1,
                        Lev2 = item.Lev2,
                        Lev3 = item.Lev3,
                        Lev4 = item.Lev4,
                        Orgin = item.Orgin,
                        ProductName = item.ProductName,
                        QtyConvert = item.QtyConvert,
                        Quantity = item.Quantity,
                        Serial = item.Serial,
                        Size = item.Size,
                        Stock_Name = dbo.Stocks.FirstOrDefault(t => t.ID == item.Stock_ID).Name,
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
        public static void UpdateProduct(StockOutwardDetailItem product)
        {
            var products = StockOutwardDetailSV.Storage;
            StockOutwardDetailItem updatingproduct = null;
            int productId = 0;
            foreach (StockOutwardDetailItem p in products)
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
            else
            {
                updatingproduct.UnitPrice = GetUnitPrice(productId, product.StockID.Value);
            }
            updatingproduct.StockID = product.StockID;
            updatingproduct.Stock_Name = product.Stock_Name;
            updatingproduct.Amount = product.Amount;
            StockOutwardDetailSV.Storage = products;
        }
    }
}

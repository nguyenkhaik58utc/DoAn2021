using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class InventorySV
    {
        private InventoryDA inventoryDA = new InventoryDA();
        public List<UnitStockItem> GetUnitStock(int stockId = 0, int productId = 0)
        {
            List<UnitStockItem> data = new List<UnitStockItem>();
            var rs = inventoryDA.Get(p => p.StockProductID == productId && p.Stock_ID == stockId).FirstOrDefault();
            if (rs != null)
            {
                decimal unitprice = 0;
                if (rs.Quantity != 0 && rs.Quantity != null && rs.Amount != null)
                {
                    unitprice = Math.Round((decimal)(rs.Amount / rs.Quantity), 2);
                }
                data.Add(new UnitStockItem
                {
                    Name = "Bình quân gia quyền",
                    Value = unitprice,
                    ID = "BQGQ"
                });
            }
            return data;
        }
        public bool GetByProductId(int StockProductID, int StockID)
        {
            var rs = inventoryDA.Get(p => p.StockProductID == StockProductID && p.Stock_ID == StockID).FirstOrDefault();
            if (rs != null)
                return true;
            return false;
        }
        public List<int> GetListProductIdInvalid()
        {
            List<int> LstPr = new List<int>();
            if (inventoryDA.GetQuery().ToList().Count > 0)
            {
                LstPr = inventoryDA.GetQuery().Select(t => (int)t.StockProductID).ToList();
            }
            return LstPr;
        }
        private decimal GetNumberBegin(int id, int productId)
        {
            var dbo = inventoryDA.Repository;
            var obj = dbo.StockInventoryDetails.Where(t => t.StockID == id && t.StockProductID == productId && (t.RefType == (int)Common.RefType.inward_Start)).OrderByDescending(t => t.ID).FirstOrDefault();
            if (obj != null)
                return (decimal)obj.Quantity;
            return 0;
        }
        private decimal GetNumberInward(int id, int productId)
        {
            var dbo = inventoryDA.Repository;
            decimal obj = 0;
            if (dbo.StockInventoryDetails.Where(t => t.StockID == id && t.StockProductID == productId && (t.RefType == (int)Common.RefType.inward || t.RefType == (int)Common.RefType.inward)).ToList().Count > 0)
            {
                obj = (decimal)dbo.StockInventoryDetails.Where(t => t.StockID == id && t.StockProductID == productId && (t.RefType == (int)Common.RefType.inward)).Sum(t => t.Quantity);
            }
            return obj;
        }
        private decimal GetNumberOutward(int id, int productId)
        {
            var dbo = inventoryDA.Repository;
            decimal obj = 0;
            if (dbo.StockInventoryDetails.Where(t => t.StockID == id && t.StockProductID == productId && t.RefType == (int)Common.RefType.outward).ToList().Count > 0)
            {
                obj = (decimal)dbo.StockInventoryDetails.Where(t => t.StockID == id && t.StockProductID == productId && t.RefType == (int)Common.RefType.outward).Sum(t => t.Quantity);
            }
            return obj;
        }
        public decimal GetCurrentQuatity(int StockProductID, int StockID)
        {
            var rs = inventoryDA.Get(p => p.StockProductID == StockProductID && p.Stock_ID == StockID).FirstOrDefault();
            if (rs != null)
                return decimal.Parse(rs.Quantity.ToString());
            return 0;
        }
        public decimal GetUnitPrice(int StockProductID, int StockID)
        {
            var rs = inventoryDA.Get(p => p.StockProductID == StockProductID && p.Stock_ID == StockID).FirstOrDefault();
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
        public int GetIdByProductIdAndStockId(int StockProductID, int StockID)
        {
            var rs = inventoryDA.Get(p => p.StockProductID == StockProductID && p.Stock_ID == StockID).FirstOrDefault();
            if (rs != null)
                return rs.ID;
            return 0;
        }
        public decimal GetQuantity(int StockProductID, int StockID)
        {
            var rs = inventoryDA.Get(p => p.StockProductID == StockProductID && p.Stock_ID == StockID).FirstOrDefault();
            if (rs != null)
                return rs.Quantity != null ? (decimal)rs.Quantity : 0;
            return 0;
        }
        public decimal GetAmount(int StockProductID, int StockID)
        {
            var rs = inventoryDA.Get(p => p.StockProductID == StockProductID && p.Stock_ID == StockID).FirstOrDefault();
            if (rs != null)
                return rs.Amount != null ? (decimal)rs.Amount : 0;
            return 0;
        }
        private string ReturnStatus(decimal quantity, int productId)
        {
            var dbo = inventoryDA.Repository;
            var obj = dbo.StockProducts.Where(t => t.ID == productId).FirstOrDefault();
            decimal quan = 0;
            decimal quanmax = 0;
            if (obj != null)
            {
                quan = obj.MinStock.ToString().Trim().Equals("") != true ? (decimal)obj.MinStock : 0;
                quanmax = obj.MaxStock.ToString().Trim().Equals("") != true ? (decimal)obj.MaxStock : 0;
            }
            if (quantity < quan)
            {
                return "<span style=\"color:red\"> Dưới mức tối thiểu </span>";
            }
            else if (quantity > quanmax)
            {
                return "<span style=\"color:blue\"> Vượt mức tối đa </span>";
            }
            else
            {
                return "";
            }

        }
        public List<InventoryItem> GetAll(ModelFilter filter, Nullable<int> StockID, int method)
        {
            List<InventoryItem> lst = new List<InventoryItem>();
            var dbo = inventoryDA.Repository;
            var data = inventoryDA.GetQuery()
                .OrderByDescending(t => t.ID)
                .Where(t => t.Stock_ID == StockID)
                .Filter(filter)
                .ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new InventoryItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        Currency_ID = item.Currency_ID,
                        Customer_ID = item.Customer_ID,
                        Description = item.Description,
                        RefDate = item.RefDate,
                        RefType = item.RefType,
                        Group_Name = item.StockProduct.StockProductGroup.Name,
                        Stock_Name = "Kho hàng: " + dbo.Stocks.FirstOrDefault(t => t.ID == (int)item.Stock_ID) != null ? dbo.Stocks.FirstOrDefault(t => t.ID == (int)item.Stock_ID).Name : string.Empty,
                        ProductName = item.StockProduct.Name,
                        ProductCode = item.StockProduct.Code,
                        StockID = item.Stock_ID,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        Batch = item.Batch,
                        ChassyNo = item.ChassyNo,
                        Color = item.Color,
                        Height = item.Height,
                        Limit = item.Limit,
                        Location = item.Location,
                        Orgin = item.Orgin,
                        Unit = dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.StockProduct.Unit_ID) != null ? dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.StockProduct.Unit_ID).Name : string.Empty,
                        UnitPrice = GetUnitPrice((int)item.StockProductID, (int)item.Stock_ID),
                        //UnitPrice = (item.stock_Inventory_Detail.LastOrDefault().E_Amt + item.Amount)/(item.stock_Inventory_Detail.LastOrDefault().E_Qty+ item.Quantity),
                        StockProductID = item.StockProductID,
                        Quantity = item.Quantity,
                        Status = ReturnStatus((decimal)item.Quantity, (int)item.StockProductID),
                        RefID = item.RefID,
                        Serial = item.Serial,
                        Size = item.Size,
                        Width = item.Width
                    });
                }
            }
            return lst;
        }
        public List<InventoryItem> GetByStockId(int StockID)
        {
            List<InventoryItem> lst = new List<InventoryItem>();
            var dbo = inventoryDA.Repository;
            var data = inventoryDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.Stock_ID == StockID).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new InventoryItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        RefDate = item.RefDate,
                        Group_Name = item.StockProduct.StockProductGroup.Name,
                        ProductName = item.StockProduct.Name,
                        ProductCode = item.StockProduct.Code,
                        StockID = item.Stock_ID,
                        Limit = item.Limit,
                        Location = item.Location,
                        Orgin = item.Orgin,
                        Unit = dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.StockProduct.Unit_ID) != null ? dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.StockProduct.Unit_ID).Name : string.Empty,
                        UnitPrice = GetUnitPrice((int)item.StockProductID, (int)item.Stock_ID),
                        StockProductID = item.StockProductID,
                        Quantity = item.Quantity
                    });
                }
            }
            return lst;
        }
        public List<InventoryItem> GetAllReportByGroupProduct(int page, int pageSize, out int total, Nullable<int> StockID, Nullable<int> group_Id, int method)
        {
            List<InventoryItem> lst = new List<InventoryItem>();
            var dbo = inventoryDA.Repository;
            var data = inventoryDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.Stock_ID == StockID && t.StockProduct.StockProductGroupID == group_Id).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new InventoryItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        Currency_ID = item.Currency_ID,
                        Customer_ID = item.Customer_ID,
                        Description = item.Description,
                        RefDate = item.RefDate,
                        RefType = item.RefType,
                        Group_Name = "Nhóm hàng: " + item.StockProduct.StockProductGroup.Name,
                        Stock_Name = dbo.Stocks.FirstOrDefault(t => t.ID == (int)item.Stock_ID) != null ? dbo.Stocks.FirstOrDefault(t => t.ID == (int)item.Stock_ID).Name : string.Empty,
                        ProductName = item.StockProduct.Name,
                        ProductCode = item.StockProduct.Code,
                        StockID = item.Stock_ID,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        Batch = item.Batch,
                        ChassyNo = item.ChassyNo,
                        Color = item.Color,
                        Height = item.Height,
                        Limit = item.Limit,
                        Location = item.Location,
                        Orgin = item.Orgin,
                        Unit = dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.StockProduct.Unit_ID) != null ? dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.StockProduct.Unit_ID).Name : string.Empty,
                        UnitPrice = GetUnitPrice((int)item.StockProductID, (int)item.Stock_ID),
                        //UnitPrice = (item.stock_Inventory_Detail.LastOrDefault().E_Amt + item.Amount)/(item.stock_Inventory_Detail.LastOrDefault().E_Qty+ item.Quantity),
                        StockProductID = item.StockProductID,
                        Quantity = item.Quantity,
                        RefID = item.RefID,
                        Serial = item.Serial,
                        Size = item.Size,
                        Width = item.Width
                    });
                }
            }
            return lst;
        }
        public List<InventoryItem> GetAllReportByQuantity(int page, int pageSize, out int total, Nullable<int> StockID, Nullable<DateTime> fromDate, Nullable<DateTime> toDate)
        {
            List<InventoryItem> lst = new List<InventoryItem>();
            var dbo = inventoryDA.Repository;
            var fromDateForQr = System.Convert.ToDateTime(fromDate);
            var toDateQr = System.Convert.ToDateTime(toDate).AddDays(1);
            var data = inventoryDA.GetQuery()
                .OrderByDescending(t => t.ID)
                .Where(t => t.Stock_ID == StockID && (t.Limit > fromDateForQr && t.Limit <= toDateQr))
                .Page(page, pageSize, out total)
                .ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new InventoryItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        Currency_ID = item.Currency_ID,
                        Customer_ID = item.Customer_ID,
                        Description = item.Description,
                        RefDate = item.RefDate,
                        RefType = item.RefType,
                        Group_Name = "Nhóm hàng: " + item.StockProduct.StockProductGroup.Name,
                        Stock_Name = dbo.Stocks.FirstOrDefault(t => t.ID == (int)item.Stock_ID) != null ? dbo.Stocks.FirstOrDefault(t => t.ID == (int)item.Stock_ID).Name : string.Empty,
                        ProductName = item.StockProduct.Name,
                        ProductCode = item.StockProduct.Code,
                        StockID = item.Stock_ID,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        Batch = item.Batch,
                        ChassyNo = item.ChassyNo,
                        Color = item.Color,
                        Height = item.Height,
                        Limit = item.Limit,
                        Location = item.Location,
                        Orgin = item.Orgin,
                        Unit = dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.StockProduct.Unit_ID) != null ? dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.StockProduct.Unit_ID).Name : string.Empty,
                        UnitPrice = GetUnitPrice((int)item.StockProductID, (int)item.Stock_ID),
                        NumberBegin = GetNumberBegin((int)item.Stock_ID, (int)item.StockProductID),
                        NumberInward = GetNumberInward((int)item.Stock_ID, (int)item.StockProductID),
                        NumberOutward = GetNumberOutward((int)item.Stock_ID, (int)item.StockProductID),
                        StockProductID = item.StockProductID,
                        Quantity = item.Quantity,
                        RefID = item.RefID,
                        Serial = item.Serial,
                        Size = item.Size,
                        Width = item.Width
                    });
                }
            }
            return lst;
        }
        public void InsertStart(List<StockInwardDetail> obj, string refID)
        {
            StockInventory objNew = new StockInventory();
            foreach (var item in obj)
            {
                if (GetByProductId((int)item.StockProductID, (int)item.Stock_ID) == false)
                {

                    objNew.RefDate = DateTime.Now;
                    objNew.RefType = item.RefType;
                    objNew.Stock_ID = item.Stock_ID;
                    objNew.StockProductID = item.StockProductID;
                    objNew.Amount = 0;
                    objNew.Batch = item.Batch;
                    objNew.ChassyNo = item.ChassyNo;
                    objNew.Color = item.Color;
                    objNew.Quantity = 0;
                    objNew.CreateAt = DateTime.Now;
                    objNew.Description = item.Description;
                    objNew.Height = item.Height;
                    objNew.Limit = DateTime.Now;
                    objNew.Orgin = item.Orgin;
                    objNew.Serial = item.Serial;
                    objNew.Size = item.Size;
                    objNew.Stock_ID = item.Stock_ID;
                    objNew.RefID = refID;
                    objNew.Width = item.Width;
                    inventoryDA.Insert(objNew);
                }
                else
                {
                    var objUpdate = inventoryDA.GetById(GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.Stock_ID));
                    objUpdate.RefDate = DateTime.Now;
                    objUpdate.RefType = item.RefType;
                    objUpdate.Stock_ID = item.Stock_ID;
                    objUpdate.Quantity = item.Quantity;
                    objUpdate.Amount = item.Amount;
                    objUpdate.StockProductID = item.StockProductID;
                    objUpdate.Description = item.Description;
                    objUpdate.Limit = DateTime.Now;
                    objUpdate.RefID = refID;
                    objUpdate.Width = item.Width;
                    inventoryDA.Update(objUpdate);
                }
                inventoryDA.Save();
            }

        }
        public void Insert(List<StockInwardDetail> obj, string refID)
        {
            StockInventory objNew = new StockInventory();
            foreach (var item in obj)
            {
                if (GetByProductId((int)item.StockProductID, (int)item.Stock_ID) == false)
                {

                    objNew.RefDate = DateTime.Now;
                    objNew.RefType = item.RefType;
                    objNew.Stock_ID = item.Stock_ID;
                    objNew.StockProductID = item.StockProductID;
                    objNew.Amount = 0;
                    objNew.Batch = item.Batch;
                    objNew.ChassyNo = item.ChassyNo;
                    objNew.Color = item.Color;
                    objNew.Quantity = 0;
                    objNew.CreateAt = DateTime.Now;
                    objNew.Description = item.Description;
                    objNew.Height = item.Height;
                    objNew.Limit = DateTime.Now;
                    objNew.Orgin = item.Orgin;
                    objNew.Serial = item.Serial;
                    objNew.Size = item.Size;
                    objNew.Stock_ID = item.Stock_ID;
                    objNew.RefID = refID;
                    objNew.Width = item.Width;
                    inventoryDA.Insert(objNew);
                }
                else
                {
                    var objUpdate = inventoryDA.GetById(GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.Stock_ID));
                    objUpdate.RefDate = DateTime.Now;
                    objUpdate.RefType = item.RefType;
                    objUpdate.Stock_ID = item.Stock_ID;
                    objUpdate.StockProductID = item.StockProductID;
                    objUpdate.Description = item.Description;
                    objUpdate.Limit = DateTime.Now;
                    objUpdate.RefID = refID;
                    objUpdate.Width = item.Width;
                    inventoryDA.Update(objUpdate);

                }
                inventoryDA.Save();
            }

        }
        public void Insert(List<StockInwardDetailItem> obj, string refID)
        {
            StockInventory objNew = new StockInventory();
            foreach (var item in obj)
            {
                if (GetByProductId((int)item.StockProductID, (int)item.StockID) == false)
                {

                    objNew.RefDate = DateTime.Now;
                    objNew.RefType = item.RefType;
                    objNew.Stock_ID = item.StockID;
                    objNew.StockProductID = item.StockProductID;
                    objNew.Amount = 0;
                    objNew.Batch = item.Batch;
                    objNew.ChassyNo = item.ChassyNo;
                    objNew.Color = item.Color;
                    objNew.Quantity = 0;
                    objNew.CreateAt = DateTime.Now;
                    objNew.Description = item.Description;
                    objNew.Height = item.Height;
                    objNew.Limit = DateTime.Now;
                    objNew.Orgin = item.Orgin;
                    objNew.Serial = item.Serial;
                    objNew.Size = item.Size;
                    objNew.Stock_ID = item.StockID;
                    objNew.RefID = refID;
                    objNew.Width = item.Width;
                    inventoryDA.Insert(objNew);
                }
                else
                {
                    var objUpdate = inventoryDA.GetById(GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.StockID));
                    objUpdate.RefDate = DateTime.Now;
                    objUpdate.RefType = item.RefType;
                    objUpdate.Stock_ID = item.StockID;
                    objUpdate.StockProductID = item.StockProductID;
                    objUpdate.Description = item.Description;
                    objUpdate.Limit = DateTime.Now;
                    objUpdate.RefID = refID;
                    objUpdate.Width = item.Width;
                    inventoryDA.Update(objUpdate);

                }
                inventoryDA.Save();
            }

        }
        public void Insert(List<StockTransferDetail> obj, string refID)
        {
            StockInventory objNew = new StockInventory();
            foreach (var item in obj)
            {
                if (GetByProductId((int)item.StockProductID, (int)item.InStock) == false)
                {
                    objNew.RefID = refID;
                    objNew.RefDate = DateTime.Now;
                    objNew.RefType = item.RefType;
                    objNew.Stock_ID = item.InStock;
                    objNew.StockProductID = item.StockProductID;
                    objNew.Batch = item.Batch;
                    objNew.Limit = DateTime.Now;
                    objNew.Quantity = 0;
                    objNew.Amount = item.Amount;
                    objNew.Width = 0;
                    objNew.Height = 0;
                    objNew.CreateAt = DateTime.Now;
                    objNew.Description = item.Description;
                    objNew.Serial = item.Serial;
                    inventoryDA.Insert(objNew);
                }
                else
                {
                    var objUpdate = inventoryDA.GetById(GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.InStock));
                    objUpdate.RefDate = DateTime.Now;
                    objUpdate.RefType = item.RefType;
                    objUpdate.Stock_ID = item.InStock;
                    objUpdate.StockProductID = item.StockProductID;
                    objUpdate.Description = item.Description;
                    objUpdate.Limit = DateTime.Now;
                    objUpdate.RefID = refID;
                    objUpdate.Width = 0;
                    inventoryDA.Update(objUpdate);
                }
                inventoryDA.Save();
            }
        }
        public void Insert(List<StockTransferDetailItem> obj, string refID)
        {
            StockInventory objNew = new StockInventory();
            foreach (var item in obj)
            {
                if (GetByProductId((int)item.StockProductID, (int)item.InStock) == false)
                {
                    objNew.RefID = refID;
                    objNew.RefDate = DateTime.Now;
                    objNew.RefType = item.RefType;
                    objNew.Stock_ID = item.InStock;
                    objNew.StockProductID = item.StockProductID;
                    objNew.Batch = item.Batch;
                    objNew.Limit = DateTime.Now;
                    objNew.Quantity = 0;
                    objNew.Amount = item.Amount;
                    objNew.Width = 0;
                    objNew.Height = 0;
                    objNew.CreateAt = DateTime.Now;
                    objNew.Description = item.Description;
                    objNew.Serial = item.Serial;
                    inventoryDA.Insert(objNew);
                }
                else
                {
                    var objUpdate = inventoryDA.GetById(GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.InStock));
                    objUpdate.RefDate = DateTime.Now;
                    objUpdate.RefType = item.RefType;
                    objUpdate.Stock_ID = item.InStock;
                    objUpdate.StockProductID = item.StockProductID;
                    objUpdate.Description = item.Description;
                    objUpdate.Limit = DateTime.Now;
                    objUpdate.RefID = refID;
                    objUpdate.Width = 0;
                    inventoryDA.Update(objUpdate);
                }
                inventoryDA.Save();
            }
        }
        public void Insert(List<StockOutwardDetail> obj, string refID)
        {

            StockInventory objNew = new StockInventory();
            foreach (var item in obj)
            {
                if (GetByProductId((int)item.StockProductID, (int)item.Stock_ID) == false)
                {

                    objNew.RefDate = DateTime.Now;
                    objNew.RefType = item.RefType;
                    objNew.Stock_ID = item.Stock_ID;
                    objNew.StockProductID = item.StockProductID;
                    objNew.Amount = item.Amount;
                    objNew.Batch = item.Batch;
                    objNew.ChassyNo = item.ChassyNo;
                    objNew.CreateAt = DateTime.Now;
                    objNew.Description = item.Description;
                    objNew.Height = item.Height;
                    objNew.Limit = DateTime.Now;
                    objNew.Orgin = item.Orgin;
                    objNew.Quantity = item.Quantity;
                    objNew.Serial = item.Serial;
                    objNew.Size = item.Size;
                    objNew.Stock_ID = item.Stock_ID;
                    objNew.RefID = refID;
                    objNew.Width = item.Width;
                    inventoryDA.Insert(objNew);
                }
                else
                {
                    var objUpdate = inventoryDA.GetById(GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.Stock_ID));
                    objUpdate.RefDate = DateTime.Now;
                    objUpdate.RefType = item.RefType;
                    objUpdate.Stock_ID = item.Stock_ID;
                    objUpdate.StockProductID = item.StockProductID;
                    objUpdate.Description = item.Description;
                    objUpdate.Limit = DateTime.Now;
                    objUpdate.RefID = refID;
                    objUpdate.Width = item.Width;
                    inventoryDA.Update(objUpdate);

                }
                inventoryDA.Save();
            }

        }
        public void Insert(List<StockOutwardDetailItem> obj, string refID)
        {

            StockInventory objNew = new StockInventory();
            foreach (var item in obj)
            {
                if (GetByProductId((int)item.StockProductID, (int)item.StockID) == false)
                {

                    objNew.RefDate = DateTime.Now;
                    objNew.RefType = item.RefType;
                    objNew.Stock_ID = item.StockID;
                    objNew.StockProductID = item.StockProductID;
                    objNew.Amount = item.Amount;
                    objNew.Batch = item.Batch;
                    objNew.ChassyNo = item.ChassyNo;
                    objNew.CreateAt = DateTime.Now;
                    objNew.Description = item.Description;
                    objNew.Height = item.Height;
                    objNew.Limit = DateTime.Now;
                    objNew.Orgin = item.Orgin;
                    objNew.Quantity = item.Quantity;
                    objNew.Serial = item.Serial;
                    objNew.Size = item.Size;
                    objNew.Stock_ID = item.StockID;
                    objNew.RefID = refID;
                    objNew.Width = item.Width;
                    inventoryDA.Insert(objNew);
                }
                else
                {
                    var objUpdate = inventoryDA.GetById(GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.StockID));
                    objUpdate.RefDate = DateTime.Now;
                    objUpdate.RefType = item.RefType;
                    objUpdate.Stock_ID = item.StockID;
                    objUpdate.StockProductID = item.StockProductID;
                    objUpdate.Description = item.Description;
                    objUpdate.Limit = DateTime.Now;
                    objUpdate.RefID = refID;
                    objUpdate.Width = item.Width;
                    inventoryDA.Update(objUpdate);

                }
                inventoryDA.Save();
            }

        }
        public void Insert(List<StockBuildDetail> obj, string refID)
        {
            StockInventory objNew = new StockInventory();
            foreach (var item in obj)
            {
                if (GetByProductId((int)item.StockProductID, (int)item.Stock_ID) == false)
                {
                    objNew.RefID = refID;
                    objNew.RefDate = DateTime.Now;
                    objNew.RefType = item.RefType;
                    objNew.Stock_ID = item.Stock_ID;
                    objNew.StockProductID = item.StockProductID;
                    objNew.Limit = DateTime.Now;
                    objNew.Quantity = item.Quantity;
                    objNew.Amount = item.Amount;
                    objNew.Width = 0;
                    objNew.Height = 0;
                    objNew.CreateAt = DateTime.Now;
                    objNew.Description = item.Description;
                    objNew.Serial = item.Serial;
                    inventoryDA.Insert(objNew);
                }
                else
                {
                    var objUpdate = inventoryDA.GetById(GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.Stock_ID));
                    objUpdate.RefDate = DateTime.Now;
                    objUpdate.RefType = item.RefType;
                    objUpdate.Stock_ID = item.Stock_ID;
                    objUpdate.StockProductID = item.StockProductID;
                    objUpdate.Description = item.Description;
                    objUpdate.Limit = DateTime.Now;
                    objUpdate.RefID = refID;
                    objUpdate.Width = 0;
                    inventoryDA.Update(objUpdate);
                }
                inventoryDA.Save();
            }
        }
        public void Insert(List<StockBuildDetailItem> obj, string refID)
        {
            StockInventory objNew = new StockInventory();
            foreach (var item in obj)
            {
                if (GetByProductId((int)item.StockProductID, (int)item.StockID) == false)
                {
                    objNew.RefID = refID;
                    objNew.RefDate = DateTime.Now;
                    objNew.RefType = item.RefType;
                    objNew.Stock_ID = item.StockID;
                    objNew.StockProductID = item.StockProductID;
                    objNew.Limit = DateTime.Now;
                    objNew.Quantity = item.Quantity;
                    objNew.Amount = item.Amount;
                    objNew.Width = 0;
                    objNew.Height = 0;
                    objNew.CreateAt = DateTime.Now;
                    objNew.Description = item.Description;
                    objNew.Serial = item.Serial;
                    inventoryDA.Insert(objNew);
                }
                else
                {
                    var objUpdate = inventoryDA.GetById(GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.StockID));
                    objUpdate.RefDate = DateTime.Now;
                    objUpdate.RefType = item.RefType;
                    objUpdate.Stock_ID = item.StockID;
                    objUpdate.StockProductID = item.StockProductID;
                    objUpdate.Description = item.Description;
                    objUpdate.Limit = DateTime.Now;
                    objUpdate.RefID = refID;
                    objUpdate.Width = 0;
                    inventoryDA.Update(objUpdate);
                }
                inventoryDA.Save();
            }
        }
        public List<StockProductItem> GetProductsByStockID(string[] strwhere)
        {
            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            var dbo = inventoryDA.Repository;
            var lsst = inventoryDA.GetQuery().Where(t => output.Contains((int)t.Stock_ID)).Select(t => t.StockProductID).ToList();
            List<StockProductItem> lst = new List<StockProductItem>();
            var lstProducts = dbo.StockProducts.Where(t => lsst.Contains((int)t.ID)).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                foreach (var item in lstProducts)
                {
                    lst.Add(new StockProductItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Code = item.Code,
                        Unit_Name = dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.Unit_ID) != null ? dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.Unit_ID).Name : string.Empty,
                        Group_Name = dbo.StockProductGroups.FirstOrDefault(t => t.ID == (int)item.StockProductGroupID) != null ? dbo.StockProductGroups.FirstOrDefault(t => t.ID == (int)item.StockProductGroupID).Name : string.Empty,
                        Description = item.Description

                    });
                }
            }
            return lst;
        }
        public List<StockProductItem> GetProductsByStockID(ModelFilter filter, int stockId)
        {
            var dbo = inventoryDA.Repository;
            var lsst = inventoryDA.GetQuery().Where(t => t.Stock_ID == stockId).Select(t => t.StockProductID).ToList();
            if (lsst.Count > 0)
            {
                var lstProducts = dbo.StockProducts.Where(t => lsst.Contains((int)t.ID))
                    .Select(item => new StockProductItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            Unit_Name = dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.Unit_ID) != null ? dbo.StockUnits.FirstOrDefault(t => t.ID == (int)item.Unit_ID).Name : string.Empty,
                            Group_Name = dbo.StockProductGroups.FirstOrDefault(t => t.ID == (int)item.StockProductGroupID) != null ? dbo.StockProductGroups.FirstOrDefault(t => t.ID == (int)item.StockProductGroupID).Name : string.Empty,
                            Description = item.Description

                        })
                        .Filter(filter)
                        .ToList();
                return lstProducts;
            }
            else
            {
                return new List<StockProductItem>();
            }
        }
    }
}

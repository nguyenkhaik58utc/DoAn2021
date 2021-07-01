using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using iDAS.Utilities;
using iDAS.DA;
using iDAS.Items;
namespace iDAS.Services
{
    public class InventoryDetailSV
    {
        private InventoryDetailDA inventory_DetailDA = new InventoryDetailDA();
        private decimal GetQuantity(int StockProductID, int StockID)
        {
            var dbo = inventory_DetailDA.Repository;
            var rs = dbo.StockInventories.Where(p => p.StockProductID == StockProductID && p.Stock_ID == StockID).FirstOrDefault();
            if (rs != null)
                return rs.Quantity != null ? (decimal)rs.Quantity : 0;
            return 0;
        }
        private decimal GetAmount(int StockProductID, int StockID)
        {
            var dbo = inventory_DetailDA.Repository;
            var rs = dbo.StockInventories.Where(p => p.StockProductID == StockProductID && p.Stock_ID == StockID).FirstOrDefault();
            if (rs != null)
                return rs.Amount != null ? (decimal)rs.Amount : 0;
            return 0;
        }
        public int GetIdByProductIdAndStockId(int StockProductID, int StockID)
        {
            var dbo = inventory_DetailDA.Repository;
            var rs = dbo.StockInventories.Where(p => p.StockProductID == StockProductID && p.Stock_ID == StockID).FirstOrDefault();
            if (rs != null)
                return rs.ID;
            return 0;
        }
        public void Insert(List<StockInwardDetail> lstObj, string refNo, int customer_ID)
        {
            List<StockInventoryDetail> newList = new List<StockInventoryDetail>();
            foreach (var item in lstObj)
            {
                newList.Add(new StockInventoryDetail()
                {
                    E_Amt = GetAmount((int)item.StockProductID, (int)item.Stock_ID),
                    E_Qty = GetQuantity((int)item.StockProductID, (int)item.Stock_ID),
                    RefNo = refNo,
                    RefDate = DateTime.Now,
                    RefType = item.RefType,
                    RefStatus = item.RefType,
                    StoreID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.Stock_ID),
                    StockID = item.Stock_ID,
                    StockProductID = item.StockProductID,
                    Product_Name = item.ProductName,
                    Supplier_ID = customer_ID,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice != null ? item.UnitPrice : 0,
                    Quantity = item.Quantity != null ? item.Quantity : 0,
                    Amount = item.Amount != null ? item.Amount : 0,
                    Price = item.UnitPrice != null ? item.UnitPrice : 0,
                    Batch = item.Batch,
                    CreateAt = DateTime.Now,
                    CreateBy = (int)item.CreateBy,
                    Description = item.Description,
                    Employee_ID = item.CreateBy,
                    StockInventoryID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.Stock_ID)
                });
            }
            inventory_DetailDA.InsertRange(newList);
            inventory_DetailDA.Save();
            
        }
        
        public void Insert(List<StockInwardDetailItem> lstObj, string refNo, int customer_ID)
        {
            List<StockInventoryDetail> newList = new List<StockInventoryDetail>();
            foreach (var item in lstObj)
            {
                newList.Add(new StockInventoryDetail()
                {
                    E_Amt = GetAmount((int)item.StockProductID, (int)item.StockID),
                    E_Qty = GetQuantity((int)item.StockProductID, (int)item.StockID),
                    RefNo = refNo,
                    RefDate = DateTime.Now,
                    RefType = item.RefType,
                    RefStatus = item.RefType,
                    StoreID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.StockID),
                    StockID = item.StockID,
                    StockProductID = item.StockProductID,
                    Product_Name = item.ProductName,
                    Supplier_ID = customer_ID,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice != null ? item.UnitPrice : 0,
                    Quantity = item.Quantity != null ? item.Quantity : 0,
                    Amount = item.Amount != null ? item.Amount : 0,
                    Price = item.UnitPrice != null ? item.UnitPrice : 0,
                    Batch = item.Batch,
                    CreateAt = DateTime.Now,
                    CreateBy = (int)item.CreateBy,
                    Description = item.Description,
                    Employee_ID = item.CreateBy,
                    StockInventoryID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.StockID)

                });
            }
            inventory_DetailDA.InsertRange(newList);
            inventory_DetailDA.Save();
        }
        public void InsertStart(List<StockInwardDetail> lstObj, string refNo)
        {
            List<StockInventoryDetail> newList = new List<StockInventoryDetail>();
            foreach (var item in lstObj)
            {
                newList.Add(new StockInventoryDetail()
                {
                    E_Amt = GetAmount((int)item.StockProductID, (int)item.Stock_ID),
                    E_Qty = GetQuantity((int)item.StockProductID, (int)item.Stock_ID),
                    RefNo = refNo,
                    RefDate = DateTime.Now,
                    RefType = item.RefType,
                    RefStatus = item.RefType,
                    StoreID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.Stock_ID),
                    StockID = item.Stock_ID,
                    StockProductID = item.StockProductID,
                    Product_Name = item.ProductName,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice != null ? item.UnitPrice : 0,
                    Quantity = item.Quantity != null ? item.Quantity : 0,
                    Amount = item.Amount != null ? item.Amount : 0,
                    Price = item.UnitPrice != null ? item.UnitPrice : 0,
                    Batch = item.Batch,
                    CreateAt = DateTime.Now,
                    CreateBy = (int)item.CreateBy,
                    Description = item.Description,
                    Employee_ID = item.CreateBy,
                    StockInventoryID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.Stock_ID)

                });
            }
            inventory_DetailDA.InsertRange(newList);
            inventory_DetailDA.Save();
        }
        public void InsertStart(List<StockInwardDetailItem> lstObj, string refNo)
        {
            List<StockInventoryDetail> newList = new List<StockInventoryDetail>();
            foreach (var item in lstObj)
            {
                newList.Add(new StockInventoryDetail()
                {
                    E_Amt = GetAmount((int)item.StockProductID, (int)item.StockID),
                    E_Qty = GetQuantity((int)item.StockProductID, (int)item.StockID),
                    RefNo = refNo,
                    RefDate = DateTime.Now,
                    RefType = item.RefType,
                    RefStatus = item.RefType,
                    StoreID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.StockID),
                    StockID = item.StockID,
                    StockProductID = item.StockProductID,
                    Product_Name = item.ProductName,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice != null ? item.UnitPrice : 0,
                    Quantity = item.Quantity != null ? item.Quantity : 0,
                    Amount = item.Amount != null ? item.Amount : 0,
                    Price = item.UnitPrice != null ? item.UnitPrice : 0,
                    Batch = item.Batch,
                    CreateAt = DateTime.Now,
                    CreateBy = (int)item.CreateBy,
                    Description = item.Description,
                    Employee_ID = item.CreateBy,
                    StockInventoryID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.StockID)

                });
            }
            inventory_DetailDA.InsertRange(newList);
            inventory_DetailDA.Save();
        }
        public void Insert(List<StockOutwardDetail> lstObj, string refNo, int customer_ID)
        {
            List<StockInventoryDetail> newList = new List<StockInventoryDetail>();
            foreach (var item in lstObj)
            {
                newList.Add(new StockInventoryDetail()
                {
                    E_Amt = GetAmount((int)item.StockProductID, (int)item.Stock_ID),
                    E_Qty = GetQuantity((int)item.StockProductID, (int)item.Stock_ID),
                    RefNo = refNo,
                    RefDate = DateTime.Now,
                    RefType = item.RefType,
                    RefStatus = item.RefType,
                    StoreID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.Stock_ID),
                    StockID = item.Stock_ID,
                    StockProductID = item.StockProductID,
                    Product_Name = item.ProductName,
                    Customer_ID = customer_ID,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Amount = item.Amount,
                    Price = item.UnitPrice,
                    Batch = item.Batch,
                    CreateAt = DateTime.Now,
                    CreateBy = (int)item.CreateBy,
                    Description = item.Description,
                    Employee_ID = item.CreateBy,
                    StockInventoryID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.Stock_ID)

                });
            }
            inventory_DetailDA.InsertRange(newList);
            inventory_DetailDA.Save();
        }
        public void Insert(List<StockOutwardDetailItem> lstObj, string refNo, int customer_ID)
        {
            List<StockInventoryDetail> newList = new List<StockInventoryDetail>();
            foreach (var item in lstObj)
            {
                newList.Add(new StockInventoryDetail()
                {
                    E_Amt = GetAmount((int)item.StockProductID, (int)item.StockID),
                    E_Qty = GetQuantity((int)item.StockProductID, (int)item.StockID),
                    RefNo = refNo,
                    RefDate = DateTime.Now,
                    RefType = item.RefType,
                    RefStatus = item.RefType,
                    StoreID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.StockID),
                    StockID = item.StockID,
                    StockProductID = item.StockProductID,
                    Product_Name = item.ProductName,
                    Customer_ID = customer_ID,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Amount = item.Amount,
                    Price = item.UnitPrice,
                    Batch = item.Batch,
                    CreateAt = DateTime.Now,
                    CreateBy = (int)item.CreateBy,
                    Description = item.Description,
                    Employee_ID = item.CreateBy,
                    StockInventoryID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.StockID)

                });
            }
            inventory_DetailDA.InsertRange(newList);
            inventory_DetailDA.Save();
        }
        public void Insert(List<StockTransferDetail> lstObj, string refNo)
        {
            List<StockInventoryDetail> newList = new List<StockInventoryDetail>();

            foreach (var item in lstObj)
            {
                newList.Add(new StockInventoryDetail()
                {
                    StoreID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.InStock),
                    RefNo = refNo,
                    RefDate = DateTime.Now,
                    RefType = item.RefType,
                    RefStatus = (int)Common.RefType.inward,
                    E_Amt = GetAmount((int)item.StockProductID, (int)item.InStock),
                    E_Qty = GetQuantity((int)item.StockProductID, (int)item.InStock),
                    StockID = item.InStock,
                    StockProductID = item.StockProductID,
                    Product_Name = item.Product_Name,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Amount = item.Amount,
                    Price = item.UnitPrice,
                    Batch = item.Batch,
                    CreateAt = DateTime.Now,
                    CreateBy = (int)item.CreateBy,
                    Description = item.Description,
                    Employee_ID = item.CreateBy,
                    StockInventoryID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.InStock)

                });
                newList.Add(new StockInventoryDetail()
                {
                    StoreID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.OutStock),
                    RefNo = refNo,
                    RefDate = DateTime.Now,
                    RefType = item.RefType,
                    RefStatus = (int)Common.RefType.outward,
                    E_Amt = GetAmount((int)item.StockProductID, (int)item.OutStock),
                    E_Qty = GetQuantity((int)item.StockProductID, (int)item.OutStock),
                    StockID = item.OutStock,
                    StockProductID = item.StockProductID,
                    Product_Name = item.Product_Name,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Amount = item.Amount,
                    Price = item.UnitPrice,
                    Batch = item.Batch,
                    CreateAt = DateTime.Now,
                    CreateBy = (int)item.CreateBy,
                    Description = item.Description,
                    Employee_ID = item.CreateBy,
                    StockInventoryID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.OutStock)

                });
            }
            inventory_DetailDA.InsertRange(newList);
            inventory_DetailDA.Save();
        }
        public void Insert(List<StockTransferDetailItem> lstObj, string refNo)
        {
            List<StockInventoryDetail> newList = new List<StockInventoryDetail>();

            foreach (var item in lstObj)
            {
                newList.Add(new StockInventoryDetail()
                {
                    StoreID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.InStock),
                    RefNo = refNo,
                    RefDate = DateTime.Now,
                    RefType = item.RefType,
                    RefStatus = (int)Common.RefType.inward,
                    E_Amt = GetAmount((int)item.StockProductID, (int)item.InStock),
                    E_Qty = GetQuantity((int)item.StockProductID, (int)item.InStock),
                    StockID = item.InStock,
                    StockProductID = item.StockProductID,
                    Product_Name = item.Product_Name,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Amount = item.Amount,
                    Price = item.UnitPrice,
                    Batch = item.Batch,
                    CreateAt = DateTime.Now,
                    CreateBy = (int)item.CreateBy,
                    Description = item.Description,
                    Employee_ID = item.CreateBy,
                    StockInventoryID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.InStock)

                });
                newList.Add(new StockInventoryDetail()
                {
                    StoreID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.OutStock),
                    RefNo = refNo,
                    RefDate = DateTime.Now,
                    RefType = item.RefType,
                    RefStatus = (int)Common.RefType.outward,
                    E_Amt = GetAmount((int)item.StockProductID, (int)item.OutStock),
                    E_Qty = GetQuantity((int)item.StockProductID, (int)item.OutStock),
                    StockID = item.OutStock,
                    StockProductID = item.StockProductID,
                    Product_Name = item.Product_Name,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Amount = item.Amount,
                    Price = item.UnitPrice,
                    Batch = item.Batch,
                    CreateAt = DateTime.Now,
                    CreateBy = (int)item.CreateBy,
                    Description = item.Description,
                    Employee_ID = item.CreateBy,
                    StockInventoryID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.OutStock)

                });
            }
            inventory_DetailDA.InsertRange(newList);
            inventory_DetailDA.Save();
        }
        public void Insert(List<StockBuildDetail> lstObj, string refNo)
        {
            List<StockInventoryDetail> newList = new List<StockInventoryDetail>();
            foreach (var item in lstObj)
            {
                newList.Add(new StockInventoryDetail()
                {
                    StoreID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.Stock_ID),
                    RefNo = refNo,
                    RefDate = DateTime.Now,
                    RefType = item.RefType,
                    RefStatus = item.RefTypeSub,
                    E_Amt = GetAmount((int)item.StockProductID, (int)item.Stock_ID),
                    E_Qty = GetQuantity((int)item.StockProductID, (int)item.Stock_ID),
                    StockID = item.Stock_ID,
                    StockProductID = item.StockProductID,
                    Product_Name = item.ProductName,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Amount = item.Amount,
                    Price = item.UnitPrice,
                    CreateAt = DateTime.Now,
                    CreateBy = (int)item.CreateBy,
                    Description = item.Description,
                    Employee_ID = item.CreateBy,
                    StockInventoryID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.Stock_ID)

                });
            }
            inventory_DetailDA.InsertRange(newList);
            inventory_DetailDA.Save();
        }
        public void Insert(List<StockBuildDetailItem> lstObj, string refNo)
        {
            List<StockInventoryDetail> newList = new List<StockInventoryDetail>();
            foreach (var item in lstObj)
            {
                newList.Add(new StockInventoryDetail()
                {
                    StoreID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.StockID),
                    RefNo = refNo,
                    RefDate = DateTime.Now,
                    RefType = item.RefType,
                    RefStatus = item.RefTypeSub,
                    E_Amt = GetAmount((int)item.StockProductID, (int)item.StockID),
                    E_Qty = GetQuantity((int)item.StockProductID, (int)item.StockID),
                    StockID = item.StockID,
                    StockProductID = item.StockProductID,
                    Product_Name = item.ProductName,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Amount = item.Amount,
                    Price = item.UnitPrice,
                    CreateAt = DateTime.Now,
                    CreateBy = (int)item.CreateBy,
                    Description = item.Description,
                    Employee_ID = item.CreateBy,
                    StockInventoryID = GetIdByProductIdAndStockId((int)item.StockProductID, (int)item.StockID)

                });
            }
            inventory_DetailDA.InsertRange(newList);
            inventory_DetailDA.Save();
        }
    }
}

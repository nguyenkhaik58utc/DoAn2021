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

    public class TransRefSV
    {
        private TransferRefDA trans_RefDA = new TransferRefDA();
        public void InsertTransfer(int refId)
        {
            var dbo = trans_RefDA.Repository;
            var obj = dbo.StockTransfers.Where(t => t.ID == refId).FirstOrDefault();
            if (obj != null)
            {
                StockTransferRef lstTrans_ref = new StockTransferRef();
                lstTrans_ref.Active = true;
                lstTrans_ref.Amount = obj.Amount;
                lstTrans_ref.FDiscount = 0;
                lstTrans_ref.FAmount = 0;
                lstTrans_ref.Discount = 0;
                lstTrans_ref.CreateAt = obj.CreateAt;
                lstTrans_ref.CreateBy = obj.CreateBy;
                lstTrans_ref.Description = obj.Description;
                lstTrans_ref.IsClose = false;
                lstTrans_ref.RefDate = DateTime.Now;
                lstTrans_ref.RefID = refId;
                lstTrans_ref.RefType = obj.RefType;
                lstTrans_ref.TransFrom = obj.FromStock_ID;
                lstTrans_ref.TransTo = obj.ToStock_ID;
                lstTrans_ref.RefOrgNo = obj.Barcode;
                lstTrans_ref.Branch_ID = obj.Branch_ID;
                lstTrans_ref.Contract_ID = obj.Contract_ID;
                lstTrans_ref.Currency_ID = obj.Currency_ID;
                lstTrans_ref.Department_ID = obj.Department_ID;
                lstTrans_ref.Employee_ID = obj.Employee_ID;
                lstTrans_ref.ExchangeRate = obj.ExchangeRate;
                trans_RefDA.Insert(lstTrans_ref);
                trans_RefDA.Save();
            }

        }
        public void InsertOutward(int refId)
        {
            var dbo = trans_RefDA.Repository;
            var obj = dbo.StockOutwards.Where(t => t.ID == refId).FirstOrDefault();
            if (obj != null)
            {
                StockTransferRef lstTrans_ref = new StockTransferRef();
                lstTrans_ref.Active = true;
                lstTrans_ref.Amount = obj.Amount;
                lstTrans_ref.CreateAt = obj.CreateAt;
                lstTrans_ref.CreateBy = obj.CreateBy;
                lstTrans_ref.Description = obj.Description;
                lstTrans_ref.IsClose = false;
                lstTrans_ref.RefDate = DateTime.Now;
                lstTrans_ref.FDiscount = 0;
                lstTrans_ref.FAmount = obj.FAmount;
                lstTrans_ref.RefID = refId;
                lstTrans_ref.RefType = obj.RefType;
                lstTrans_ref.Stock_ID = obj.Stock_ID;
                lstTrans_ref.RefOrgNo = obj.Barcode;
                lstTrans_ref.Branch_ID = obj.Branch_ID;
                lstTrans_ref.Contract_ID = obj.Contract_ID;
                lstTrans_ref.Currency_ID = obj.Currency_ID;
                lstTrans_ref.Department_ID = obj.Department_ID;
                lstTrans_ref.Employee_ID = obj.Employee_ID;
                lstTrans_ref.ExchangeRate = obj.ExchangeRate;
                trans_RefDA.Insert(lstTrans_ref);
                trans_RefDA.Save();
            }

        }
        public void InsertInward(int stock_InwardID)
        {
            var dbo = trans_RefDA.Repository;
            var obj = dbo.StockInwards.Where(t => t.ID == stock_InwardID).FirstOrDefault();
            StockTransferRef lstTrans_ref = new StockTransferRef();
            if (obj != null)
            {
                lstTrans_ref.Active = true;
                lstTrans_ref.Amount = obj.Amount;
                lstTrans_ref.CreateAt = (DateTime)obj.CreateAt;
                lstTrans_ref.CreateBy = (int)obj.CreateBy;
                lstTrans_ref.Description = obj.Description;
                lstTrans_ref.IsClose = false;
                lstTrans_ref.RefDate = DateTime.Now;
                lstTrans_ref.RefID = stock_InwardID;
                lstTrans_ref.RefType = obj.RefType;
                lstTrans_ref.Stock_ID = obj.Stock_ID;
                lstTrans_ref.RefOrgNo = obj.Barcode;
                lstTrans_ref.Branch_ID = obj.Branch_ID;
                lstTrans_ref.Contract_ID = obj.Contract_ID;
                lstTrans_ref.Currency_ID = obj.Currency_ID;
                lstTrans_ref.Department_ID = obj.Department_ID;
                lstTrans_ref.Employee_ID = obj.Employee_ID;
                lstTrans_ref.ExchangeRate = obj.ExchangeRate;
                lstTrans_ref.Discount = obj.Discount;
                lstTrans_ref.FAmount = 0;
                trans_RefDA.Insert(lstTrans_ref);
                trans_RefDA.Save();
            }
        }
        public void InsertBuild(int refId)
        {

            var dbo = trans_RefDA.Repository;
            var obj = dbo.StockBuilds.Where(t => t.ID == refId).FirstOrDefault();
            if (obj != null)
            {
                StockTransferRef lstTrans_ref = new StockTransferRef();
                lstTrans_ref.Active = true;
                lstTrans_ref.Amount = obj.Amount;
                lstTrans_ref.FAmount = obj.FAmount;
                lstTrans_ref.Discount = obj.Discount;
                lstTrans_ref.FDiscount = 0;
                lstTrans_ref.CreateAt = (DateTime)obj.CreateAt;
                lstTrans_ref.CreateBy = (int)obj.CreateBy;
                lstTrans_ref.Description = obj.Description;
                lstTrans_ref.IsClose = false;
                lstTrans_ref.RefDate = DateTime.Now;
                lstTrans_ref.RefID = refId;
                lstTrans_ref.RefType = obj.RefType;
                lstTrans_ref.RefOrgNo = obj.Barcode;
                lstTrans_ref.Currency_ID = obj.Currency_ID;
                lstTrans_ref.Department_ID = obj.Department_ID;
                lstTrans_ref.Employee_ID = obj.Employee_ID;
                lstTrans_ref.ExchangeRate = obj.ExchangeRate;
                trans_RefDA.Insert(lstTrans_ref);
                trans_RefDA.Save();
            }

        }
        public List<TransRefItem> GetForVouchersManagerment(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year, string choice)
        {

            var dbo = trans_RefDA.Repository;
            List<TransRefItem> lst = new List<TransRefItem>();
            var data = new List<StockTransferRef>();
            if (choice.Equals("App.Time"))
            {
                if (year.Trim().Equals(""))
                    year = DateTime.Now.Year.ToString();
                int y1 = int.Parse(year);
                var fromDateForQr = System.Convert.ToDateTime(fromdate);
                var toDateQr = System.Convert.ToDateTime(todate).AddDays(1);
                data = trans_RefDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.RefDate.Value.Year == y1 && (t.RefDate > fromDateForQr && t.RefDate <= toDateQr)).Page(page, pageSize, out total).ToList();
            }
            else
            {
                data = trans_RefDA.GetQuery().OrderByDescending(t => t.ID).Page(page, pageSize, out total).ToList();
            }
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new TransRefItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        Description = item.Description,
                        RefType = item.RefType,
                        RefType_Name = Common.GetRefTypeName(item.RefType.ToString().Equals("") == false ? (int)item.RefType : 30),
                        Sorted = item.Sorted,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        Employee_ID = item.Employee_ID,
                        Active = item.Active,
                        Branch_ID = item.Branch_ID,
                        Contract = item.Contract,
                        Contract_ID = item.Contract_ID,
                        Currency_ID = item.Currency_ID,
                        Department_ID = item.Department_ID,
                        Discount = item.Discount,
                        ExchangeRate = item.ExchangeRate,
                        FAmount = item.FAmount,
                        FDiscount = item.FDiscount,
                        IsClose = item.IsClose,
                        Reason = item.Reason,
                        RefDate = item.RefDate,
                        RefID = item.RefID,
                        RefOrgNo = item.RefOrgNo,
                        TransFrom = item.TransFrom,
                        TransTo = item.TransTo,
                        Employee_Name = item.CreateBy != null ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == dbo.HumanUsers.FirstOrDefault(x => x.ID == item.CreateBy).HumanEmployeeID).Name : string.Empty
                    });
                }

            }
            return lst;
        }
        public List<InventoryDetailItem> GetAll(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, Nullable<int> productId, string year)
        {
            if (year.Trim().Equals(""))
                year = DateTime.Now.Year.ToString();
            var dbo = trans_RefDA.Repository;
            int y1 = int.Parse(year);
            List<InventoryDetailItem> lst = new List<InventoryDetailItem>();
            var fromDateForQr = System.Convert.ToDateTime(fromdate);
            var toDateQr = System.Convert.ToDateTime(todate).AddDays(1);
            var data = dbo.StockInventoryDetails.Where(t => t.RefDate.Value.Year == y1 && (t.RefDate > fromDateForQr && t.RefDate <= toDateQr) && t.StockProductID == productId).OrderByDescending(t => t.ID).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new InventoryDetailItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        Description = item.Description,
                        RefType = item.RefType,
                        Sorted = item.Sorted,
                        StockID = item.StockID,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        Employee_ID = item.Employee_ID,
                        StockProductID = item.StockProduct.ID,
                        Product_Name = item.StockProduct.Name,
                        Unit = item.Unit,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Batch = item.Batch,
                        Book_ID = item.Book_ID,
                        Customer_ID = item.Customer_ID,
                        E_Amt = item.E_Amt,
                        E_Qty = item.E_Qty,
                        Price = item.Price,
                        RefDate = item.RefDate,
                        RefDetailNo = item.RefDetailNo,
                        RefNo = item.RefNo,
                        RefStatus = item.RefStatus,
                        Serial = item.Serial,
                        StoreID = item.StockID,
                        Product_Code = item.StockProduct.Code,
                        RefStatus_Name = item.RefStatus == (int)Common.RefType.outward ? "Xuất" : "Nhập",
                        Stock_Name = item.Stock.Name,
                        RefType_Name = Common.GetRefTypeName(item.RefType.ToString().Equals("") == false ? (int)item.RefType : 30),
                    });
                }

            }
            return lst;
        }
    }
}

using iDAS.Base;
using System;
using System.Collections.Generic;
using iDAS.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iDAS.Utilities;
using iDAS.DA;
using iDAS.Items;
namespace iDAS.Services
{
    public class StockInwardSV
    {
        private StockInwardDA stock_InwardDA = new StockInwardDA();
        public bool CheckBarcodeExits(string barcode)
        {
            var rs = stock_InwardDA.GetQuery().Where(t => t.Barcode.Equals(barcode)).FirstOrDefault();
            if (rs != null)
                return true;
            return false;
        }
        public List<StockInwardItem> GetAll(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year)
        {
            if (year.Trim().Equals(""))
                year = DateTime.Now.Year.ToString();
            int y1 = int.Parse(year);
            List<StockInwardItem> lst = new List<StockInwardItem>();
            var fromDateForQr = System.Convert.ToDateTime(fromdate);
            var toDateQr = System.Convert.ToDateTime(todate).AddDays(1);
            var data = stock_InwardDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.RefDate.Value.Year == y1 && (t.RefDate > fromDateForQr && t.RefDate <= toDateQr)).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockInwardItem
                    {
                        ID = item.ID,
                        Active = item.Active,
                        Amount = item.Amount,
                        Barcode = item.Barcode,
                        Branch_ID = item.Branch_ID,
                        Charge = item.Charge,
                        Contact = item.Contact,
                        Contract_ID = item.Contract_ID,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        Currency_ID = item.Currency_ID,
                        Customer_ID = item.Customer_ID,
                        CustomerAddress = item.CustomerAddress,
                        CustomerName = item.CustomerName,
                        Department_ID = item.Department_ID,
                        Description = item.Description,
                        RefType_Name = Common.GetRefTypeName(item.RefType.ToString().Equals("") == false ? (int)item.RefType : 30),
                        Discount = item.Discount,
                        Employee_ID = item.Employee_ID,
                        ExchangeRate = item.ExchangeRate,
                        FAmount = item.FAmount,
                        IsClose = item.IsClose,
                        Payment = item.Payment,
                        Reason = item.Reason,
                        Ref_OrgNo = item.Ref_OrgNo,
                        RefDate = item.RefDate,
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
        public List<StockInwardItem> GetAll(int page, int pageSize, out int total)
        {

            List<StockInwardItem> lst = new List<StockInwardItem>();
            var data = stock_InwardDA.GetQuery().OrderByDescending(t => t.ID).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockInwardItem
                    {
                        ID = item.ID,
                        Active = item.Active,
                        Amount = item.Amount,
                        Barcode = item.Barcode,
                        Branch_ID = item.Branch_ID,
                        Charge = item.Charge,
                        Contact = item.Contact,
                        Contract_ID = item.Contract_ID,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        Currency_ID = item.Currency_ID,
                        Customer_ID = item.Customer_ID,
                        CustomerAddress = item.CustomerAddress,
                        CustomerName = item.CustomerName,
                        Department_ID = item.Department_ID,
                        Description = item.Description,
                        RefType_Name = Common.GetRefTypeName(item.RefType.ToString().Equals("") == false ? (int)item.RefType : 30),
                        Discount = item.Discount,
                        Employee_ID = item.Employee_ID,
                        ExchangeRate = item.ExchangeRate,
                        FAmount = item.FAmount,
                        IsClose = item.IsClose,
                        Payment = item.Payment,
                        Reason = item.Reason,
                        Ref_OrgNo = item.Ref_OrgNo,
                        RefDate = item.RefDate,
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
        public string GetMaxCode()
        {
            var lstTmp = stock_InwardDA.GetQuery(t => t.Barcode.Contains("NK")).OrderByDescending(t => t.ID).FirstOrDefault();
            if (lstTmp != null)
                return lstTmp.Barcode;
            return string.Empty;
        }
        public string GetMaxCodeStart()
        {
            var lstTmp = stock_InwardDA.GetQuery(t => t.Barcode.Contains("DK")).OrderByDescending(t => t.ID).FirstOrDefault();
            if (lstTmp != null)
                return lstTmp.Barcode;
            return string.Empty;
        }
        public int Insert(StockInward obj, Nullable<decimal> discount, Nullable<decimal> fAmount)
        {
            StockInward objNew = new StockInward();
            objNew.Active = true;
            objNew.Amount = obj.Amount;
            objNew.Contract_ID = obj.Contract_ID;
            objNew.Barcode = obj.Barcode;
            objNew.Contact = obj.Contact;
            objNew.Discount = discount;
            objNew.FAmount = fAmount;
            objNew.Employee_ID = obj.Employee_ID;
            objNew.CreateAt = DateTime.Now;
            objNew.CreateBy = obj.CreateBy;
            objNew.Customer_ID = obj.Customer_ID;
            objNew.CustomerAddress = obj.CustomerAddress;
            objNew.CustomerName = obj.CustomerName;
            objNew.Description = obj.Description;
            objNew.Reason = obj.Reason;
            objNew.RefDate = DateTime.Now;
            objNew.Sorted = 2;
            objNew.RefType = (int)Common.RefType.inward;
            objNew.Discount = obj.Discount;
            objNew.FAmount = obj.FAmount;
            stock_InwardDA.Insert(objNew);
            stock_InwardDA.Save();
            return objNew.ID;
        }
        public int Insert(StockInwardItem obj, Nullable<decimal> discount, Nullable<decimal> fAmount)
        {
            StockInward objNew = new StockInward();
            objNew.Active = true;
            objNew.Amount = obj.Amount;
            objNew.Contract_ID = obj.Contract_ID;
            objNew.Barcode = obj.Barcode;
            objNew.Contact = obj.Contact;
            objNew.Discount = discount;
            objNew.FAmount = fAmount;
            objNew.Employee_ID = obj.Employee_ID;
            objNew.CreateAt = DateTime.Now;
            objNew.CreateBy = obj.CreateBy;
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
            objNew.RefDate = obj.RefDate;
            objNew.Sorted = 2;
            objNew.RefType = (int)Common.RefType.inward;
            objNew.Discount = obj.Discount;
            objNew.FAmount = obj.FAmount;
            stock_InwardDA.Insert(objNew);
            stock_InwardDA.Save();
            return objNew.ID;
        }
        public int InsertStart(StockInward obj, Nullable<decimal> discount, Nullable<decimal> fAmount)
        {
            StockInward objNew = new StockInward();
            objNew.Active = true;
            objNew.Amount = obj.Amount;
            objNew.Contract_ID = obj.Contract_ID;
            objNew.Barcode = obj.Barcode;
            objNew.Contact = obj.Contact;
            objNew.Discount = discount;
            objNew.FAmount = fAmount;
            objNew.Employee_ID = obj.Employee_ID;
            objNew.CreateAt = DateTime.Now;
            objNew.CreateBy = obj.CreateBy;
            objNew.Customer_ID = obj.Customer_ID;
            objNew.CustomerAddress = obj.CustomerAddress;
            objNew.CustomerName = obj.CustomerName;
            objNew.Description = obj.Description;
            objNew.Reason = obj.Reason;
            objNew.RefDate = DateTime.Now;
            objNew.Sorted = 2;
            objNew.RefType = (int)Common.RefType.inward_Start;
            objNew.Discount = obj.Discount;
            objNew.FAmount = obj.FAmount;
            stock_InwardDA.Insert(objNew);
            stock_InwardDA.Save();
            return objNew.ID;
        }
        public int InsertStart(StockInwardItem obj, Nullable<decimal> discount, Nullable<decimal> fAmount)
        {
            StockInward objNew = new StockInward();
            objNew.Active = true;
            objNew.Amount = obj.Amount;
            objNew.Contract_ID = obj.Contract_ID;
            objNew.Barcode = obj.Barcode;
            objNew.Contact = obj.Contact;
            objNew.Discount = discount;
            objNew.FAmount = fAmount;
            objNew.Employee_ID = obj.Employee_ID;
            objNew.CreateAt = DateTime.Now;
            objNew.CreateBy = obj.CreateBy;
            objNew.Customer_ID = obj.Customer_ID;
            objNew.CustomerAddress = obj.CustomerAddress;
            objNew.CustomerName = obj.CustomerName;
            objNew.Description = obj.Description;
            objNew.Reason = obj.Reason;
            objNew.RefDate = obj.RefDate;
            objNew.Sorted = 2;
            objNew.RefType = (int)Common.RefType.inward_Start;
            objNew.Discount = obj.Discount;
            objNew.FAmount = obj.FAmount;
            stock_InwardDA.Insert(objNew);
            stock_InwardDA.Save();
            return objNew.ID;
        }

    }
}

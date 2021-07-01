using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;

using System.Collections;

using iDAS.Utilities;
using iDAS.DA;
using iDAS.Items;
namespace iDAS.Services
{
    public class StockOutwardSV
    {
        private StockOutwardDA stock_OutwardDA = new StockOutwardDA();
        public bool CheckBarcodeExits(string barcode)
        {
            var rs = stock_OutwardDA.GetQuery().Where(t => t.Barcode.Equals(barcode)).FirstOrDefault();
            if (rs != null)
                return true;
            return false;
        }
        public List<StockOutwardDetailItem> GetForReport(int page, int pageSize, out int total, bool isInside, Nullable<int> provider_Id, Nullable<DateTime> fromDate, Nullable<DateTime> toDate)
        {
            var dbo = stock_OutwardDA.Repository;
            var fromDateForQr = System.Convert.ToDateTime(fromDate);
            var toDateQr = System.Convert.ToDateTime(toDate).AddDays(1);
            var data = dbo.StockOutwardDetails
                .Where(t =>t.StockOutward.IsInside == isInside)
                .Where(t => ((isInside && t.StockOutward.Department_ID == provider_Id)||(!isInside && t.StockOutward.Customer_ID == provider_Id)) && t.CreateAt > fromDateForQr && t.CreateAt <= toDateQr).OrderBy(t => t.ID)
                .Page(page, pageSize, out total).ToList();
            List<StockOutwardDetailItem> lst = new List<StockOutwardDetailItem>();
            Hashtable hs = new Hashtable();
            foreach (var item in data)
            {
                var obj = new StockOutwardDetailItem
                {
                    StockProductID = item.StockProductID,
                    CustomerName = item.StockOutward.CustomerName,
                    ProductName = item.ProductName,
                    ProductCode = item.StockProduct.Code,
                    Quantity = item.Quantity,
                    Amount = item.Amount,
                    Cost = item.Cost,
                    Unit = item.Unit,
                };

                if (hs.ContainsKey(obj.StockProductID))
                {
                    //Neu da co thi cong don amout
                    var objOld = (StockOutwardDetailItem)hs[item.StockProductID];
                    obj.Amount += objOld.Amount;
                    obj.Quantity += objOld.Quantity;
                    obj.Cost += objOld.Cost;
                    hs[item.StockProductID] = obj;

                }
                else
                {
                    //chua co thi them vao ban ghi
                    hs.Add(item.StockProductID, obj);
                }
            }
            lst = convertToList(hs);
            return lst;
        }
        public List<StockOutwardDetailItem> GetForReportForProduct(int page, int pageSize, out int total, Nullable<int> StockProductID, Nullable<DateTime> fromDate, Nullable<DateTime> toDate)
        {
            var dbo = stock_OutwardDA.Repository;
            var fromDateForQr = System.Convert.ToDateTime(fromDate);
            var toDateQr = System.Convert.ToDateTime(toDate).AddDays(1);
            var data = dbo.StockOutwardDetails.Where(t => t.StockProductID == StockProductID && t.CreateAt > fromDateForQr && t.CreateAt <= toDateQr).OrderBy(t => t.ID).Page(page, pageSize, out total).ToList();
            List<StockOutwardDetailItem> lst = new List<StockOutwardDetailItem>();
            Hashtable hs = new Hashtable();
            foreach (var item in data)
            {
                lst.Add(new StockOutwardDetailItem
                {
                    StockProductID = item.StockProductID,
                    CustomerName = item.StockOutward.CustomerName,
                    ProductName = item.ProductName,
                    ProductCode = item.StockProduct.Code,
                    Quantity = item.Quantity,
                    Amount = item.Amount,
                    Cost = item.Cost,
                    Unit = item.Unit
                });
            }
            return lst;
        }
        private List<StockOutwardDetailItem> convertToList(Hashtable hs)
        {

            List<StockOutwardDetailItem> lst = new List<StockOutwardDetailItem>();
            foreach (var item in hs.Values)
            {
                var obj = (StockOutwardDetailItem)item;
                lst.Add(obj);
            }
            return lst;

        }
        public List<StockOutwardItem> GetAll(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year)
        {
            if (year.Trim().Equals(""))
                year = DateTime.Now.Year.ToString();
            var fromDateForQr = System.Convert.ToDateTime(fromdate);
            var toDateQr = System.Convert.ToDateTime(todate).AddDays(1);
            int y1 = int.Parse(year);
            List<StockOutwardItem> lst = new List<StockOutwardItem>();
            var data = stock_OutwardDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.RefDate.Value.Year == y1 && (t.RefDate > fromDateForQr && t.RefDate <= toDateQr)).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockOutwardItem
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
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy
                    });
                }

            }
            return lst;
        }
        public List<StockOutwardItem> GetAll(int page, int pageSize, out int total)
        {

            List<StockOutwardItem> lst = new List<StockOutwardItem>();
            var data = stock_OutwardDA.GetQuery().OrderByDescending(t => t.ID).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockOutwardItem
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
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy
                    });
                }

            }
            return lst;
        }
        public string GetMaxCode()
        {
            var lstTmp = stock_OutwardDA.GetQuery(t => t.Barcode.Contains("XK")).OrderByDescending(t => t.ID).FirstOrDefault();
            if (lstTmp != null)
                return lstTmp.Barcode;
            return "";
        }
        public int Insert(StockOutward objNew)
        {
            stock_OutwardDA.Insert(objNew);
            stock_OutwardDA.Save();
            return objNew.ID;
        }

    }
}

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
    public class StockTransferSV
    {
        private StockTransferDA stock_TransferDA = new StockTransferDA();
        public bool CheckBarcodeExits(string barcode)
        {
            var rs = stock_TransferDA.GetQuery().Where(t => t.Barcode.Equals(barcode)).FirstOrDefault();
            if (rs != null)
                return true;
            return false;
        }
        public string GetMaxCode()
        {
            var lstTmp = stock_TransferDA.GetQuery().OrderByDescending(t => t.ID).FirstOrDefault();
            if (lstTmp != null)
                return lstTmp.Barcode;
            return "";
        }
        public int Insert(StockTransferItem objNew)
        {
            var item = new StockTransfer()
            {
                Active = objNew.Active,
                Amount = objNew.Amount,
                Barcode = objNew.Barcode,
                CreateAt = DateTime.Now,
                CreateBy = objNew.CreateBy,
                Description = objNew.Description,
                RefDate = DateTime.Now,
                IsClose = false,
                RefType = objNew.RefType,
                Employee_ID = objNew.Employee_ID,
                FromStock_ID = objNew.FromStockID,
                ToStock_ID = objNew.ToStockID,
                Sender_ID = objNew.Sender_ID,
                IsReview = objNew.IsReview,
            };
            stock_TransferDA.Insert(item);
            stock_TransferDA.Save();
            return item.ID;
        }
        public List<StockTransferItem> GetAll(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year)
        {
            var dbo = stock_TransferDA.Repository;
            if (year.Trim().Equals(""))
                year = DateTime.Now.Year.ToString();
            int y1 = int.Parse(year);
            List<StockTransferItem> lst = new List<StockTransferItem>();
            var fromDateForQr = Convert.ToDateTime(fromdate);
            var toDateQr = Convert.ToDateTime(todate).AddDays(1);
            var data = stock_TransferDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.RefDate.Value.Year == y1 && (t.RefDate > fromDateForQr && t.RefDate <= toDateQr)).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockTransferItem
                    {
                        ID = item.ID,
                        Active = item.Active,
                        Amount = item.Amount,
                        Barcode = item.Barcode,
                        Branch_ID = item.Branch_ID,
                        Contract_ID = item.Contract_ID,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        Currency_ID = item.Currency_ID,
                        Department_ID = item.Department_ID,
                        Description = item.Description,
                        Employee_ID = item.Employee_ID,
                        ExchangeRate = item.ExchangeRate,
                        IsClose = item.IsClose,
                        Ref_OrgNo = item.Ref_OrgNo,
                        RefDate = item.RefDate,
                        RefType = item.RefType,
                        IsReview = item.IsReview,
                        Receiver_ID = item.Receiver_ID,
                        Sender_ID = item.Sender_ID,
                        EmployeeName = dbo.HumanEmployees.FirstOrDefault(t=>t.ID==item.Employee_ID).Name,
                        Sorted = item.Sorted,
                        FromStockID = item.FromStock_ID,
                        ToStockID = item.ToStock_ID,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy
                    });
                }

            }
            return lst;
        }

    }
}

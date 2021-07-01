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
    public class StockBuildSV
    {
        private StockBuildDA stock_BuildDA = new StockBuildDA();
        private StockSV listStockService = new StockSV();
        private StockBuildDetailDA stock_Build_DetailDA = new StockBuildDetailDA();
        private TransRefSV trans_RefService = new TransRefSV();
        public void InsertBuild(int refId)
        {
            trans_RefService.InsertBuild(refId);
        }
        public bool CheckBarcodeExits(string barcode)
        {
            var rs = stock_BuildDA.GetQuery().Where(t => t.Barcode.Equals(barcode)).FirstOrDefault();
            if (rs != null)
                return true;
            return false;
        }
        public string GetMaxCode()
        {
            var lstTmp = stock_BuildDA.GetQuery().OrderByDescending(t => t.ID).FirstOrDefault();
            if (lstTmp != null)
                return lstTmp.Barcode;
            return "";
        }
        public int Insert(StockBuild objNew)
        {
            stock_BuildDA.Insert(objNew);
            stock_BuildDA.Save();
            return objNew.ID;
        }
        public int Insert(StockBuildItem item)
        {
            var result = new StockBuild()
            {
                Amount = item.Amount,
                Barcode = item.Barcode,
                Charge = item.Charge,
                Contact = item.Contact,
                Currency_ID = item.Currency_ID,
                Department_ID = item.Department_ID,
                Description = item.Description,
                Discount = item.Discount,
                Employee_ID = item.Employee_ID,
                ExchangeRate = item.ExchangeRate,
                FAmount = item.FAmount,
                Reason = item.Reason,
                Ref_OrgNo = item.Ref_OrgNo,
                RefDate = item.RefDate,
                RefType = item.RefType,
                Sorted = item.Sorted,
                Vat = item.Vat,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy,
            };
            stock_BuildDA.Insert(result);
            stock_BuildDA.Save();
            return result.ID;
        }
        public List<StockBuildItem> GetAll(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year)
        {
            var dbo = stock_BuildDA.Repository;
            if (year.Trim().Equals(""))
                year = DateTime.Now.Year.ToString();
            int y1 = int.Parse(year);
            List<StockBuildItem> lst = new List<StockBuildItem>();
            var fromDateForQr = System.Convert.ToDateTime(fromdate);
            var toDateQr = System.Convert.ToDateTime(todate).AddDays(1);
            var data = stock_BuildDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.RefDate.Value.Year == y1 && (t.RefDate > fromDateForQr && t.RefDate <= toDateQr) && t.RefType == (int)Common.RefType.build).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockBuildItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        Barcode = item.Barcode,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        Currency_ID = item.Currency_ID,
                        Department_ID = item.Department_ID,
                        Description = item.Description,
                        Employee_ID = item.Employee_ID,
                        ExchangeRate = item.ExchangeRate,
                        Ref_OrgNo = item.Ref_OrgNo,
                        RefDate = item.RefDate,
                        RefType = item.RefType,
                        Sorted = item.Sorted,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        Charge = item.Charge,
                        Contact = item.Contact,
                        Discount = item.Discount,
                        FAmount = item.FAmount,
                        Reason = item.Reason,
                        Vat = item.Vat,
                        EmployeeName= dbo.HumanEmployees.FirstOrDefault(t=>t.ID==item.Employee_ID)!=null?dbo.HumanEmployees.FirstOrDefault(t=>t.ID==item.Employee_ID).Name:string.Empty,   

                    });
                }

            }
            return lst;
        }
        public List<StockBuildItem> GetAllDismantle(int page, int pageSize, out int total, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year)
        {
            var dbo = stock_BuildDA.Repository;
            if (year.Trim().Equals(""))
                year = DateTime.Now.Year.ToString();
            int y1 = int.Parse(year);
            List<StockBuildItem> lst = new List<StockBuildItem>();
            var fromDateForQr = System.Convert.ToDateTime(fromdate);
            var toDateQr = System.Convert.ToDateTime(todate).AddDays(1);
            var data = stock_BuildDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.RefDate.Value.Year == y1 && (t.RefDate > fromDateForQr && t.RefDate <= toDateQr) && t.RefType == (int)Common.RefType.build_Dismantle).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockBuildItem
                    {
                        ID = item.ID,
                        Amount = item.Amount,
                        Barcode = item.Barcode,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        Currency_ID = item.Currency_ID,
                        Department_ID = item.Department_ID,
                        Description = item.Description,
                        Employee_ID = item.Employee_ID,
                        ExchangeRate = item.ExchangeRate,
                        Ref_OrgNo = item.Ref_OrgNo,
                        RefDate = item.RefDate,
                        RefType = item.RefType,
                        Sorted = item.Sorted,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        Charge = item.Charge,
                        Contact = item.Contact,
                        Discount = item.Discount,
                        FAmount = item.FAmount,
                        Reason = item.Reason,
                        Vat = item.Vat,
                       EmployeeName =  dbo.HumanEmployees.FirstOrDefault(t => t.ID == item.Employee_ID)!=null?dbo.HumanEmployees.FirstOrDefault(t => t.ID == item.Employee_ID).Name:string.Empty,   

                    });
                }

            }
            return lst;
        }

    }
}

using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
namespace iDAS.Services
{
    public class AdjustmentSV
    {
        private AdjustmentDA adjustmentDA = new AdjustmentDA();
        private InventorySV inventoryService = new InventorySV();
        private AdjustmentDetailSV adjustment_DetailService = new AdjustmentDetailSV();
        public List<InventoryItem> GetByStockId(int StockID)
        {
            return inventoryService.GetByStockId(StockID);
        }
        public List<AdjustmentDetailItem> GetAll(int page, int pageSize, out int total, int idAdjustment)
        {
            return adjustment_DetailService.GetAll(page, pageSize, out total, idAdjustment);
        }
        public void Delete(int idstemp)
        {
            adjustment_DetailService.Delete(idstemp);
        }
        public void Insert(List<AdjustmentDetailItem> lst)
        {
            adjustment_DetailService.Insert(lst);
        }
        public string GetMaxCode()
        {
            var lstTmp = adjustmentDA.GetQuery().OrderByDescending(t => t.ID).FirstOrDefault();
            if (lstTmp != null)
                return lstTmp.Ref_OrgNo;
            return "";
        }
        public List<AdjustmentItem> GetAll(int page, int pageSize, out int total)
        {
            StockSV listStockService = new StockSV();
            List<AdjustmentItem> lst = new List<AdjustmentItem>();
            var data = adjustmentDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.IsClose == false).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new AdjustmentItem
                    {
                        ID = item.ID,
                        Stock_Name = listStockService.GetNameStockById((int)item.StockID),
                        //  Employee_Name = item.CreateBy!= null ? userClientSV.GetFullNameById((int)item.CreateBy):"",
                        Ref_OrgNo = item.Ref_OrgNo,
                        RefDate = item.RefDate
                    });
                }

            }
            return lst;
        }
        public AdjustmentItem GetById(int Id)
        {
            var dbo = adjustmentDA.Repository;
            var result = adjustmentDA.GetQuery(i => i.ID == Id)
                .Select(item => new AdjustmentItem
                {
                    ID = item.ID,
                    Stock_Name = dbo.Stocks.FirstOrDefault(i => i.ID == item.StockID).Name,
                    Ref_OrgNo = item.Ref_OrgNo,
                    RefDate = item.RefDate
                }).FirstOrDefault();
            return result;
        }
        public int GetAdjustmentOfCurrentDay(int StockID)
        {

            DateTime crentDate = DateTime.Now;
            DateTime startdate = Convert.ToDateTime(crentDate.AddDays(-1).ToShortDateString() + " 23:59:59");
            DateTime enddate = Convert.ToDateTime(crentDate.AddDays(1).ToShortDateString() + " 00:00:00");
            var obj = adjustmentDA.GetQuery().OrderByDescending(t => t.ID).Where(t => enddate > t.RefDate && t.RefDate > startdate && t.StockID == StockID).FirstOrDefault();
            if (obj == null)
                return 0;
            return obj.ID;
        }
        public int Insert(StockAdjustment objNew)
        {
            adjustmentDA.Insert(objNew);
            adjustmentDA.Save();
            return objNew.ID;
        }
        public int Insert(AdjustmentItem item)
        {
            var objInsert = new StockAdjustment()
            {
                Accept = item.Accept,
                Active = item.Active,
                Amount = item.Amount,
                Description = item.Description,
                Employee_ID = item.Employee_ID,
                IsClose = item.IsClose,
                Ref_OrgNo = item.Ref_OrgNo,
                RefDate =(DateTime)item.RefDate,
                RefType = item.RefType,
                StockID = item.StockID,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy,
            };
            adjustmentDA.Insert(objInsert);
            adjustmentDA.Save();
            return objInsert.ID;
        }
        public void Update(int id, Nullable<decimal> amount)
        {

            StockAdjustment obj = new StockAdjustment();
            obj = adjustmentDA.GetById(id);
            obj.CreateAt = DateTime.Now;
            obj.Amount = amount;
            adjustmentDA.Update(obj);
            adjustmentDA.Save();
        }
        public void Close(int Id)
        {
           var result= adjustmentDA.GetById(Id);
            result.IsClose = true;
            adjustmentDA.Update(result);
            adjustmentDA.Save();

        }
    }
}

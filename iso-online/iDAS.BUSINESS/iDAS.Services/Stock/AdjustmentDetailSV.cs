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
    public class AdjustmentDetailSV
    {
        private AdjustmentDetailDA adjustment_DetailDA = new AdjustmentDetailDA();
        public void Insert(List<StockAdjustmentDetail> lst)
        {
            adjustment_DetailDA.InsertRange(lst);
            adjustment_DetailDA.Save();
        }
        public void Insert(List<AdjustmentDetailItem> lst)
        {
            var lsInsert = new List<StockAdjustmentDetail>();
            foreach (var item in lst)
            {
                lsInsert.Add(new StockAdjustmentDetail
                {
                    StockProductID = item.StockProductID,
                    Product_Name = item.Product_Name,
                    UnitConvert = item.UnitConvert,
                    UnitPrice = item.UnitPrice,
                    QtyConvert = item.QtyConvert,
                    StoreID = item.StoreID,
                    Description = item.Description,
                    StockAdjustmentID = item.StockAdjustmentID,
                    Amount = item.Amount,
                    CurrentQty = item.CurrentQty,
                    NewQty = item.NewQty,
                    QtyDiff = item.QtyDiff,
                    Stock_ID = item.StockID,
                    Unit_ID = item.Unit_ID,
                     CreateAt = DateTime.Now,
                    CreateBy = item.CreateBy
                }
                    );
            }
            adjustment_DetailDA.InsertRange(lsInsert);
            adjustment_DetailDA.Save();
        }
        public void Delete(int idstemp)
        {
            var lst = adjustment_DetailDA.GetQuery(t => t.StockAdjustmentID == idstemp).ToList();
            adjustment_DetailDA.DeleteRange(lst);
            adjustment_DetailDA.Save();

        }
        public List<AdjustmentDetailItem> GetAll(int page, int pageSize, out int total, int idAdjustment)
        {
            var dbo = adjustment_DetailDA.Repository;
            List<AdjustmentDetailItem> lst = new List<AdjustmentDetailItem>();
            var data = adjustment_DetailDA.GetQuery().OrderByDescending(t => t.ID).Where(t => t.StockAdjustmentID == idAdjustment).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new AdjustmentDetailItem
                    {

                        ID = item.ID,
                        StockProductID = (int)item.StockProductID,
                        Product_Code = item.StockProduct.Code,
                        Amount = item.Amount,
                        Batch = item.Batch,
                        CurrentQty = item.CurrentQty,
                        Description = item.Description,
                        Orgin = item.Orgin,
                        Group_Name = item.StockProduct.StockProductGroup.Name,
                        Product_Name = item.StockProduct.Name,
                        StockID = item.Stock_ID,
                        Unit = dbo.StockUnits.Where(t=>t.ID==item.StockProduct.Unit_ID).FirstOrDefault().Name,
                        UnitPrice = item.UnitPrice,
                        NewQty = item.NewQty,
                        Unit_ID = item.Unit_ID,
                        QtyDiff = item.QtyDiff

                    });
                }

            }
            return lst;
        }
        public static List<AdjustmentDetailItem> Storage
        {
            get
            {
                var adjustmentitem = HttpContext.Current.Session["AdjustmentDetailItem"];
                if (adjustmentitem == null)
                {
                    adjustmentitem = new List<AdjustmentDetailItem>();
                    HttpContext.Current.Session["AdjustmentDetailItem"] = adjustmentitem;
                }

                return (List<AdjustmentDetailItem>)adjustmentitem;
            }
            set
            {
                HttpContext.Current.Session["AdjustmentDetailItem"] = value;
            }
        }
        public static void Clear()
        {
            AdjustmentDetailSV.Storage = null;
        }
        public static void AddAdjustmentDetail(List<AdjustmentDetailItem> adjustment)
        {
            var adjustments = AdjustmentDetailSV.Storage;
            adjustments.AddRange(adjustment);
            AdjustmentDetailSV.Storage = adjustments;
        }
        public static void DeleteAdjustment(int id)
        {

            var adjustments = AdjustmentDetailSV.Storage;
            AdjustmentDetailItem adjustment = null;

            foreach (AdjustmentDetailItem p in adjustments)
            {
                if (p.ID == id)
                {
                    adjustment = p;
                    break;
                }
            }

            if (adjustment == null)
            {
                throw new Exception("Không tìm thấy sản phẩm");
            }

            adjustments.Remove(adjustment);

            AdjustmentDetailSV.Storage = adjustments;

        }
        public static void UpdateAdjustmentDetail(AdjustmentDetailItem adjustment)
        {

            var adjustments = AdjustmentDetailSV.Storage;
            AdjustmentDetailItem updatingadjustment = null;

            foreach (AdjustmentDetailItem p in adjustments)
            {
                if (p.ID == adjustment.ID)
                {
                    updatingadjustment = p;
                    break;
                }
            }

            if (updatingadjustment == null)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm của phiếu nhập kho");
            }
            updatingadjustment.NewQty = adjustment.NewQty;
            updatingadjustment.QtyDiff = adjustment.QtyDiff;
            AdjustmentDetailSV.Storage = adjustments;
        }
    }
}

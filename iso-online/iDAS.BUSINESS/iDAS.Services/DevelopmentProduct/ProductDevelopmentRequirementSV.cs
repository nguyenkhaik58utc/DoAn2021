using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class ProductDevelopmentRequirementSV
    {
        private ProductDevelopmentRequirementDA productDevelopmentRequirementDA = new ProductDevelopmentRequirementDA();
        private StockProductSV stockProductSV = new StockProductSV();
        private QualityPlanSV qualityPlanSV = new QualityPlanSV();
        public int Insert(QualityPlanItem item)
        {
            return qualityPlanSV.Insert(item);
        }
        public void Update(QualityPlanItem item, int userID)
        {
            qualityPlanSV.Update(item, userID);
        }
        public List<ProductDevelopmentRequirementItem> GetAll(int page, int pageSize, out int total)
        {
            var dbo = productDevelopmentRequirementDA.Repository;
            var audits = productDevelopmentRequirementDA.GetQuery()
                 .Select(item => new ProductDevelopmentRequirementItem()
                 {
                     ID = item.ID,
                     Contents = item.Contents,
                     IsWork = item.IsWork,
                     DevelopmentFromProduct = item.DevelopmentFromProduct,
                     DevelopmentFromProductName = item.DevelopmentFromProduct.HasValue ? dbo.StockProducts.FirstOrDefault(t => t.ID == item.DevelopmentFromProduct).Name : string.Empty,
                     StockProductName = item.StockProduct.Name,
                     Reason = item.Reason,
                     StockProductID = item.StockProductID,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .OrderByDescending(item => item.CreateAt)
                 .Page(page, pageSize, out total)
                 .ToList();
            return audits;
        }
        public List<ProductDevelopmentRequirementItem> GetAllPerform(int page, int pageSize, out int total)
        {
            var dbo = productDevelopmentRequirementDA.Repository;
            var audits = productDevelopmentRequirementDA.GetQuery(t => t.IsWork)
                 .Select(item => new ProductDevelopmentRequirementItem()
                 {
                     ID = item.ID,
                     Contents = item.Contents,
                     IsWork = item.IsWork,
                     DevelopmentFromProduct = item.DevelopmentFromProduct,
                     DevelopmentFromProductName = item.DevelopmentFromProduct.HasValue ? dbo.StockProducts.FirstOrDefault(t => t.ID == item.DevelopmentFromProduct).Name : string.Empty,
                     StockProductName = item.StockProduct.Name,
                     Reason = item.Reason,
                     StockProductID = item.StockProductID,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .OrderByDescending(item => item.CreateAt)
                 .Page(page, pageSize, out total)
                 .ToList();
            return audits;
        }
        public ProductDevelopmentRequirementItem GetByID(int id)
        {
            ProductDevelopmentRequirementItem obj = new ProductDevelopmentRequirementItem();
            var dbo = productDevelopmentRequirementDA.Repository;
            var procDev = productDevelopmentRequirementDA.GetById(id);
            if (procDev != null)
            {
                obj.ID = procDev.ID;
                obj.StockProductName = procDev.StockProduct.Name;
                obj.DevelopmentFromProductName = procDev.DevelopmentFromProduct.HasValue ? dbo.StockProducts.FirstOrDefault(t => t.ID == procDev.DevelopmentFromProduct).Name : string.Empty;
                obj.DevelopmentFromProduct = procDev.DevelopmentFromProduct;
                obj.StockProductID = procDev.StockProductID;
                obj.Reason = procDev.Reason;
                obj.Contents = procDev.Contents;
                obj.IsWork = procDev.IsWork;
                obj.UpdateBy = procDev.UpdateBy;
                obj.CreateBy = procDev.CreateBy;
                obj.AttachmentFiles = new AttachmentFileItem()
                {
                    Files = dbo.ProductDevelopmentRequirementAttachmentFiles.Where(i => i.ProductDevelopmentRequirementID == id)
                        .Select(i => i.AttachmentFileID).ToList()
                };
            }
            return obj;
        }
        public void Insert(ProductDevelopmentRequirementItem objNew, int userId)
        {
            var obj = new ProductDevelopmentRequirement();
            obj.DevelopmentFromProduct = objNew.DevelopmentFromProduct;
            obj.StockProductID = objNew.StockProductID;
            obj.Reason = objNew.Reason;
            obj.Contents = objNew.Contents;
            obj.IsWork = objNew.IsWork;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            productDevelopmentRequirementDA.Insert(obj);
            productDevelopmentRequirementDA.Save();
            new FileSV().Upload<ProductDevelopmentRequirementAttachmentFile>(objNew.AttachmentFiles, obj.ID);

        }
        public void Update(ProductDevelopmentRequirementItem objEdit, int userId)
        {
            var obj = productDevelopmentRequirementDA.GetById(objEdit.ID);
            obj.DevelopmentFromProduct = objEdit.DevelopmentFromProduct;
            obj.StockProductID = objEdit.StockProductID;
            obj.Reason = objEdit.Reason;
            obj.Contents = objEdit.Contents;
            obj.IsWork = objEdit.IsWork;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            productDevelopmentRequirementDA.Update(obj);
            productDevelopmentRequirementDA.Save();
            new FileSV().Upload<ProductDevelopmentRequirementAttachmentFile>(objEdit.AttachmentFiles, objEdit.ID);

        }
        public void Delete(string stringId)
        {
            var dbo = productDevelopmentRequirementDA.Repository;
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            foreach (var item in ids)
            {
                var att = dbo.ProductDevelopmentRequirementAttachmentFiles.Where(t => t.ProductDevelopmentRequirementID == (int)item).Select(t => t.AttachmentFileID).ToArray();
                if (att.Count() > 0)
                {
                    dbo.AttachmentFiles.RemoveRange(dbo.AttachmentFiles.Where(x => att.Contains(x.ID)));
                }
                dbo.ProductDevelopmentRequirementAttachmentFiles.RemoveRange(dbo.ProductDevelopmentRequirementAttachmentFiles.Where(x => x.ProductDevelopmentRequirementID == (int)item));
            }
            productDevelopmentRequirementDA.Save();
            productDevelopmentRequirementDA.DeleteRange(ids);
            productDevelopmentRequirementDA.Save();
        }
    }
}

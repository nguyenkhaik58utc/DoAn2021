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
    public class ProductModuleSV : BaseService
    {
        ProductModuleDA productModuleDA = new ProductModuleDA();

        public List<ModuleItem> GetModules(int productId, int moduleId)
        {
            var dbo = productModuleDA.Repository;
            var systemCode = dbo.WebProducts.FirstOrDefault(i => i.ID == productId).WebProductCategory.CenterSystem.SystemCode;
            var moduleIds = dbo.WebProductModules.Where(i => !i.IsDelete).Where(i => i.WebProductID == productId).Select(i => i.CenterModuleID);
            var modules = dbo.CenterModules
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDelete)
                            .Where(i => i.SystemCode == systemCode)
                            .Where(i => !moduleIds.Contains(i.ID)|| i.ID == moduleId)
                            .OrderBy(i => i.Position)
                            .Select(i => new ModuleItem()
                            {
                                ID = i.ID,
                                Name = i.Name,
                            })
                            .ToList();
            return modules;
        }
        public List<ProductModuleItem> GetProductModules(int page, int pageSize, out int total, int productId)
        {
            var ProductModules = productModuleDA.GetQuery()
                                .Where(i => i.WebProductID == productId)
                                .Where(i => !i.IsDelete)
                                .Select(i => new ProductModuleItem()
                                {
                                    ID = i.ID,
                                    CenterModuleName = i.CenterModule.Name,
                                    IsActive = i.IsActive,
                                    CreateAt = i.CreateAt,
                                    UpdateAt = i.UpdateAt,
                                })
                                .OrderByDescending(i => i.CreateAt)
                                .Page(page, pageSize, out total)
                                .ToList();
            return ProductModules;
        }
        public ProductModuleItem GetProductModule(int id)
        {
            var ProductModule = productModuleDA.GetQuery()
                            .Where(i => i.ID == id)
                            .Where(i => !i.IsDelete)
                            .Select(i => new ProductModuleItem()
                            {
                                ID = i.ID,
                                Image = new AttachmentFileItem() { ID = i.FileID },
                                CenterModuleID = i.CenterModuleID,
                                Html = i.Html,
                                Tags = i.Tags,
                                Description = i.Description,
                                Position = i.Position,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                CreateBy = i.CreateBy,
                                UpdateAt = i.UpdateAt,
                                UpdateBy = i.UpdateBy,
                            })
                            .FirstOrDefault();
            return ProductModule;
        }
        public void Insert(ProductModuleItem item)
        {
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();

            var ProductModule = new WebProductModule()
            {
                WebProductID = item.WebProductID,
                CenterModuleID = item.CenterModuleID,
                Description = item.Description,
                Html = item.Html,
                Tags = item.Tags,
                Position = item.Position,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            if (fileId != Guid.Empty)
            {
                ProductModule.FileID = fileId;
            }
            productModuleDA.Insert(ProductModule);
            productModuleDA.Save();
        }
        public void Update(ProductModuleItem item)
        {
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            var record = productModuleDA.GetById(item.ID);
            record.CenterModuleID = item.CenterModuleID;
            record.Html = item.Html;
            record.Tags = item.Tags;
            record.Position = item.Position;
            record.IsActive = item.IsActive;
            record.Description = item.Description;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            if (fileId != Guid.Empty)
            {
                record.FileID = fileId;
            }
            productModuleDA.Save();
        }
        public void Delete(int id)
        {
            var item = productModuleDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            productModuleDA.Save();
        }
    }
}

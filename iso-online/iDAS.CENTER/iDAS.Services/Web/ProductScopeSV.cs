using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class ProductScopeSV: BaseService
    {
        ProductScopeDA productScopeDA = new ProductScopeDA();

        public List<ProductScopeItem> GetProductScopes(int page, int pageSize, out int total)
        {
            var lst = productScopeDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .Select(i => new ProductScopeItem()
                        {
                            ID = i.ID,
                            NaceCodes = i.NaceCodes,
                            IsProduction = i.IsProduction,
                            IsService = i.IsService,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return lst;
        }
        public ProductScopeItem GetProductScope(int id)
        {
            var item = productScopeDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new ProductScopeItem()
                        {
                            ID = i.ID,
                            NaceCodes = i.NaceCodes,
                            IsService = i.IsService,
                            IsProduction = i.IsProduction,
                            Factory = i.Factory,
                            Description = i.Description,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            CreateBy = i.CreateBy,
                            UpdateAt = i.UpdateAt,
                            UpdateBy = i.UpdateBy,
                        })
                        .FirstOrDefault();
            return item;
        }
        public void Insert(ProductScopeItem item)
        {
            var record = new WebProductScope()
            {
                NaceCodes = item.NaceCodes,
                Description = item.Description,
                Factory = item.Factory,
                IsProduction = item.IsProduction,
                IsService = item.IsService,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            productScopeDA.Insert(record);
            productScopeDA.Save();
        }
        public void Update(ProductScopeItem item)
        {
            var record = productScopeDA.GetById(item.ID);
            record.NaceCodes = item.NaceCodes;
            record.Description = item.Description;
            record.Factory = item.Factory;
            record.IsProduction = item.IsProduction;
            record.IsService = item.IsService;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            productScopeDA.Save();
        }
        public void Delete(int id)
        {
            var item = productScopeDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            productScopeDA.Save();
        }
        public bool CheckExist(ProductScopeItem item)
        {
            var check = productScopeDA.GetQuery()
                        .Where(i => i.ID != item.ID||item.ID == 0)
                        .Where(i => i.NaceCodes.ToUpper().Equals(item.NaceCodes.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
